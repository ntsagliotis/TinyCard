namespace TinyCard.Core.Results
{
    public class Result<T>
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public T Data { get; set; }
        public int AppEventId { get; set; }
    }
}
