using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Microsoft.AspNetCore.Mvc;

namespace Esoteric.Finance.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MethodController : CommonController
    {
        private readonly ILogger<MethodController> _logger;
        private readonly IPaymentDataRepository _dataRepository;

        public MethodController(
            ILogger<MethodController> logger,
            IPaymentDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        [HttpPut(Name = "create / find method")]
        [ProducesResponseType(200, Type = typeof(CrudResponse<int>))]
        public async Task<ObjectResult> Put(CommonNamedEntityRequest request, CancellationToken cancellationToken)
        {
            var method = await _dataRepository.FindByIdOrNameOrAddAsync<Method>(request.Id, request.Name, StringComparison.Ordinal, true, cancellationToken);

            return ObjectOk(method.ToCrudResponse(request.Id > 0 ? CrudStatus.READ : CrudStatus.CREATED));
        }

        [HttpGet(Name = "read method")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CommonNamedEntityResponse>))]
        public async Task<ObjectResult> Get(CancellationToken cancellationToken)
        {
            var methods = await _dataRepository.GetAuditedEntities<Method>(x => x, cancellationToken);

            return ObjectOk(methods.Select(m => m.ToResponse()));
        }

        [HttpDelete(Name = "destroy / update method")]
        [ProducesResponseType(200, Type = typeof(CrudResponse))]
        public async Task<ObjectResult> Delete(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            _logger.BeginScope(request);
            _logger.LogInformation("attempting to delete {type}", nameof(Method));

            var response = await _dataRepository.DeleteOrReplaceMethods(request, cancellationToken);

            return ObjectOk(response);
        }
    }
}
