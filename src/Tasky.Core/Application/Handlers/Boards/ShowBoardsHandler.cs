using JetBrains.Annotations;
using MediatR;
using Tasky.Core.Domain.Entities;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Application.Handlers.Boards;

[UsedImplicitly]
public class ShowBoardsHandler : IRequestHandler<Requests.ListBoardsWithTasks, IEnumerable<Board>>
{
    private readonly IBoardRepository _repository;

    public ShowBoardsHandler(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Board>> Handle(Requests.ListBoardsWithTasks request,
        CancellationToken cancellationToken)
    {
        var boards = await _repository.AllAsync();
        return boards;
    }
}