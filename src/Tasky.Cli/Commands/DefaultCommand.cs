using MediatR;
using Tasky.Cli.Commands.Boards;
using Tasky.Cli.UserInterface;

namespace Tasky.Cli.Commands;

public sealed class DefaultCommand : ShowBoardsCommand
{
    public DefaultCommand(IMediator mediator, IConsoleWriter writer) : base(mediator, writer)
    {
    }
}