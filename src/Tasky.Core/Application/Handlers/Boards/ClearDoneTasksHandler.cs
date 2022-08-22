using JetBrains.Annotations;
using MediatR;
using Tasky.Core.Infrastructure.Repositories;
using Tasky.Shared;

namespace Tasky.Core.Application.Handlers.Boards;

[UsedImplicitly]
public class ClearDoneTasksHandler : IRequestHandler<Requests.ClearDoneTasks>
{
    private readonly IBoardRepository _repository;

    public ClearDoneTasksHandler(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(Requests.ClearDoneTasks request, CancellationToken cancellationToken)
    {
        var boards = await _repository.AllAsync();
        foreach (var board in boards)
        {
            board.Tasks.RemoveAll(task => task.Status.Equals(Status.Done));
            board.SortTasks();
        }

        await _repository.SaveBoardsAsync(boards);
        return Unit.Value;
    }
}