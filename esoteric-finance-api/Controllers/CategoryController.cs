using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Categories;
using Esoteric.Finance.Abstractions.DataTransfer.Details;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Abstractions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Esoteric.Finance.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : CommonController
    {
        #region private
        private readonly ILogger<CategoryController> _logger;
        private readonly IPaymentDataRepository _dataRepository;
        #endregion

        public CategoryController(
            ILogger<CategoryController> logger,
            IPaymentDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        [HttpPut(Name = "create / find category")]
        [ProducesResponseType(200, Type = typeof(CrudResponse<int>))]
        public async Task<ObjectResult> Put(CommonNamedEntityRequest request, CancellationToken cancellationToken)
        {
            var createdCategory = await _dataRepository.FindByIdOrNameOrAddAsync<Category>(request.Id, request.Name, StringComparison.Ordinal, true, cancellationToken);

            return ObjectOk(new CrudResponse<int>
            {
                Id = createdCategory.CategoryId,
                CrudStatus = request.Id > 0 ? CrudStatus.UPDATED : CrudStatus.CREATED
            });
        }

        [HttpGet(Name = "read category")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryResponse>))]
        public async Task<ObjectResult> Get(CancellationToken cancellationToken)
        {
            var categories = await _dataRepository.GetAuditedEntities<Category>(x => x.Include(x => x.Details), cancellationToken);

            return ObjectOk(categories.Select(c => c.ToResponse(includeDetails: true)));
        }

        [HttpDelete(Name = "destroy / update category")]
        [ProducesResponseType(200, Type = typeof(CrudResponse))]
        public async Task<ObjectResult> Delete(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            var response = await _dataRepository.DeleteOrReplaceCategories(request, cancellationToken);

            return ObjectOk(response);
        }
    }
}
