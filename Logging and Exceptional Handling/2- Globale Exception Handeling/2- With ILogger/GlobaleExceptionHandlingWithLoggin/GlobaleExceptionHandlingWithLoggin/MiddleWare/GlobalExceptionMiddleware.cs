namespace GlobaleExceptionHandlingWithLoggin
{
    public class GlobalExceptionMiddleware
    {
        // RequestDelegate ko inject kar rahe hain
        private readonly RequestDelegate _next;

        //logger ko inject kar rahe hain
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        // ILogger inject kiya for logging
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Aage next middleware/action chalayenge
                await _next(context);
            }
            catch (Exception ex)
            {
                // 🔥 Logging: Error ko log kar rahe hain with stack trace
                _logger.LogError(ex, "Unhandled exception occurredd.");

                // User ke liye simple response
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    StatusCode = 500,
                    Message = "Something went wrong. Please try again later."
                };

                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }

}
