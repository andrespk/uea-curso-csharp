using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces;
using MiniKanban.Domain.Entities;
using MiniKanban.Domain.Enums;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Exceptions.Users;

namespace MiniKanban.Application.Services;

public class CreateBoardService : ICreateBoardService, ScopedInjection
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBoardService(
        IBoardRepository boardRepository,
        IBoardMemberRepository boardMemberRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _boardRepository = boardRepository;
        _boardMemberRepository = boardMemberRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BoardResponseDto> CreateAsync(CreateBoardDto request)
    {
        if (await _userRepository.GetByIdAsync(request.OwnerId) == null)
            throw new BusinessException("Owner user not found.");

        var board = new Board
        {
            Name = request.Name,
            Description = request.Description,
            OwnerId = request.OwnerId
        };

        var ownerMember = new BoardMember
        {
            BoardId = board.Id,
            UserId = request.OwnerId,
            Role = BoardRole.Owner
        };

        await _boardRepository.AddAsync(board);
        await _boardMemberRepository.AddAsync(ownerMember);
        await _unitOfWork.CommitAsync();

        return BoardMapping.ToResponse(board);
    }
}
