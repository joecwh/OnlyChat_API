namespace API.Responses
{
    public class HttpResponse<T>
    {
        public bool IsSuccess { get; set; } = false;
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; } = default;
    }
}
