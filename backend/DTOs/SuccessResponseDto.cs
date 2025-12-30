namespace Backend.DTOs
{
    public class SuccessResponseDto<T>
    {
        public string Message { get; set; } = "Success";
        public int StatusCode { get; set; }
        public T? Data { get; set; }
    }
}