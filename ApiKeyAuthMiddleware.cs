namespace authSwagger
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration conf)
        {
            _next = next;
            _configuration = conf;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.Request.Headers.TryGetValue(AuthConstant.ApiKeyHeaderName, out var extractedAiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("key missing ");
                return;
            }
            var apikey = _configuration.GetValue<string>(AuthConstant.ApiKeySectionNme);
            if(!apikey.Equals(extractedAiKey))
            {
                await context.Response.WriteAsync("key missing ");
                return;
            }
            await _next(context);
        }
    }
}
