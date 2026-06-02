using Microsoft.AspNetCore.Routing;

namespace MiniKanban.Infrastructure.Api.Interfaces;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}