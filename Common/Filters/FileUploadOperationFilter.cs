using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Filters;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        var fileParams = context.ApiDescription.ActionDescriptor.Parameters
            .Where(p => p.ParameterType == typeof(IFormFile))
            .ToList();

        if (!fileParams.Any())
            return;

        foreach (var param in fileParams)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = param.Name,
                In = ParameterLocation.Query,
                Description = "Upload File",
                Required = true,
                Schema = new OpenApiSchema { Type = "file" }
            });
        }
    }
}