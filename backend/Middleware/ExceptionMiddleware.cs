using Backend.DTOs;

namespace Backend.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                var response = new ErrorResponseDto
                {
                    Message = ex.Message,
                    StatusCode = ex.StatusCode,
                    Errors = ex.Errors
                };

                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception)
            {
                var response = new ErrorResponseDto
                {
                    Message = "Internal server error",
                    StatusCode = StatusCodes.Status500InternalServerError,
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
