using Esoteric.Finance.Abstractions;
using Esoteric.Finance.Abstractions.DataTransfer;
using Esoteric.Finance.Abstractions.DataTransfer.Categories;
using Esoteric.Finance.Abstractions.DataTransfer.Methods;
using Esoteric.Finance.Abstractions.DataTransfer.Recipients;
using Esoteric.Finance.Abstractions.DataTransfer.Transactions;
using Esoteric.Finance.Abstractions.Entities.Payment;
using System.Linq;

namespace Esoteric.Finance.Abstractions.DataTransfer
{
    public static class Extensions
    {
        public static CrudResponse<TKey> ToCreateResponse<TKey>(this TKey id) 
            => new CrudResponse<TKey> { Id = id, CrudStatus = Constants.CrudStatus.CREATED };
        public static CrudResponse<TKey> ToUpdateResponse<TKey>(this TKey id)
            => new CrudResponse<TKey> { Id = id, CrudStatus = Constants.CrudStatus.UPDATED };
        public static TransactionResponse ToTransactionResponse(this Transaction transaction)
        {
            TransactionResponse response = new TransactionResponse
            {
                TransactionId = transaction.TransactionId,
                PostedDate = transaction.PostedDate,
                TransactionDate = transaction.TransactionDate,
            };

            if (transaction.Recipient != null)
            {
                response.Recipient = transaction.Recipient.ToTransactionRecipientResponse();
            }

            if (transaction.Methods != null)
            {
                response.Methods = transaction.Methods.Select(ToTransactionMethodResponse);
            }

            if (transaction.SubCategories != null)
            {
                response.Categories = transaction.SubCategories.Select(ToTransactionCategoryResponse);
            }

            return response;
        }
        public static TransactionRecipientResponse ToTransactionRecipientResponse(this Recipient recipient, bool includeTransactionIds = false)
        {
            var response = new TransactionRecipientResponse
            {
                RecipientId = recipient.RecipientId,
                Recipient = recipient.Name,
            };

            if (recipient.Transactions.NullSafeAny() && includeTransactionIds)
            {
                response.TransactionIds = recipient.Transactions.Select(x => x.TransactionId).ToList();
            }

            return response;
        }
        public static TransactionMethodResponse ToTransactionMethodResponse(this TransactionMethod transactionMethod)
        {
            var response = new TransactionMethodResponse
            {
                Amount = transactionMethod.Amount,
                MethodId = transactionMethod.MethodId,
                TransactionId = transactionMethod.TransactionId,
            };

            if (transactionMethod.Method != null)
            {
                response.Method = transactionMethod.Method.Name;
                response.MethodId = transactionMethod.Method.MethodId;
            }

            if (transactionMethod.Transaction != null)
            {
                response.TransactionId = transactionMethod.Transaction.TransactionId;
            }

            return response;
        }
        public static TransactionCategoryResponse ToTransactionCategoryResponse(this TransactionSubCategory transactionSubCategory)
        {
            var response = new TransactionCategoryResponse
            {
                Multiplier = transactionSubCategory.Multiplier,
                SubCategoryId = transactionSubCategory.SubCategoryId,
                TransactionId = transactionSubCategory.TransactionId,
            };

            if (transactionSubCategory.SubCategory != null)
            {
                response.SubCategory = transactionSubCategory.SubCategory.Name;
                response.SubCategoryId = transactionSubCategory.SubCategory.SubCategoryId;
                response.CategoryId = transactionSubCategory.SubCategory.CategoryId;
                if (transactionSubCategory.SubCategory.Category != null)
                {
                    response.Category = transactionSubCategory.SubCategory.Category.Name;
                    response.CategoryId = transactionSubCategory.SubCategory.Category.CategoryId;
                }
            }

            if (transactionSubCategory.Transaction != null)
            {
                response.TransactionId = transactionSubCategory.Transaction.TransactionId;
            }

            return response;
        }
        public static RecipientResponse ToRecipientResponse(this Recipient recipient, bool includeFullTransaction = false)
        {
            var response = new RecipientResponse
            {
                Id = recipient.RecipientId,
                Name = recipient.Name,
            };

            if (recipient.Transactions.NullSafeAny() && includeFullTransaction)
            {
                response.Transactions = recipient.Transactions.Select(ToTransactionResponse);
            }

            return response;
        }
        public static MethodResponse ToMethodResponse(this Method method, bool includeFullTransaction = false)
        {
            var response = new MethodResponse
            {
                Id = method.MethodId,
                Name = method.Name,
            };

            if (method.Transactions.NullSafeAny())
            {
                if (includeFullTransaction)
                {
                    response.Transactions = method.Transactions.Select(t => t.Transaction.ToTransactionResponse());
                }
                else
                {
                    response.TransactionMethods = method.Transactions.Select(ToTransactionMethodResponse);
                }
            }

            return response;
        }
        public static CategoryResponse ToCategoryResponse(this Category category, bool includeSubCategories = false)
        {
            var response = new CategoryResponse
            {
                Id = category.CategoryId,
                Name = category.Name,
            };

            if (category.SubCategories.NullSafeAny() && includeSubCategories)
            {
                response.SubCategories = category.SubCategories.Select(sc => sc.ToSubCategoryResponse(includeCategory: false)).ToList();
            }

            return response;
        }
        public static SubCategoryResponse ToSubCategoryResponse(this SubCategory subCategory, bool includeCategory = false, bool includeFullTransaction = true)
        {
            var response = new SubCategoryResponse
            {
                Id = subCategory.SubCategoryId,
                Name = subCategory.Name,
            };

            if (subCategory.Transactions.NullSafeAny())
            {
                if (includeFullTransaction)
                {
                    response.Transactions = subCategory.Transactions.Select(t => t.Transaction.ToTransactionResponse());
                }
                else
                {
                    response.TransactionCategories = subCategory.Transactions.Select(ToTransactionCategoryResponse);
                }
            }

            if (subCategory.Category != null && includeCategory)
            {
                response.Category = subCategory.Category.ToCategoryResponse(includeSubCategories: false);
            }

            return response;
        }
    }
}
