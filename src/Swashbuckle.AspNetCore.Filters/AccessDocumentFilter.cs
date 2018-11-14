using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swashbuckle.AspNetCore.Filters
{
    /// <summary>
    /// Hides all Swagger operations and descriptions if the current user is not authenticated.
    /// At the moment you need to use a browser plugin to pass a valid Authorization header,
    /// e.g. https://github.com/disptr/httpheadermangler
    /// This is because Swashbuckle.AspNetCore 3.0 and above use a version of swagger-ui
    /// which doesn't pass the Authorization token which was entered on the swagger-ui page.
    /// Older versions of swagger-ui used to do this. Once this swagger-ui PR has been
    /// completed: https://github.com/swagger-api/swagger-ui/pull/4334 and when Swashbuckle
    /// has been updated to use that version of swagger-ui, then you won't need to use
    /// a browser header extension
    /// </summary>
    public class AccessDocumentFilter : IDocumentFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccessDocumentFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                RemoveOperations(swaggerDoc);
                RemoveDescriptions(swaggerDoc);
            }
        }

        private static void RemoveOperations(SwaggerDocument swaggerDoc)
        {
            if (swaggerDoc?.Paths?.Values == null)
            {
                return;
            }

            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                pathItem.Delete = null;
                pathItem.Patch = null;
                pathItem.Post = null;
                pathItem.Put = null;
            }
        }

        private void RemoveDescriptions(SwaggerDocument swaggerDoc)
        {
            swaggerDoc?.Definitions?.Clear();
        }
    }
}
