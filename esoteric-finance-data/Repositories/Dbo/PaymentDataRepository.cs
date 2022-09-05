using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Abstractions.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Net;

namespace Esoteric.Finance.Data.Repositories.Dbo
{
    internal class PaymentDataRepository : CommonDataRepository<EsotericFinanceContext>, IPaymentDataRepository
    {
        public PaymentDataRepository(EsotericFinanceContext context, ILogger<PaymentDataRepository> logger)
            : base(context, logger) { }

        public async Task<Transaction> CreateTransaction(
            TransactionRequest? transactionRequest, CancellationToken cancellationToken)
        {
            if (transactionRequest == null)
            {
                throw new ArgumentNullException(nameof(transactionRequest));
            }

            var recipient = await FindByIdOrNameOrAddAsync<Recipient>(transactionRequest.Recipient.RecipientId, transactionRequest.Recipient.Recipient, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

            var methods = new List<TransactionMethod>();

            foreach (var methodModel in transactionRequest.Methods)
            {
                var method = await FindByIdOrNameOrAddAsync<Method>(methodModel.MethodId, methodModel.Method, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

                methods.Add(new TransactionMethod
                {
                    Amount = methodModel.Amount,
                    MethodId = method.MethodId
                });
            }

            var subcategories = new List<TransactionSubCategory>();

            foreach (var categoryModel in transactionRequest.Categories)
            {
                var category = await FindByIdOrNameOrAddAsync<Category>(categoryModel.CategoryId, categoryModel.Category, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

                var subCategory = await GetSubCategoryByIdOrNameOrAddAsync(category.CategoryId, categoryModel.SubCategoryId, categoryModel.SubCategory, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

                subcategories.Add(new TransactionSubCategory
                {
                    Multiplier = categoryModel.Multiplier,
                    SubCategoryId = subCategory.SubCategoryId,
                });
            }

            var transaction = new Transaction
            {
                RecipientId = recipient.RecipientId,
                TransactionDate = transactionRequest.TransactionDate,
                PostedDate = transactionRequest.PostedDate,
            };

            _context.PaymentTransactions.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            foreach (var method in methods)
            {
                method.TransactionId = transaction.TransactionId;

                _context.PaymentTransactionMethods.Add(method);
            }

            foreach (var subcategory in subcategories)
            {
                subcategory.TransactionId = transaction.TransactionId;

                _context.PaymentTransactionSubCategories.Add(subcategory);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return transaction;
        }


        public virtual async Task<SubCategory> GetSubCategoryByIdOrNameOrAddAsync(int categoryId, int? id, string name, StringComparison stringComparison, bool saveChanges, CancellationToken cancellationToken)
        {
            var entity = await FindByIdAsync<SubCategory>(id, name, cancellationToken);

            if (entity != null && entity.CategoryId != categoryId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, Reasons.FOREIGN_KEY_MISMATCH);
            }

            if (entity == null)
            {
                Expression<Func<SubCategory, bool>> predicate 
                    = stringComparison == StringComparison.InvariantCultureIgnoreCase
                    ? sc => sc.CategoryId == categoryId && sc.Name.ToLowerInvariant() == name.ToLowerInvariant()
                    : stringComparison == StringComparison.OrdinalIgnoreCase
                        ? sc => sc.CategoryId == categoryId && sc.Name.ToLower() == name.ToLower()
                        : sc => sc.CategoryId == categoryId && sc.Name == name;

                entity = await _context.PaymentSubCategories.AsQueryable()
                    .FirstOrDefaultAsync(predicate, cancellationToken);
            }

            if (entity == null)
            {
                entity = new SubCategory { CategoryId = categoryId, Name = name };

                _context.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);
            }

            return entity;
        }

        public async Task<Transaction> GetTransaction(long key, CancellationToken cancellationToken)
        {
            var transaction = await _context.PaymentTransactions.AsQueryable()
                .Include(e => e.Recipient)
                .Include(e => e.Methods)
                .Include(e => e.SubCategories)
                .FirstOrDefaultAsync(e => e.TransactionId == key, cancellationToken);

            if (transaction == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = "id mismatch", entity = typeof(Transaction), key });
            }

            return transaction;
        }

        public async Task<IList<Transaction>> GetTransactions(Expression<Func<Transaction, bool>> filter, CancellationToken cancellationToken)
        {
            return await _context.PaymentTransactions.AsQueryable()
                .Include(e => e.Recipient)
                .Include(e => e.Methods).ThenInclude(m => m.Method)
                .Include(e => e.SubCategories).ThenInclude(m => m.SubCategory).ThenInclude(m => m.Category)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public Task<Transaction> UpdateTransaction(TransactionRequest transactionRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<CrudResponse> DeleteTransaction(long key, CancellationToken cancellationToken)
        {
            var transaction = await _context.FindAsync<Transaction>(keyValues: new object[] { key }, cancellationToken);

            if (transaction == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = "id mismatch", entity = typeof(Transaction), key });
            }

            _context.Remove(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            return new CrudResponse { CrudStatus = CrudStatus.DELETED };
        }

        public async Task<IList<Recipient>> GetTargetsWithIncludes(Expression<Func<Recipient, bool>>? filter, CancellationToken cancellationToken)
        {
            if (filter == null)
            {
                filter = x => true;
            }

            return await _context.PaymentTargets.AsQueryable()
                .Include(e => e.Transactions)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Category>> GetCategoriesWithIncludes(Expression<Func<Category, bool>>? filter, CancellationToken cancellationToken)
        {
            if (filter == null)
            {
                filter = x => true;
            }

            return await _context.PaymentCategories.AsQueryable()
                .Include(e => e.SubCategories)
                .ThenInclude(e => e.Transactions)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Method>> GetMethodsWithIncludes(Expression<Func<Method, bool>>? filter, CancellationToken cancellationToken)
        {
            if (filter == null)
            {
                filter = x => true;
            }

            return await _context.PaymentMethods.AsQueryable()
                .Include(e => e.Transactions)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<CrudResponse> DeleteOrReplaceRecipients(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            // get all targets with name, case insensitive
            var targets = await GetTargetsWithIncludes(e => e.Name.ToLowerInvariant() == request.Name.ToLowerInvariant(), cancellationToken);

            if (targets.NullOrNotAny())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = Reasons.ID_MISMATCH });
            }
            if (targets.Any(t => t.Transactions.NullSafeAny()))
            {
                // get or create revised recipient (if any transactions exist
                var newTarget = await FindByIdOrNameOrAddAsync<Recipient>(null, request.ReplacementName, StringComparison.Ordinal, true, cancellationToken);

                // if there are any related transactions, update them to the new recipient if they aren't already
                await UpdateAuditedEntities(
                    entities: targets.SelectMany(e => e.Transactions).Where(e => e.RecipientId != newTarget.RecipientId),
                    update: e => e.RecipientId = newTarget.RecipientId,
                    true,
                    cancellationToken);

                // delete entities that aren't the revised one
                await DeleteAuditedEntities(targets.Where(t => t.RecipientId != newTarget.RecipientId), true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.UPDATED };
            }
            else
            {
                await DeleteAuditedEntities(targets, true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.DELETED };
            }
        }

        public async Task<CrudResponse> DeleteOrReplaceMethods(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            // get all Methods with name, case insensitive
            var methods = await GetMethodsWithIncludes(e => e.Name.ToLowerInvariant() == request.Name.ToLowerInvariant(), cancellationToken);

            if (methods.NullOrNotAny())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = Reasons.ID_MISMATCH });
            }
            else if (methods.Any(m => m.Transactions.NullSafeAny()))
            {
                // get or create revised Method (if any transactions exist
                var newMethod = await FindByIdOrNameOrAddAsync<Method>(null, request.ReplacementName, StringComparison.Ordinal, true, cancellationToken);

                // if there are any related transactions, update them to the new Method if they aren't already
                await UpdateAuditedEntities(
                    entities: methods.SelectMany(e => e.Transactions).Where(e => e.MethodId != newMethod.MethodId),
                    update: e => e.MethodId = newMethod.MethodId,
                    true,
                    cancellationToken);

                // delete entities that aren't the revised one
                await DeleteAuditedEntities(methods.Where(t => t.MethodId != newMethod.MethodId), true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.UPDATED };
            }
            else
            {
                await DeleteAuditedEntities(methods, true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.DELETED };
            }
        }

        public async Task<CrudResponse> DeleteOrReplaceCategories(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            // get all Categorys with name, case insensitive
            var categories = await GetCategoriesWithIncludes(e => e.Name.ToLowerInvariant() == request.Name.ToLowerInvariant(), cancellationToken);

            if (categories.NullOrNotAny())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = Reasons.ID_MISMATCH });
            }
            else if (categories.Any(c => c.SubCategories.NullSafeAny()))
            {
                // get or create revised Category (if any transactions exist)
                var newCategory = await FindByIdOrNameOrAddAsync<Category>(null, request.ReplacementName, StringComparison.Ordinal, true, cancellationToken);

                // if there are any related transactions, update them to the new Category if they aren't already
                await UpdateAuditedEntities(
                    entities: categories.SelectMany(e => e.SubCategories).Where(e => e.CategoryId != newCategory.CategoryId),
                    update: e => e.CategoryId = newCategory.CategoryId,
                    true,
                    cancellationToken);

                // delete entities that aren't the revised one
                await DeleteAuditedEntities(categories.Where(t => t.CategoryId != newCategory.CategoryId), true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.UPDATED };
            }
            else
            {
                await DeleteAuditedEntities(categories, true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.DELETED };
            }
        }

        public async Task<CrudResponse> DeleteOrReplaceSubCategories(Category category, CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            // matching categories
            var subCategories = category.SubCategories?.Where(sc => sc.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));

            if (subCategories.NullOrNotAny())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = Reasons.ID_MISMATCH });
            }
            else if (subCategories.NullSafeAny(sc => sc.Transactions.NullSafeAny()))
            {
                // get or create revised Category (if any transactions exist
                var newSubCategory = await GetSubCategoryByIdOrNameOrAddAsync(category.CategoryId, null, request.ReplacementName, StringComparison.Ordinal, true, cancellationToken);

                // if there are any related transactions, update them to the new Category if they aren't already
                await UpdateAuditedEntities(
                    entities: subCategories.SelectMany(e => e.Transactions).Where(e => e.SubCategoryId != newSubCategory.SubCategoryId),
                    update: e => e.SubCategoryId = newSubCategory.SubCategoryId,
                    true,
                    cancellationToken);

                // delete entities that aren't the revised one
                await DeleteAuditedEntities(subCategories.Where(t => t.SubCategoryId != newSubCategory.SubCategoryId), true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.UPDATED };
            }
            else
            {
                await DeleteAuditedEntities(subCategories, true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.DELETED };
            }
        }
    }
}
