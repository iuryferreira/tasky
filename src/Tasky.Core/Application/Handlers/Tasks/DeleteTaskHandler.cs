using JetBrains.Annotations;
using MediatR;
using Notie.Contracts;
using Notie.Models;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Application.Handlers.Tasks;

[UsedImplicitly]
public class DeleteTaskHandler : IRequestHandler<Requests.DeleteTask>
{
    private readonly INotifier _notifier;
    private readonly IBoardRepository _repository;

    public DeleteTaskHandler(IBoardRepository repository, INotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(Requests.DeleteTask request, CancellationToken cancellationToken)
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

        board?.Tasks.Remove(task);
        board?.SortTasks();
        await _repository.UpdateAsync(board!);
        return Unit.Value;
    }
}