using MediatR;
using Tasky.Core.Domain.Entities;
using Tasky.Core.Infrastructure;
using Task = System.Threading.Tasks.Task;

namespace Tasky.Core.Application.Handlers;

public class ListBoardsWithTasksHandler : IRequestHandler<Requests.ListBoardsWithTasks, IEnumerable<Board>>
{
    private readonly IContext _context;

    public ListBoardsWithTasksHandler(IContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Board>> Handle(Requests.ListBoardsWithTasks request,
        CancellationToken cancellationToken)
    {
        var boards = await _context.ReadAsync();
        return await Task.FromResult(boards);
    }
}