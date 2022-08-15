using MediatR;
using Tasky.Core.Infrastructure.Repositories;

namespace Tasky.Core.Application.Handlers.Steps;

public class ChangeStepStatusHandler : IRequestHandler<Requests.ChangeStepStatus>
{
    private readonly IBoardRepository _repository;

    public ChangeStepStatusHandler(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(Requests.ChangeStepStatus request, CancellationToken cancellationToken)
    {
        var board = await _repository.GetByNameAsync(request.Data.BoardName);

        var task = board?.Tasks.FirstOrDefault(t => t.Id.Equals(request.Data.TaskId));

        var step = task?.Steps.FirstOrDefault(t => t.Id.Equals(request.Data.StepId));
        if (step is null) return Unit.Value;

        step.ChangeStatus(request.Status);

        await _repository.UpdateAsync(board!);
        return Unit.Value;
    }
}