using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.Constants;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Details;
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

        public async Task<CrudResponse<long>> CreateTransaction(
            TransactionRequest? transactionRequest, CancellationToken cancellationToken)
        {
            if (transactionRequest == null)
            {
                throw new ArgumentNullException(nameof(transactionRequest));
            }

            var initiator = await FindByIdOrNameOrAddAsync<Initiator>(transactionRequest.Initiator.Id, transactionRequest.Initiator.Name, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

            var recipient = await FindByIdOrNameOrAddAsync<Recipient>(transactionRequest.Recipient.Id, transactionRequest.Recipient.Name, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

            var methods = new List<TransactionMethod>();

            foreach (var methodModel in transactionRequest.Methods)
            {
                var method = await FindByIdOrNameOrAddAsync<Method>(methodModel.Id, methodModel.Name, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

                methods.Add(new TransactionMethod
                {
                    MethodId = method.Id,
                    Amount = methodModel.Amount,
                });
            }

            var details = new List<TransactionDetail>();

            foreach (var detailModel in transactionRequest.Details)
            {
                var detail = await FindDetailOrAddAsync(detailModel, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

                details.Add(new TransactionDetail
                {
                    DetailId = detail.DetailId,
                    Multiplier = detailModel.Multiplier,
                });
            }

            var transaction = new Transaction
            {
                TransactionDate = transactionRequest.TransactionDate,
                PostedDate = transactionRequest.PostedDate,
                InitiatorId = initiator.InitiatorId,
                RecipientId = recipient.RecipientId,
            };

            _context.PaymentTransactions.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            foreach (var method in methods)
            {
                method.TransactionId = transaction.TransactionId;

                _context.PaymentTransactionMethods.Add(method);
            }

            foreach (var detail in details)
            {
                detail.TransactionId = transaction.TransactionId;

                _context.PaymentTransactionDetails.Add(detail);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return transaction.ToCrudResponse(CrudStatus.CREATED);
        }

        public async Task<Transaction> GetTransaction(long key, CancellationToken cancellationToken)
        {
            var transaction = await _context.PaymentTransactions.AsQueryable()
                .Include(e => e.Initiator)
                .Include(e => e.TransactionMethods).ThenInclude(e => e.Method)
                .Include(e => e.Recipient)
                .Include(e => e.TransactionDetails).ThenInclude(e => e.Detail).ThenInclude(e => e.Category)
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
                .Include(e => e.Initiator)
                .Include(e => e.TransactionMethods).ThenInclude(e => e.Method)
                .Include(e => e.Recipient)
                .Include(e => e.TransactionDetails).ThenInclude(e => e.Detail).ThenInclude(e => e.Category)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task<CrudResponse> UpdateTransaction(TransactionRequest transactionRequest, CancellationToken cancellationToken)
        {
            if (transactionRequest.Id == null) { throw new HttpStatusCodeException(HttpStatusCode.NotFound, "id is null"); }

            var transaction = await GetTransaction(transactionRequest.Id.Value, cancellationToken);

            // find or create initiator
            var initiator = await FindByIdOrNameOrAddAsync<Initiator>(
                transactionRequest.Initiator.Id, 
                transactionRequest.Initiator.Name, StringComparison.OrdinalIgnoreCase, true, cancellationToken);
            // change related initiator if necessary
            if (transaction.InitiatorId != initiator.Id)
            {
                transaction.InitiatorId = initiator.Id;
            }
            // find or create recipient
            var recipient = await FindByIdOrNameOrAddAsync<Recipient>(
                transactionRequest.Recipient.Id,
                transactionRequest.Recipient.Name, StringComparison.OrdinalIgnoreCase, true, cancellationToken);
            // change related recipient if necessary
            if (transaction.RecipientId != recipient.Id)
            {
                transaction.RecipientId = recipient.Id;
            }
            // save initiator and recipient changes
            await _context.SaveChangesAsync(cancellationToken);


            var transactionMethods = new List<TransactionMethod>();
            // find or create Method records
            foreach (var methodRequest in transactionRequest.Methods)
            {
                // find or create method
                var method = await FindByIdOrNameOrAddAsync<Method>(
                    methodRequest.Id,
                    methodRequest.Name, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

                transactionMethods.Add(new TransactionMethod
                {
                    MethodId = method.MethodId,
                    TransactionId = transaction.TransactionId,
                    Amount = methodRequest.Amount
                });
            }
            // delete existing related TransactionMethods
            _context.PaymentTransactionMethods.RemoveRange(transaction.TransactionMethods);
            // create related TransactionMethod records
            _context.PaymentTransactionMethods.AddRange(transactionMethods);
            // save method changes
            await _context.SaveChangesAsync(cancellationToken);

            var transactionDetails = new List<TransactionDetail>();
            // find or create Category and Detail records
            foreach (var detailRequest in transactionRequest.Details)
            {
                var detail = await FindDetailOrAddAsync(
                    detailRequest, StringComparison.OrdinalIgnoreCase, true, cancellationToken);

                transactionDetails.Add(new TransactionDetail
                {
                    DetailId = detail.DetailId,
                    TransactionId = transaction.TransactionId,
                    Multiplier = detailRequest.Multiplier,
                });
            }
            // delete existing related TransactionDetails
            _context.PaymentTransactionDetails.RemoveRange(transaction.TransactionDetails);
            // create related TransactionDetails
            _context.PaymentTransactionDetails.AddRange(transactionDetails);
            // save details changes
            await _context.SaveChangesAsync(cancellationToken);

            return new CrudResponse { CrudStatus = CrudStatus.UPDATED };
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

        public async Task<IList<Category>> GetCategoriesWithIncludes(Expression<Func<Category, bool>>? filter, CancellationToken cancellationToken)
        {
            filter ??= x => true;

            return await _context.PaymentCategories.AsQueryable()
                .Include(e => e.Details).ThenInclude(e => e.TransactionDetails).ThenInclude(e => e.Transaction)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Detail>> GetDetailsWithIncludes(Expression<Func<Detail, bool>>? filter, CancellationToken cancellationToken)
        {
            filter ??= x => true;

            return await _context.PaymentDetails.AsQueryable()
                .Include(e => e.TransactionDetails).ThenInclude(e => e.Transaction)
                .Include(e => e.Category)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<Detail> FindDetailOrAddAsync(DetailRequest request, StringComparison stringComparison, bool saveChanges, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var foundEntity = (await GetAuditedEntities<Detail>(q => q.Where(e => e.DetailId == request.Id), cancellationToken))?.FirstOrDefault();

                if (foundEntity != null) { return foundEntity; }
            }

            var category = await FindByIdOrNameOrAddAsync<Category>(request.Category.Id, request.Category.Name, stringComparison, saveChanges, cancellationToken);

            var entity = new Detail { CategoryId = category.Id, Description = request.Description };

            _context.Add(entity);

            if (saveChanges) { await _context.SaveChangesAsync(cancellationToken); }

            return entity;
        }

        public async Task<IList<Initiator>> GetInitiatorsWithIncludes(Expression<Func<Initiator, bool>>? filter, CancellationToken cancellationToken)
        {
            filter ??= x => true;

            return await _context.PaymentInitiators.AsQueryable()
                .Include(e => e.Transactions)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Method>> GetMethodsWithIncludes(Expression<Func<Method, bool>>? filter, CancellationToken cancellationToken)
        {
            filter ??= x => true;

            return await _context.PaymentMethods.AsQueryable()
                .Include(e => e.TransactionMethods).ThenInclude(e => e.Transaction)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Recipient>> GetRecipientsWithIncludes(Expression<Func<Recipient, bool>>? filter, CancellationToken cancellationToken)
        {
            filter ??= x => true;

            return await _context.PaymentRecipients.AsQueryable()
                .Include(e => e.Transactions)
                .Where(filter)
                .ToListAsync(cancellationToken);
        }

        public async Task<CrudResponse> DeleteOrReplaceCategories(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            // get all Categorys with name, case insensitive
            var categories = await GetCategoriesWithIncludes(e => e.Name.ToLowerInvariant() == request.Name.ToLowerInvariant(), cancellationToken);

            if (categories.NullOrNotAny())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = Reasons.ID_MISMATCH });
            }
            else if (categories.Any(c => c.Details.NullSafeAny()))
            {
                // get or create revised Category (if any transactions exist)
                var newCategory = await FindByIdOrNameOrAddAsync<Category>(null, request.ReplacementName, StringComparison.Ordinal, true, cancellationToken);

                // if there are any related transactions, update them to the new Category if they aren't already
                await UpdateAuditedEntities(
                    entities: categories.SelectMany(e => e.Details).Where(e => e.CategoryId != newCategory.CategoryId),
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

        public async Task<CrudResponse> DeleteOrRenameDetails(DetailDeleteRequest request, CancellationToken cancellationToken)
        {
            // matching categories
            var details = await GetDetailsWithIncludes(e => e.DetailId == request.Id, cancellationToken);

            if (details.NullOrNotAny())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = Reasons.ID_MISMATCH });
            }
            else if (details.NullSafeAny(e => e.TransactionDetails.NullSafeAny()))
            {
                // if there are any related transactions, update these entities
                await UpdateAuditedEntities(
                    entities: details.Where(e => e.TransactionDetails.NullSafeAny()),
                    update: e => e.Description = request.ReplacementDescription,
                    true,
                    cancellationToken);

                // delete entities that don't have related transactions
                await DeleteAuditedEntities(details.Where(e => e.TransactionDetails.NullOrNotAny()), true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.UPDATED };
            }
            else
            {
                await DeleteAuditedEntities(details, true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.DELETED };
            }
        }

        public async Task<CrudResponse> DeleteOrReplaceInitiators(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            // get all Initiator with name, case insensitive
            var initiators = await GetInitiatorsWithIncludes(e => e.Name.ToLowerInvariant() == request.Name.ToLowerInvariant(), cancellationToken);

            if (initiators.NullOrNotAny())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = Reasons.ID_MISMATCH });
            }
            else if (initiators.Any(m => m.Transactions.NullSafeAny()))
            {
                // get or create revised Initiator (if any transactions exist
                var newInitiator = await FindByIdOrNameOrAddAsync<Initiator>(null, request.ReplacementName, StringComparison.Ordinal, true, cancellationToken);

                // if there are any related transactions, update them to the new Initiator if they aren't already
                await UpdateAuditedEntities(
                    entities: initiators.SelectMany(e => e.Transactions).Where(e => e.InitiatorId != newInitiator.InitiatorId),
                    update: e => e.InitiatorId = newInitiator.InitiatorId,
                    true,
                    cancellationToken);

                // delete entities that aren't the revised one
                await DeleteAuditedEntities(initiators.Where(t => t.InitiatorId != newInitiator.InitiatorId), true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.UPDATED };
            }
            else
            {
                await DeleteAuditedEntities(initiators, true, cancellationToken);

                return new CrudResponse { CrudStatus = CrudStatus.DELETED };
            }
        }

        public async Task<CrudResponse> DeleteOrReplaceMethods(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            // get all Method with name, case insensitive
            var methods = await GetMethodsWithIncludes(e => e.Name.ToLowerInvariant() == request.Name.ToLowerInvariant(), cancellationToken);

            if (methods.NullOrNotAny())
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, new { reason = Reasons.ID_MISMATCH });
            }
            else if (methods.Any(m => m.TransactionMethods.NullSafeAny()))
            {
                // get or create revised Method (if any transactions exist
                var newMethod = await FindByIdOrNameOrAddAsync<Method>(null, request.ReplacementName, StringComparison.Ordinal, true, cancellationToken);

                // if there are any related transactions, update them to the new Method if they aren't already
                await UpdateAuditedEntities(
                    entities: methods.SelectMany(e => e.TransactionMethods).Where(e => e.MethodId != newMethod.MethodId),
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

        public async Task<CrudResponse> DeleteOrReplaceRecipients(CommonNamedEntityDeleteRequest request, CancellationToken cancellationToken)
        {
            // get all targets with name, case insensitive
            var targets = await GetRecipientsWithIncludes(e => e.Name.ToLowerInvariant() == request.Name.ToLowerInvariant(), cancellationToken);

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
    }
}
