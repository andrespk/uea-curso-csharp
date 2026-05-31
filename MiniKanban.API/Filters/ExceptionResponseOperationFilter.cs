using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiniKanban.API.Filters;

public class ExceptionResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!operation.Responses.ContainsKey("400"))
        {
            operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "Erro de negócio ou de validação (interceptado pelo GlobalExceptionHandler).",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository)
                    }
                }
            });
        }

        if (!operation.Responses.ContainsKey("500"))
        {
            operation.Responses.Add("500", new OpenApiResponse
            {
                Description = "Erro interno não tratado no servidor (interceptado pelo GlobalExceptionHandler).",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository)
                    }
                }
            });
        }
    }
}
