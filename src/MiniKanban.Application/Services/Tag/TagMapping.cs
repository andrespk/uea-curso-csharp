using Mapster;
using MiniKanban.Application.DTOs;

namespace MiniKanban.Application.Services.Tag;

internal static class TagMapping
{
    public static TagResponseDto ToResponse(Domain.Entities.Tag tag)
    {
        return tag.Adapt<TagResponseDto>();
    }

    public static Domain.Entities.Tag ToEntity(CreateTagDto dto)
    {
        return dto.Adapt<Domain.Entities.Tag>();
    }

    public static Domain.Entities.Tag ToEntity(UpdateTagDto dto, Domain.Entities.Tag entity)
    {
        return dto.Adapt(entity);
    }
}