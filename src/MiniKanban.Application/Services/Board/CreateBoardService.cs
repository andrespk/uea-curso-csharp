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

    public async Task<BoardResponseDto> CreateAsync(CreateBoardDto request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _userRepository.GetByIdAsync(request.OwnerId, cancellationToken) == null)
            throw new BusinessException("Owner user not found.");

        cancellationToken.ThrowIfCancellationRequested();

        var board = BoardMapping.ToEntity(request);

        var ownerMember = new BoardMember
        {
            BoardId = board.Id,
            UserId = request.OwnerId,
            Role = BoardRole.Owner
        };

        await _boardRepository.AddAsync(board, cancellationToken);
        await _boardMemberRepository.AddAsync(ownerMember, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return BoardMapping.ToResponse(board);
    }
}
