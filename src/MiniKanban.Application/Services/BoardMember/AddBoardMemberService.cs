using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Enums;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class AddBoardMemberService : IAddBoardMemberService, ScopedInjection
{
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddBoardMemberService(
        IBoardMemberRepository boardMemberRepository,
        IBoardRepository boardRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _boardMemberRepository = boardMemberRepository;
        _boardRepository = boardRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BoardMemberResponseDto> AddAsync(CreateBoardMemberDto request)
    {
        if (request.Role == BoardRole.Owner)
            throw new BusinessException("Use board creation to define the owner.");

        if (await _boardRepository.GetByIdAsync(request.BoardId) == null)
            throw new BusinessException("Board not found.");

        if (await _userRepository.GetByIdAsync(request.UserId) == null)
            throw new BusinessException("User not found.");

        if (await _boardMemberRepository.ExistsAsync(request.BoardId, request.UserId))
            throw new BusinessException("User is already a board member.");

        var member = new BoardMember
        {
            BoardId = request.BoardId,
            UserId = request.UserId,
            Role = request.Role
        };

        await _boardMemberRepository.AddAsync(member);
        await _unitOfWork.CommitAsync();

        return BoardMemberMapping.ToResponse(member);
    }
}
