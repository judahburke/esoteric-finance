using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.Entities.Payment;
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

        [HttpPut(Name = "create / find recipient")]
        [ProducesResponseType(200, Type = typeof(CommonNamedEntityResponse))]
        public async Task<ObjectResult> Put(CommonNamedEntityRequest request, CancellationToken cancellationToken)
        {
            var recipient = await _dataRepository.FindByIdOrNameOrAddAsync<Recipient>(request.Id, request.Name, StringComparison.Ordinal, true, cancellationToken);

            return ObjectOk(recipient.ToCrudResponse(request.Id > 0 ? CrudStatus.READ : CrudStatus.CREATED));
        }

        [HttpGet(Name = "read recipients")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CommonNamedEntityResponse>))]
        public async Task<ObjectResult> Get(CancellationToken cancellationToken)
        {
            var recipients = await _dataRepository.GetAuditedEntities<Recipient>(x => x, cancellationToken);

            return ObjectOk(recipients.Select(t => t.ToCommonResponse()));
        }

        [HttpDelete(Name = "destroy / update recipient")]
        [ProducesResponseType(200, Type = typeof(CrudResponse))]
        public async Task<ObjectResult> Delete(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            _logger.BeginScope(request);
            _logger.LogInformation("attempting to delete {type}", nameof(Recipient));

            var response = await _dataRepository.DeleteOrReplaceRecipients(request, cancellationToken);

            return ObjectOk(response);
        }
    }
}
