using MediatR;
using Tasky.Cli.UserInterface;

namespace Tasky.Cli.Commands.Output.Handlers;

public class ListTaskOutputHandler : IRequestHandler<Requests.ListTaskOutputRequest>
{
    private readonly IRender _render;

    public ListTaskOutputHandler(IRender render)
    {
        _render = render;
    }

    public Task<Unit> Handle(Requests.ListTaskOutputRequest request, CancellationToken cancellationToken)
    {
        var content = new Content();
        content.Set("Hello World").Bold();
        _render.Print(content.ToString());
        return Unit.Task;
    }
}