public class BusinessException : Exception
{
    public int StatusCode { get; }
    public List<string>? Errors { get; }

    public BusinessException(
        string message,
        int statusCode = StatusCodes.Status400BadRequest,
        List<string>? errors = null
    ) : base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
    }
}
