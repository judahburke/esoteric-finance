namespace Esoteric.Finance.Abstractions.DataTransfer.Details
{
    public class DetailDeleteRequest : DetailModel
    {
        public long? Id { get; set; }
        public string ReplacementDescription { get; set; }
    }
}
