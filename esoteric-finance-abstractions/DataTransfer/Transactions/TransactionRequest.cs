using Esoteric.Finance.Abstractions.DataTransfer.Details;
using Esoteric.Finance.Abstractions.DataTransfer.Methods;

namespace Esoteric.Finance.Abstractions.DataTransfer.Transactions
{
    public class TransactionRequest
        : TransactionModel<CommonNamedEntityRequest, CommonNamedEntityRequest, TransactionMethodRequest, TransactionDetailRequest>
    {
        public long? Id { get; set; }
    }
}
