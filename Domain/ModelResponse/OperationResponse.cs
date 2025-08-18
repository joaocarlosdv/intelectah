namespace Domain.ModelResponse
{
    public class OperationResponse<T>
    {
        public bool Success { get; set; }
        public T? Object { get; set; }
        public string? Message { get; set; }
        public Exception? Exception { get; set; }
    }
}
