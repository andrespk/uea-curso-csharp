using MiniKanban.Application.DTOs;
using MiniKanban.Application.Interfaces.Board;
using MiniKanban.Domain.Enums;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Domain.Interfaces.DependencyInjection;
using MiniKanban.Domain.Interfaces.Repositories;
using MiniKanban.Exceptions;

namespace MiniKanban.Application.Services.Board;

public class CreateBoardService : ICreateBoardService, ScopedInjection
{
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

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

    public async Task<BoardResponseDto> CreateAsync(CreateBoardDto request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _userRepository.GetByIdAsync(request.OwnerId, cancellationToken) == null)
            throw new BusinessException("Owner user not found.");

        cancellationToken.ThrowIfCancellationRequested();

        var board = BoardMapping.ToEntity(request);

        var ownerMember = new Domain.Entities.BoardMember
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