using MediatR;
using Notie.Contracts;
using Tasky.Cli.UserInterface;

namespace Tasky.Cli.Commands;

public sealed class DefaultCommand : ListCommand
{
    public DefaultCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier) : base(mediator, writer,
        notifier)
    {
    }
}