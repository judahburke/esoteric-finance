using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Categories;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Abstractions.Exceptions;
using Esoteric.Finance.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
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

        public CategoryController(
            ILogger<CategoryController> logger,
            IPaymentDataRepository dataRepository)
        {
            _logger = logger;
            _dataRepository = dataRepository;
        }

        [HttpPut(Name = "create category")]
        [ProducesResponseType(200, Type = typeof(CrudResponse<int>))]
        public async Task<ObjectResult> CreateCategory(CommonNamedEntityRequest request, CancellationToken cancellationToken)
        {
            var createdCategory = await _dataRepository.FindByIdOrNameOrAddAsync<Category>(request.Id, request.Name, StringComparison.Ordinal, true, cancellationToken);

            return ObjectOk(createdCategory.CategoryId.ToCreateResponse());
        }

        [HttpGet(Name = "read category")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryResponse>))]
        public async Task<ObjectResult> GetCategories(CancellationToken cancellationToken)
        {
            var categories = await _dataRepository.GetNamedEntities<Category>(null, cancellationToken);

            return ObjectOk(categories.Select(c => c.ToCategoryResponse()));
        }

        [HttpDelete(Name = "destroy category")]
        [ProducesResponseType(200, Type = typeof(CrudResponse))]
        public async Task<ObjectResult> DeleteCategory(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            var response = await _dataRepository.DeleteOrReplaceCategories(request, cancellationToken);

            return ObjectOk(response);
        }


        [HttpPut("{paymentCategoryId}/subcategory", Name = "create subcategory")]
        [ProducesResponseType(200, Type = typeof(CrudResponse<int>))]
        public async Task<ObjectResult> CreateSubCategory([FromRoute]int paymentCategoryId, CommonNamedEntityRequest request, CancellationToken cancellationToken)
        {
            // validate category
            var category = await ValidatePaymentCategoryId(paymentCategoryId, cancellationToken);

            var subCategory = await _dataRepository.GetSubCategoryByIdOrNameOrAddAsync(category.CategoryId, request.Id, request.Name, StringComparison.Ordinal, true, cancellationToken);

            return ObjectOk(subCategory.SubCategoryId.ToCreateResponse());
        }

        [HttpGet("{paymentCategoryId}/subcategory", Name = "read subcategory")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SubCategoryResponse>))]
        public async Task<ObjectResult> GetSubCategories([FromRoute] int paymentCategoryId, CancellationToken cancellationToken)
        {
            // validate category
            var category = await ValidatePaymentCategoryId(paymentCategoryId, cancellationToken);

            var subCategories = await _dataRepository.GetNamedEntities<SubCategory>(sc => sc.CategoryId == category.CategoryId, cancellationToken);

            return ObjectOk(subCategories.Select(sc => sc.ToSubCategoryResponse()));
        }

        [HttpDelete("{paymentCategoryId}/subcategory", Name = "destroy subcategory")]
        [ProducesResponseType(200, Type = typeof(CrudResponse))]
        public async Task<ObjectResult> DeleteSubCategory([FromRoute] int paymentCategoryId, CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            var category = await ValidateInclusivePaymentCategoryId(paymentCategoryId, cancellationToken);
            var _ = request?.Name ?? request?.ReplacementName ?? throw new ArgumentNullException(nameof(request));

            var response = await _dataRepository.DeleteOrReplaceSubCategories(category, request, cancellationToken);

            return ObjectOk(response);
        }
    }
}
