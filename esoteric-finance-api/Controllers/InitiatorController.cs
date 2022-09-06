using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Details;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Microsoft.AspNetCore.Mvc;

namespace Esoteric.Finance.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InitiatorController : CommonController
    {
        #region private
        private readonly ILogger<InitiatorController> _logger;
        private readonly IPaymentDataRepository _dataRepository;
        #endregion

        public InitiatorController(
            ILogger<InitiatorController> logger,
            IPaymentDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        [HttpPut(Name = "create / find initiator")]
        [ProducesResponseType(200, Type = typeof(CrudResponse<int>))]
        public async Task<ObjectResult> Put(CommonNamedEntityRequest request, CancellationToken cancellationToken)
        {
            var initiator = await _dataRepository.FindByIdOrNameOrAddAsync<Initiator>(request.Id, request.Name, StringComparison.Ordinal, true, cancellationToken);

            return ObjectOk(initiator.ToCrudResponse(request.Id > 0 ? CrudStatus.READ : CrudStatus.CREATED));
        }

        [HttpGet(Name = "read initiator")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CommonNamedEntityResponse>))]
        public async Task<ObjectResult> Get(CancellationToken cancellationToken)
        {
            var categories = await _dataRepository.GetAuditedEntities<Initiator>(x => x, cancellationToken);

            return ObjectOk(categories.Select(c => c.ToCommonResponse()));
        }


        [HttpDelete(Name = "destroy / update initiator")]
        [ProducesResponseType(200, Type = typeof(CrudResponse))]
        public async Task<ObjectResult> Delete(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            _logger.BeginScope(request);
            _logger.LogInformation("attempting to delete {type}", nameof(Initiator));

            var response = await _dataRepository.DeleteOrReplaceInitiators(request, cancellationToken);

            return ObjectOk(response);
        }
    }
}
