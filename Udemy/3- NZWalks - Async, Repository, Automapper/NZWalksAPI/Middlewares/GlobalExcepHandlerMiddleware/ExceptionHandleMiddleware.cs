using System.Net;

namespace NZWalksAPI.Middlewares.GlobalExcepHandlerMiddleware
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandleMiddleware> _logger;
        public ExceptionHandleMiddleware(ILogger<ExceptionHandleMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception ex)
            {
                
                var errorId = Guid.NewGuid().ToString(); // Generate a unique error ID for tracking

                // Log the exception to console or file using Serilog
                //_logger.LogError(ex, "An unhandled exception occurred while processing the request.");
                _logger.LogError(ex, $"{errorId} - {ex.Message}");


                // retun a user-friendly error response or return a custom error message
                //httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";


                // Create a response object
                var response = new
                {
                    Id=errorId,
                    Message = "An unexpected error occurred. Please try again laterrr.",

                    //Details = ex.Message // You can include more details if needed, but be cautious about exposing sensitive information
                };
                // Write the response as JSON
                await httpContext.Response.WriteAsJsonAsync(response);
            }

        }
    }
}
//WriteAsJsonAsync converts the response object to JSON and writes it to the response body.