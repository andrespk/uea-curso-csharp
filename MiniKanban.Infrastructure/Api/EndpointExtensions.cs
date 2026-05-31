using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using MiniKanban.Infrastructure.Api.Interfaces;

namespace MiniKanban.Infrastructure.Api;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var endpointTypes = entryAssembly.GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in endpointTypes)
        {
            var endpointInstance = (IEndpoint)Activator.CreateInstance(type)!;
            endpointInstance.MapEndpoint(app);
        }

        return app;
    }
}
