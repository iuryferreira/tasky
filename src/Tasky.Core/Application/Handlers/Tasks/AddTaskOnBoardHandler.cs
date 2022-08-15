using MediatR;
using Tasky.Core.Domain.Entities;
using Tasky.Core.Infrastructure.Repositories;
using Task = Tasky.Core.Domain.Entities.Task;

namespace Tasky.Core.Application.Handlers.Tasks;

public class AddTaskOnBoardHandler : IRequestHandler<Requests.AddTaskOnBoard>
{
    private readonly IBoardRepository _repository;

    public AddTaskOnBoardHandler(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(Requests.AddTaskOnBoard request, CancellationToken cancellationToken)
    {
        Task task;
        var board = await _repository.GetByNameAsync(request.Data.BoardName);
        if (board is null)
        {
            task = Task.CreateTaskWithText("1", request.Data.Text);
            board = new Board(request.Data.BoardName, new List<Task> {task});
            await _repository.AddAsync(board);
            return Unit.Value;
        }

        task = Task.CreateTaskWithText($"{board.Quantity + 1}", request.Data.Text);
        board.AddTask(task);
        await _repository.UpdateAsync(board);
        return Unit.Value;
    }
}