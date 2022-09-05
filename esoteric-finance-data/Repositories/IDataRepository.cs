using Esoteric.Finance.Abstractions.Common;
using System.Linq.Expressions;

namespace Esoteric.Finance.Data.Repositories
{
    public interface IDataRepository
    {
        Task<IList<T>> GetAuditedEntities<T>(Expression<Func<T, bool>>? filter, CancellationToken cancellationToken) where T : CommonAuditedEntity;
        /// <summary>
        /// Selects lookup entities for expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<T>> GetNamedEntities<T>(Expression<Func<T, bool>>? filter, CancellationToken cancellationToken) where T : CommonNamedEntity;
        /// <summary>
        /// Selects lookup entity by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <remarks>logs a warning if the given name does not match the result</remarks>
        /// <returns></returns>
        Task<T?> FindByIdAsync<T>(int? id, string? name, CancellationToken cancellationToken) where T : CommonNamedEntity;
        /// <summary>
        /// Selects lookup entity by id, or by name if the former fails
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T?> FindByIdOrNameAsync<T>(int? id, string name, StringComparison stringComparison, CancellationToken cancellationToken) where T : CommonNamedEntity;
        /// <summary>
        /// Selects lookup entity by id, or by name if the former fails, or creates a new entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="saveChanges"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> FindByIdOrNameOrAddAsync<T>(int? id, string name, StringComparison stringComparison, bool saveChanges, CancellationToken cancellationToken) where T : CommonNamedEntity;
        /// <summary>
        /// Updates the expressed range of entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="saveChanges"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAuditedEntities<T>(Expression<Func<T, bool>> filter, Action<T> update, bool saveChanges, CancellationToken cancellationToken) where T : CommonAuditedEntity;
        /// <summary>
        /// Updates the given range of entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="saveChanges"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAuditedEntities<T>(IEnumerable<T> entities, Action<T> update, bool saveChanges, CancellationToken cancellationToken) where T : CommonAuditedEntity;
        /// <summary>
        /// Updates the expressed entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="saveChanges"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAuditedEntity<T>(Func<IQueryable<T>, CancellationToken, Task<T>>? filter, Action<T> update, bool saveChanges, CancellationToken cancellationToken) where T : CommonAuditedEntity;
        /// <summary>
        /// Updates the given entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <param name="saveChanges"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAuditedEntity<T>(T entity, Action<T> update, bool saveChanges, CancellationToken cancellationToken) where T : CommonAuditedEntity;
        /// <summary>
        /// Deletes the given entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="saveChanges"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAuditedEntities<T>(IEnumerable<T> entities, bool saveChanges, CancellationToken cancellationToken) where T : CommonAuditedEntity;
        /// <summary>
        /// Deletes the given entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="saveChanges"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAuditedEntity<T>(T entity, bool saveChanges, CancellationToken cancellationToken) where T : CommonAuditedEntity;
    }
}
