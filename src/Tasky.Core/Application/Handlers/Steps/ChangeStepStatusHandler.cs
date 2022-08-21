using JetBrains.Annotations;
using MediatR;
using Notie.Contracts;
using Notie.Models;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Application.Handlers.Steps;

[UsedImplicitly]
public class ChangeStepStatusHandler : IRequestHandler<Requests.ChangeStepStatus>
{
    private readonly INotifier _notifier;
    private readonly IBoardRepository _repository;

    public ChangeStepStatusHandler(IBoardRepository repository, INotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(Requests.ChangeStepStatus request, CancellationToken cancellationToken)
    {
        if (!request.Data.Valid)
        {
            _notifier.AddNotificationsByFluent(request.Data.ValidationResult);
            return Unit.Value;
        }

        var board = await _repository.GetByNameAsync(request.Data.BoardName);

        var task = board?.Tasks.Find(t => t.Id.Equals(request.Data.TaskId));

        var step = task?.Steps.Find(t => t.Id.Equals(request.Data.StepId));
        if (step is null)
        {
            var notification = new Notification("StepId", "The given step id was not found.");
            _notifier.AddNotification(notification);
            return Unit.Value;
        }

        step.ChangeStatus(task!, request.Status);

        await _repository.UpdateAsync(board!);
        return Unit.Value;
    }
}