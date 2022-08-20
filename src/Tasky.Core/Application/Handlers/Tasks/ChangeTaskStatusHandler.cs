using JetBrains.Annotations;
using MediatR;
using Notie.Contracts;
using Notie.Models;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Application.Handlers.Tasks;


[UsedImplicitly]
public class ChangeTaskStatusHandler : IRequestHandler<Requests.ChangeTaskStatus>
{
    private readonly IBoardRepository _repository;
    private readonly INotifier _notifier;

    public ChangeTaskStatusHandler(IBoardRepository repository, INotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(Requests.ChangeTaskStatus request, CancellationToken cancellationToken)
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

        task.ChangeStatus(request.Status);

        await _repository.UpdateAsync(board!);
        return Unit.Value;
    }
}