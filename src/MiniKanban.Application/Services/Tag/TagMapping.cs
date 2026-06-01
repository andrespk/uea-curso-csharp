using Mapster;
using MiniKanban.Application.DTOs;
using MiniKanban.Domain.Entities;

namespace MiniKanban.Application.Services;

internal static class TagMapping
{
    public static TagResponseDto ToResponse(Tag tag)
    {
        return tag.Adapt<TagResponseDto>();
    }

    public static Tag ToEntity(CreateTagDto dto)
    {
        return dto.Adapt<Tag>();
    }

    public static Tag ToEntity(UpdateTagDto dto, Tag entity)
    {
        return dto.Adapt(entity);
    }
}

