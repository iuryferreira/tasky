using MediatR;
using Tasky.Core.Domain;
using Tasky.Core.Domain.Entities;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Application.Handlers;

public class AddStepOnTaskHandler : IRequestHandler<Requests.AddStepOnTask, IEnumerable<Board>>
{
    private readonly IBoardRepository _repository;

    public AddStepOnTaskHandler(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Board>> Handle(Requests.AddStepOnTask request, CancellationToken cancellationToken)
    {
        var board = await _repository.GetByNameAsync(request.Data.BoardName);
        if (board is null) return await _repository.AllAsync();

        var task = board.Tasks.FirstOrDefault(t => t.Id.Equals(request.Data.TaskId));
        if (task is null) return await _repository.AllAsync();

        var step = new Step($"{task.Id}.{task.Steps.Count + 1}", request.Data.Text, Status.Todo);
        task.AddStep(step);

        var index = board.Tasks.IndexOf(task);
        board.Tasks[index] = task;

        await _repository.UpdateAsync(board);
        return await _repository.AllAsync();
    }
}