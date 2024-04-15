using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace authSwagger
{
    public class ApiKeyAuthFilter : Attribute, IAuthorizationFilter
    {
        
        private readonly IConfiguration _configuration;

        public ApiKeyAuthFilter(IConfiguration configuration) {
            _configuration = configuration;
        }

        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstant.ApiKeyHeaderName, out var extractedAiKey))
            {
                context.Result = new UnauthorizedObjectResult("Api Key Missing");
                return;
            }
          //  var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apikey = _configuration.GetValue<string>(AuthConstant.ApiKeySectionNme);

            if (!apikey.Equals(extractedAiKey))
            {
                context.Result = new UnauthorizedObjectResult("Api Key Wrong");
                return;
            }
        }
    }
}
