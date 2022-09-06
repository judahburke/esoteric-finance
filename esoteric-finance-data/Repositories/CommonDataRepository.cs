using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Esoteric.Finance.Data.Repositories
{
    internal abstract class CommonDataRepository<TContext> : IDataRepository
        where TContext : DbContext
    {
        protected readonly TContext _context;
        protected readonly ILogger _logger;

        public CommonDataRepository(TContext context, ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public virtual async Task<IList<T>> GetAuditedEntities<T>(Expression<Func<IQueryable<T>, IQueryable<T>>> query, CancellationToken cancellationToken)
            where T : CommonAuditedEntity
        {
            return await query.Compile().Invoke(_context.Set<T>().AsQueryable()).ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> FindByIdAsync<T>(int? id, string? name, CancellationToken cancellationToken)
            where T : CommonNamedEntity
        {
            _logger.BeginScope(new { name, id });

            if (id > 0)
            {
                var entityById = await _context.FindAsync<T>(new object?[] { id }, cancellationToken);

                if (entityById != null)
                {
                    if (name != null && entityById.Name != name)
                    {
                        _logger.LogWarning("{type} for {id} does not have a .Name value of {name}", typeof(T).Name, id, name);
                    }

                    return entityById;
                }
            }

            return null;
        }

        public virtual async Task<T?> FindByIdOrNameAsync<T>(int? id, string name, StringComparison stringComparison, CancellationToken cancellationToken)
            where T : CommonNamedEntity
        {
            var entity = await FindByIdAsync<T>(id, name, cancellationToken);

            if (entity == null || !entity.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                entity = await _context.Set<T>().AsQueryable().FirstOrDefaultAsync(e => e.Name == name, cancellationToken);
            }

            return entity;
        }

        public virtual async Task<T> FindByIdOrNameOrAddAsync<T>(int? id, string name, StringComparison stringComparison, bool saveChanges, CancellationToken cancellationToken)
            where T : CommonNamedEntity
        {
            var entity = await FindByIdOrNameAsync<T>(id, name, stringComparison, cancellationToken);

            if (entity == null)
            {
                entity = Activator.CreateInstance<T>();

                entity.Name = name;

                _context.Add(entity);

                if (saveChanges)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            return entity;
        }

        public virtual async Task UpdateAuditedEntities<T>(Expression<Func<T, bool>> filter, Action<T> update, bool saveChanges, CancellationToken cancellationToken)
            where T : CommonAuditedEntity
        {
            var _0 = filter ?? throw new ArgumentNullException(nameof(filter));
            var _1 = update ?? throw new ArgumentNullException(nameof(update));

            var entities = await _context.Set<T>().AsQueryable().Where(filter).ToListAsync(cancellationToken);

            foreach (var entity in entities)
            {
                update(entity);

                _context.Update(entity);
            }

            if (saveChanges)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task UpdateAuditedEntities<T>(IEnumerable<T> entities, Action<T> update, bool saveChanges, CancellationToken cancellationToken)
            where T : CommonAuditedEntity
        {
            var _0 = entities ?? throw new ArgumentNullException(nameof(entities));
            var _1 = update ?? throw new ArgumentNullException(nameof(update));

            foreach (var entity in entities)
            {
                update(entity);

                _context.Update(entity);
            }

            if (saveChanges)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task UpdateAuditedEntity<T>(Func<IQueryable<T>, CancellationToken, Task<T>>? filter, Action<T> update, bool saveChanges, CancellationToken cancellationToken) 
            where T : CommonAuditedEntity
        {
            var _0 = filter ?? throw new ArgumentNullException(nameof(filter));
            var _1 = update ?? throw new ArgumentNullException(nameof(update));

            var entity = await filter(_context.Set<T>().AsQueryable(), cancellationToken);

            update(entity);

            _context.Update(entity);

            if (saveChanges)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task UpdateAuditedEntity<T>(T entity, Action<T> update, bool saveChanges, CancellationToken cancellationToken) 
            where T : CommonAuditedEntity
        {
            var _0 = entity ?? throw new ArgumentNullException(nameof(entity));
            var _1 = update ?? throw new ArgumentNullException(nameof(update));

            update(entity);

            _context.Update(entity);

            if (saveChanges)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteAuditedEntities<T>(IEnumerable<T> entities, bool saveChanges, CancellationToken cancellationToken)
            where T : CommonAuditedEntity
        {
            _context.RemoveRange(entities);

            if (saveChanges)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task DeleteAuditedEntity<T>(T entity, bool saveChanges, CancellationToken cancellationToken) 
            where T : CommonAuditedEntity
        {
            _context.Remove(entity);

            if (saveChanges)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
