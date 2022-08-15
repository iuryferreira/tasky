using MediatR;
using Tasky.Core.Domain;
using Tasky.Core.Domain.Entities;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Application.Handlers.Steps;

public class AddStepOnTaskHandler : IRequestHandler<Requests.AddStepOnTask>
{
    private readonly IBoardRepository _repository;

    public AddStepOnTaskHandler(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(Requests.AddStepOnTask request, CancellationToken cancellationToken)
    {
        var board = await _repository.GetByNameAsync(request.Data.BoardName);

        var task = board?.Tasks.FirstOrDefault(t => t.Id.Equals(request.Data.TaskId));
        if (task is null) return Unit.Value;

        var step = new Step($"{task.Id}.{task.Steps.Count + 1}", request.Data.Text, Status.Todo);
        task.AddStep(step);

        var index = board!.Tasks.IndexOf(task);
        board.Tasks[index] = task;

        await _repository.UpdateAsync(board);
        return Unit.Value;
    }
}