using Esoteric.Finance.Abstractions.Constants;

namespace Esoteric.Finance.Abstractions.DataTransfer
{
    public class CrudResponse<TKey> : CrudResponse
    {
        public virtual TKey Id { get; set; }
    }
    public class CrudResponse
    {
        public virtual CrudStatus CrudStatus { get; set; }
    }
}
