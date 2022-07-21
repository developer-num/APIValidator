using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Validator.Cliente.Models
{
    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters.Count > 0)
            {
                var versionParameter = operation.Parameters.Single(v => v.Name.Equals("version"));
                operation.Parameters.Remove(versionParameter);
            }
        }
    }

    public class ReplaceVersionWithExactValuePath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths;
            swaggerDoc.Paths = new OpenApiPaths();

            foreach (var path in paths)
            {
                var key = path.Key.Replace("v{version}", swaggerDoc.Info.Version);
                var value = path.Value;
                swaggerDoc.Paths.Add(key, value);
            }
        } 
    }
}
