using MediatR;
using Tasky.Cli.UserInterface;

namespace Tasky.Cli.Commands;

public sealed class DefaultCommand : ListTasksCommand
{
    public DefaultCommand(IMediator mediator, IConsoleWriter writer) : base(mediator, writer)
    {
    }
}