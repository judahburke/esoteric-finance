using Esoteric.Finance.Abstractions.DataTransfer.Details;
using Esoteric.Finance.Abstractions.DataTransfer.Methods;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionResponse
        : TransactionModel<CommonNamedEntityResponse, CommonNamedEntityResponse, TransactionMethodResponse, TransactionDetailResponse>
    {
        public long Id { get; set; }
    }
}
