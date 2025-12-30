namespace Backend.DTOs
{
    public class ErrorResponseDto
    {
        public string Message { get; set; } = "An error occurred";
        public int StatusCode { get; set; }
        public List<string>? Errors { get; set; }
    }
}