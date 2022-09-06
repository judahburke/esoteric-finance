using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Abstractions.Exceptions;
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
            if (request.Id > 0)
            {
                return ObjectOk(await _dataRepository.UpdateTransaction(request, cancellationToken));
            }
            else
            {
                return ObjectOk(await _dataRepository.CreateTransaction(request, cancellationToken));
            }
        }

        [HttpGet(Name = "GetTransactions")]
        public IEnumerable<TransactionResponse> Get(CancellationToken cancellationToken)
        {
            var transactions = _dataRepository.GetTransactions(e => true, cancellationToken).GetAwaiter().GetResult();

            return transactions.Select(x => x.ToResponse()).OrderByDescending(x => x.TransactionDate).ToArray();
        }

        [HttpPost("filter", Name = "FilterTransactions")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionResponse>))]
        public async Task<ObjectResult> PostFilter(TransactionLookupRequest request, CancellationToken cancellationToken)
        {
            var transactions = await _dataRepository.GetTransactions(e => request == null || (
                (request.Id == null || e.TransactionId == request.Id.Value) &&
                (request.MinimumDate == null || e.TransactionDate >= request.MinimumDate.Value) &&
                (request.MaximumDate == null || e.TransactionDate <= request.MaximumDate.Value)), cancellationToken);

            return ObjectOk(transactions.Select(x => x.ToResponse()));
        }

        [HttpDelete(Name = "DeleteTransaction")]
        public async Task<ObjectResult> Delete(TransactionLookupRequest request, CancellationToken cancellationToken)
        {
            _logger.BeginScope(request);
            _logger.LogInformation("attempting to delete {type}", nameof(Transaction));

            if (request.Id == null || request.Id < 1)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, new { reason = "id is " + request.Id });
            }

            var response = await _dataRepository.DeleteTransaction(request.Id.Value, cancellationToken);

            return ObjectOk(response);
        }
    }
}
