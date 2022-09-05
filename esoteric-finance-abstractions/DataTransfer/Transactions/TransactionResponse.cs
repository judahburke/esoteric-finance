using Esoteric.Finance.Abstractions.DataTransfer.Recipients;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionResponse
        : TransactionModel<TransactionRecipientResponse, TransactionMethodResponse, TransactionCategoryResponse>
    {
        public long TransactionId { get; set; }
    }
}
