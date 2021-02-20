using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tasker.Api.Swagger
{
    public class SetVersionInPaths : IDocumentFilter
    {
        public void Apply(
            SwaggerDocument swaggerDoc,
            DocumentFilterContext context)
        {
            swaggerDoc.Paths = swaggerDoc.Paths
                .ToDictionary(
                    path => path.Key.Replace("v{v}", swaggerDoc.Info.Version),
                    path => path.Value
                );
        }
    }
}