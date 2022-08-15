using MediatR;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Application.Handlers.Tasks;

public class ChangeTaskStatusHandler : IRequestHandler<Requests.ChangeTaskStatus>
{
    private readonly IBoardRepository _repository;

    public ChangeTaskStatusHandler(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(Requests.ChangeTaskStatus request, CancellationToken cancellationToken)
    {
        var board = await _repository.GetByNameAsync(request.Data.BoardName);

        var task = board?.Tasks.FirstOrDefault(t => t.Id.Equals(request.Data.TaskId));
        if (task is null) return Unit.Value;

        task.ChangeStatus(request.Status);

        await _repository.UpdateAsync(board!);
        return Unit.Value;
    }
}