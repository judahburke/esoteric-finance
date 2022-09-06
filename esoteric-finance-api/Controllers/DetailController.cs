using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.DataTransfer;
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
    public class DetailController : CommonController
    {
        #region private
        private readonly ILogger<DetailController> _logger;
        private readonly IPaymentDataRepository _dataRepository;
        private async Task<Category> ValidatePaymentCategoryId(int id, CancellationToken cancellationToken)
        {
            var category = await _dataRepository.FindByIdAsync<Category>(id, null, cancellationToken);

            return category ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { entity = nameof(Category), id, reason = Reasons.ID_MISMATCH });
        }
        private async Task<Category> ValidateInclusivePaymentCategoryId(int id, CancellationToken cancellationToken)
        {
            var categories = await _dataRepository.GetCategoriesWithIncludes(e => e.CategoryId == id, cancellationToken);

            return categories?.SingleOrDefault() ?? throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { entity = nameof(Category), id, reason = Reasons.ID_MISMATCH });
        }
        #endregion

        public DetailController(
            ILogger<DetailController> logger,
            IPaymentDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        [HttpPut(Name = "create / find detail")]
        [ProducesResponseType(200, Type = typeof(CrudResponse<int>))]
        public async Task<ObjectResult> Put(DetailRequest request, CancellationToken cancellationToken)
        {
            var detail = await _dataRepository.FindDetailOrAddAsync(request, StringComparison.Ordinal, true, cancellationToken);

            return ObjectOk(detail.ToCrudResponse(request.Id > 0 ? CrudStatus.READ : CrudStatus.CREATED));
        }

        [HttpGet(Name = "read deatil")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DetailResponse>))]
        public async Task<ObjectResult> Get([FromQuery] int categoryId, [FromQuery] string descriptionSubstring, CancellationToken cancellationToken)
        {
            var detail = await _dataRepository.GetAuditedEntities<Detail>(q => q.Where(
                d => (categoryId < 1 || d.CategoryId == categoryId) && (descriptionSubstring == null || d.Description.Contains(descriptionSubstring))), cancellationToken);

            return ObjectOk(detail.Select(sc => sc.ToResponse(includeCategory: true)));
        }

        [HttpDelete(Name = "destroy / update detail")]
        [ProducesResponseType(200, Type = typeof(CrudResponse))]
        public async Task<ObjectResult> Delete(DetailDeleteRequest request, CancellationToken cancellationToken)
        {
            var response = await _dataRepository.DeleteOrRenameDetails(request, cancellationToken);

            return ObjectOk(response);
        }
    }
}
