namespace DotNetIdentityAPI.CustomMiddleware
{
    public class CustomizeAuthorizationFailureMessageMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomizeAuthorizationFailureMessageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(new
                {
                    error = "Fobidden Access",
                    message = "You are not authorized to access this resource"
                }.ToString());
            }
        }
    }
}
