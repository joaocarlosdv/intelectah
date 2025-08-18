namespace Application.Dtos
{
    public class OperationResponseDto<Dto>
    {
        public bool Success { get; set; }
        public Dto? Object { get; set; }
        public string? Message { get; set; }
        public Exception? Exception { get; set; }
    }
}
