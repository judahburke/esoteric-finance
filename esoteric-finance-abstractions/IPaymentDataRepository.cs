using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Details;
using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Esoteric.Finance.Abstractions
{
    public interface IPaymentDataRepository : IDataRepository
    {
        /// <summary>
        /// Creates <see cref="Transaction"/> along with relevant entities as needed
        /// </summary>
        /// <param name="transactionRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>only the <see cref="Transaction"/> with no includes</returns>
        Task<CrudResponse<long>> CreateTransaction(TransactionRequest transactionRequest, CancellationToken cancellationToken);
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
        /// Updates <see cref="Transaction"/> and creates relevant entities as needed
        /// </summary>
        /// <param name="transactionRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CrudResponse> UpdateTransaction(TransactionRequest transactionRequest, CancellationToken cancellationToken);
        /// <summary>
        /// Deletes <see cref="Transaction"/> entity for MethodId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<CrudResponse> DeleteTransaction(long id, CancellationToken cancellation);
        /// <summary>
        /// Selects <see cref="Category"/> entities with foreign key relationships
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Category>> GetCategoriesWithIncludes(Expression<Func<Category, bool>>? filter, CancellationToken cancellationToken);
        /// <summary>
        /// Selects lookup entity by MethodId, or by name if the former fails, or creates a new entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="saveChanges"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Detail> FindDetailOrAddAsync(DetailRequest request, StringComparison stringComparison, bool saveChanges, CancellationToken cancellationToken);
        /// <summary>
        /// Selects <see cref="Initiator"/> entities with foreign key relationships
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Initiator>> GetInitiatorsWithIncludes(Expression<Func<Initiator, bool>>? filter, CancellationToken cancellationToken);
        /// <summary>
        /// Selects <see cref="Method"/> entities with foreign key relationships
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Method>> GetMethodsWithIncludes(Expression<Func<Method, bool>>? filter, CancellationToken cancellationToken);
        /// <summary>
        /// Selects <see cref="Recipient"/> entities with foreign key relationships
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<Recipient>> GetRecipientsWithIncludes(Expression<Func<Recipient, bool>>? filter, CancellationToken cancellationToken);
        Task<CrudResponse> DeleteOrReplaceCategories(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken);
        Task<CrudResponse> DeleteOrRenameDetails(DetailDeleteRequest request, CancellationToken cancellationToken);
        Task<CrudResponse> DeleteOrReplaceInitiators(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken);
        Task<CrudResponse> DeleteOrReplaceMethods(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken);
        Task<CrudResponse> DeleteOrReplaceRecipients(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken);
    }
}
