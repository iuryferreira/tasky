using JetBrains.Annotations;
using MediatR;
using Notie.Contracts;
using Notie.Models;
using Tasky.Core.Domain;
using Tasky.Core.Domain.Entities;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Application.Handlers.Steps;

[UsedImplicitly]
public class AddStepOnTaskHandler : IRequestHandler<Requests.AddStepOnTask>
{
    private readonly INotifier _notifier;
    private readonly IBoardRepository _repository;

    public AddStepOnTaskHandler(IBoardRepository repository, INotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(Requests.AddStepOnTask request, CancellationToken cancellationToken)
    {
        if (!request.Data.Valid)
        {
            _notifier.AddNotificationsByFluent(request.Data.ValidationResult);
            return Unit.Value;
        }

        var board = await _repository.GetByNameAsync(request.Data.BoardName);

        var task = board?.Tasks.Find(t => t.Id.Equals(request.Data.TaskId));

        if (task is null)
        {
            var notification = new Notification("TaskId", "The given task id was not found.");
            _notifier.AddNotification(notification);
            return Unit.Value;
        }

        var step = new Step($"{task.Id}.{task.Steps.Count + 1}", request.Data.Text, Status.Todo);
        task.AddStep(step);

        var index = board!.Tasks.IndexOf(task);
        board.Tasks[index] = task;

        await _repository.UpdateAsync(board);
        return Unit.Value;
    }
}