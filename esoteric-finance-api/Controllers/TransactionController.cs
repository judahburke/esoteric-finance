using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using Esoteric.Finance.Abstractions.Exceptions;
using Esoteric.Finance.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Esoteric.Finance.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : CommonController
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IPaymentDataRepository _dataRepository;

        public TransactionController(
            ILogger<TransactionController> logger,
            IPaymentDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        [HttpPut(Name = "PutTransaction")]
        public async Task<ObjectResult> Put(TransactionRequest request, CancellationToken cancellationToken)
        {
            if (request.TransactionId > 0)
            {
                var updatedTransaction = await _dataRepository.UpdateTransaction(request, cancellationToken);

                return ObjectOk(updatedTransaction.TransactionId.ToUpdateResponse());
            }
            else
            {
                var createdTransaction = await _dataRepository.CreateTransaction(request, cancellationToken);

                return ObjectOk(createdTransaction.TransactionId.ToCreateResponse());
            }
        }

        [HttpGet(Name = "GetTransactions")]
        public IEnumerable<TransactionResponse> Get(CancellationToken cancellationToken)
        {
            var transactions = _dataRepository.GetTransactions(e => true, cancellationToken).GetAwaiter().GetResult();

            return transactions.Select(x => x.ToTransactionResponse()).OrderByDescending(x => x.TransactionDate).ToArray();
        }

        [HttpPost("filter", Name = "FilterTransactions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionResponse>))]
        public async Task<ObjectResult> Filter(TransactionLookupRequest request, CancellationToken cancellationToken)
        {
            var transactions = await _dataRepository.GetTransactions(e => request == null || (
                (request.TransactionId == null || e.TransactionId == request.TransactionId.Value) &&
                (request.MinimumDate == null || e.TransactionDate >= request.MinimumDate.Value) &&
                (request.MaximumDate == null || e.TransactionDate <= request.MaximumDate.Value)), cancellationToken);

            return ObjectOk(transactions.Select(x => x.ToTransactionResponse()));
        }

        [HttpDelete(Name = "DeleteTransaction")]
        public async Task<ObjectResult> DeleteTransaction(TransactionLookupRequest request, CancellationToken cancellationToken)
        {
            if (request.TransactionId == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, new { reason = "id is null" });
            }

            var response = await _dataRepository.DeleteTransaction(request.TransactionId.Value, cancellationToken);

            return ObjectOk(response);
        }
    }
}
