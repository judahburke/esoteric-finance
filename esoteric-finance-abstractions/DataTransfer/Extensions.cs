using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.Common;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Categories;
using Esoteric.Finance.Abstractions.DataTransfer.Details;
using Esoteric.Finance.Abstractions.DataTransfer.Methods;
using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using Esoteric.Finance.Abstractions.Entities.Payment;
using System.Linq;
using System.Net.Http.Headers;

namespace Esoteric.Finance.Abstractions.DataTransfer
{
    public static class Extensions
    {
        public static CrudResponse<int> ToCrudResponse(this Category entity, Constants.CrudStatus crudStatus)
            => new CrudResponse<int> { Id = entity.CategoryId, CrudStatus = crudStatus };
        public static CrudResponse<long> ToCrudResponse(this Detail entity, Constants.CrudStatus crudStatus)
            => new CrudResponse<long> { Id = entity.DetailId, CrudStatus = crudStatus };
        public static CrudResponse<int> ToCrudResponse(this Initiator entity, Constants.CrudStatus crudStatus)
            => new CrudResponse<int> { Id = entity.InitiatorId, CrudStatus = crudStatus };
        public static CrudResponse<int> ToCrudResponse(this Method entity, Constants.CrudStatus crudStatus)
            => new CrudResponse<int> { Id = entity.MethodId, CrudStatus = crudStatus };
        public static CrudResponse<int> ToCrudResponse(this Recipient entity, Constants.CrudStatus crudStatus)
            => new CrudResponse<int> { Id = entity.RecipientId, CrudStatus = crudStatus };
        public static CrudResponse<long> ToCrudResponse(this Transaction entity, Constants.CrudStatus crudStatus)
            => new CrudResponse<long> { Id = entity.TransactionId, CrudStatus = crudStatus };
        public static TransactionResponse ToResponse(this Transaction transaction)
        {
            TransactionResponse response = new TransactionResponse
            {
                Id = transaction.TransactionId,
                TransactionDate = transaction.TransactionDate,
                PostedDate = transaction.PostedDate,
            };

            if (transaction.Initiator != null)
            {
                response.Initiator = transaction.Initiator.ToCommonResponse();
            }

            if (transaction.Recipient != null)
            {
                response.Recipient = transaction.Recipient.ToCommonResponse();
            }

            if (transaction.TransactionMethods.NullSafeAny())
            {
                response.Methods = transaction.TransactionMethods.Select(e => e.ToResponse());
            }

            if (transaction.TransactionDetails.NullSafeAny())
            {
                response.Details = transaction.TransactionDetails.Select(e => e.ToResponse());
            }

            return response;
        }
        public static TransactionDetailResponse ToResponse(this TransactionDetail detail)
        {
            var response = new TransactionDetailResponse
            {
                Id = detail.DetailId,
                Description = detail.Detail.Description,
                Category = detail.Detail.Category.ToResponse(),
                Multiplier = detail.Multiplier,
                TransactionId = detail.TransactionId
            };

            return response;
        }
        public static DetailResponse ToResponse(this Detail detail, bool includeCategory = false, bool includeTransaction = false)
        {
            var response = new DetailResponse
            {
                Id = detail.DetailId,
                Description = detail.Description,
            };

            if (detail.Category != null && includeCategory)
            {
                response.Category = detail.Category.ToResponse();
            }

            if (detail.TransactionDetails.NullSafeAny() && includeTransaction)
            {
                response.Transactions = detail.TransactionDetails.Select(ToResponse);
            }

            return response;
        }
        public static CategoryResponse ToResponse(this Category category, bool includeDetails = false)
        {
            var response = new CategoryResponse
            {
                Id = category.CategoryId,
                Name = category.Name,
            };

            if (category.Details.NullSafeAny() && includeDetails)
            {
                response.Details = category.Details.Select(e => e.ToResponse());
            }

            return response;
        }
        public static CommonNamedEntityResponse ToCommonResponse(this Category category, bool includeTransaction = false)
        {
            var response = new CommonNamedEntityResponse
            {
                Id = category.CategoryId,
                Name = category.Name,
            };

            if (includeTransaction && category.Details.NullSafeAny())
            {
                response.Transactions = category.Details.Where(d => d.TransactionDetails.NullSafeAny()).SelectMany(e => e.TransactionDetails.Select(x => x.Transaction.ToResponse()));
            }

            return response;
        }
        public static CommonNamedEntityResponse ToCommonResponse(this Initiator initiator, bool includeTransaction = false)
        {
            var response = new CommonNamedEntityResponse
            {
                Id = initiator.InitiatorId,
                Name = initiator.Name,
            };

            if (includeTransaction && initiator.Transactions.NullSafeAny())
            {
                response.Transactions = initiator.Transactions.Select(ToResponse);
            }

            return response;
        }
        public static TransactionMethodResponse ToResponse(this TransactionMethod method)
        {
            var response = new TransactionMethodResponse
            {
                Id = method.MethodId,
                Name = method.Method.Name,
                Amount = method.Amount,
                TransactionId = method.TransactionId
            };

            return response;
        }
        public static MethodResponse ToResponse(this Method method)
        {
            var response = new MethodResponse
            {
                Id = method.Id,
                Name = method.Name,
            };

            if (method.TransactionMethods.NullSafeAny())
            {
                response.Transactions = method.TransactionMethods.Select(ToResponse);
            }

            return response;
        }
        public static CommonNamedEntityResponse ToCommonResponse(this Method method, bool includeTransaction = false)
        {
            var response = new CommonNamedEntityResponse
            {
                Id = method.Id,
                Name = method.Name,
            };

            if (includeTransaction && method.TransactionMethods.NullSafeAny())
            {
                response.Transactions = method.TransactionMethods.Select(x => x.Transaction.ToResponse());
            }

            return response;
        }
        public static CommonNamedEntityResponse ToCommonResponse(this Recipient recipient, bool includeTransaction = false)
        {
            var response = new CommonNamedEntityResponse
            {
                Id = recipient.RecipientId,
                Name = recipient.Name,
            };

            if (includeTransaction && recipient.Transactions.NullSafeAny())
            {
                response.Transactions = recipient.Transactions.Select(ToResponse);
            }

            return response;
        }
    }
}
