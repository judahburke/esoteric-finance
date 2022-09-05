using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Recipients;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Esoteric.Finance.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipientController : CommonController
    {
        private readonly ILogger<RecipientController> _logger;
        private readonly IPaymentDataRepository _dataRepository;

        public RecipientController(
            ILogger<RecipientController> logger,
            IPaymentDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        [HttpPut(Name = "create recipient")]
        [ProducesResponseType(200, Type = typeof(RecipientResponse))]
        public async Task<ObjectResult> CreateRecipient(CommonNamedEntityRequest request, CancellationToken cancellationToken)
        {
            var recipient = await _dataRepository.FindByIdOrNameOrAddAsync<Recipient>(request.Id, request.Name, StringComparison.Ordinal, true, cancellationToken);

            return ObjectOk(recipient.RecipientId.ToCreateResponse());
        }

        [HttpGet(Name = "read recipients")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RecipientResponse>))]
        public async Task<ObjectResult> GetRecipients(CancellationToken cancellationToken)
        {
            var recipients = await _dataRepository.GetNamedEntities<Recipient>(null, cancellationToken);

            return ObjectOk(recipients.Select(t => t.ToRecipientResponse()));
        }

        [HttpDelete(Name = "destroy recipient")]
        [ProducesResponseType(200, Type = typeof(CrudResponse))]
        public async Task<ObjectResult> DeleteRecipient(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            var response = await _dataRepository.DeleteOrReplaceRecipients(request, cancellationToken);

            return ObjectOk(response);
        }
    }
}
