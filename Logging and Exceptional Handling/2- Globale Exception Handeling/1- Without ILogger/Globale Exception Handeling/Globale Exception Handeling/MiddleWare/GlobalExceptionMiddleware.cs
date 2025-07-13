namespace Globale_Exception_Handeling.MiddleWare
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        // Constructor me next middleware ko inject kiya ja raha hai
        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Ye method har request ke liye run hota hai
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Agar koi exception nahi aata to request normally aage jayegi
                await _next(context);
            }
            catch (Exception ex)
            {
                // Agar koi bhi exception aata hai to ye block chalega

                // HTTP response code 500 set kar rahe hain (Internal Server Error)
                context.Response.StatusCode = 500;

                // Response ka content type JSON set kar rahe hain
                context.Response.ContentType = "application/json";

                // JSON me ek simple error message prepare kar rahe hain
                var result = new
                {
                    StatusCode = 500,
                    Message = "Something went wrong. Please try again later."
                };

                // User ko JSON response bhej rahe hain
                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }

}
