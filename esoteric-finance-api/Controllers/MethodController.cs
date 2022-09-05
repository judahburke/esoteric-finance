using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Methods;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Data.Repositories;
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

        [HttpPut(Name = "create method")]
        [ProducesResponseType(200, Type = typeof(CrudResponse<int>))]
        public async Task<ObjectResult> CreateMethod(CommonNamedEntityRequest request, CancellationToken cancellationToken)
        {
            var method = await _dataRepository.FindByIdOrNameOrAddAsync<Method>(request.Id, request.Name, StringComparison.Ordinal, true, cancellationToken);

            return ObjectOk(method.MethodId.ToCreateResponse());
        }

        [HttpGet(Name = "read method")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MethodResponse>))]
        public async Task<ObjectResult> GetMethods(CancellationToken cancellationToken)
        {
            var methods = await _dataRepository.GetNamedEntities<Method>(null, cancellationToken);

            return ObjectOk(methods.Select(m => m.ToMethodResponse()));
        }

        [HttpDelete(Name = "destroy method")]
        [ProducesResponseType(200, Type = typeof(CrudResponse))]
        public async Task<ObjectResult> DeleteMethod(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            var response = await _dataRepository.DeleteOrReplaceMethods(request, cancellationToken);

            return ObjectOk(response);
        }
    }
}
