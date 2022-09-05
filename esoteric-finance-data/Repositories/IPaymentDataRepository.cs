using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Abstractions.Exceptions;
using System.Linq.Expressions;

namespace Esoteric.Finance.Data.Repositories
{
    public interface IPaymentDataRepository : IDataRepository
    {
        /// <summary>
        /// Selects lookup entity by id, or by name if the former fails, or creates a new entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="saveChanges"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SubCategory> GetSubCategoryByIdOrNameOrAddAsync(int categoryId, int? id, string name, StringComparison stringComparison, bool saveChanges, CancellationToken cancellationToken);
        /// <summary>
        /// Creates <see cref="Transaction"/> along with <see cref="Recipient"/>, <see cref="Method"/>, 
        /// <see cref="Category"/> and <see cref="SubCategory"/> entities as needed
        /// </summary>
        /// <param name="transactionRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>only the <see cref="Transaction"/> with no includes</returns>
        Task<Transaction> CreateTransaction(
            TransactionRequest transactionRequest, CancellationToken cancellationToken);
        /// <summary>
        /// Selects <see cref="Transaction"/> entity for key
        /// </summary>
        /// <param name="id"><see cref="Transaction.TransactionId"/></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="HttpStatusCodeException">on key mismatch</exception>
        /// <returns><see cref="Transaction"/></returns>
        Task<Transaction> GetTransaction(long id, CancellationToken cancellationToken);
        /// <summary>
        /// Selects <see cref="Transaction"/> entities for expression
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Transaction>> GetTransactions(Expression<Func<Transaction, bool>> filter, CancellationToken cancellationToken);
        /// <summary>
        /// Updates <see cref="Transaction"/> and creates <see cref="Recipient"/>, <see cref="Method"/>, 
        /// <see cref="Category"/> and <see cref="SubCategory"/> entities as needed
        /// </summary>
        /// <param name="transactionRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>only the <see cref="Transaction"/> with no includes</returns>
        Task<Transaction> UpdateTransaction(
            TransactionRequest transactionRequest, CancellationToken cancellationToken);
        /// <summary>
        /// Deletes <see cref="Transaction"/> entity for id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CrudResponse> DeleteTransaction(long id, CancellationToken cancellation);
        /// <summary>
        /// Selects <see cref="Recipient"/> entities with foreign key relationships
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Recipient>> GetTargetsWithIncludes(Expression<Func<Recipient, bool>>? filter, CancellationToken cancellationToken);
        /// <summary>
        /// Selects <see cref="Category"/> entities with foreign key relationships
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Category>> GetCategoriesWithIncludes(Expression<Func<Category, bool>>? filter, CancellationToken cancellationToken);
        /// <summary>
        /// Selects <see cref="Method"/> entities with foreign key relationships
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Method>> GetMethodsWithIncludes(Expression<Func<Method, bool>>? filter, CancellationToken cancellationToken);
        Task<CrudResponse> DeleteOrReplaceRecipients(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken);
        Task<CrudResponse> DeleteOrReplaceMethods(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken);
        Task<CrudResponse> DeleteOrReplaceCategories(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken);
        Task<CrudResponse> DeleteOrReplaceSubCategories(Category category, CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken);
    }
}
