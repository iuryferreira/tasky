using System.Globalization;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.UserInterface;

namespace Tasky.Cli.Contracts;

public abstract class BaseCommand<T> : AsyncCommand<T> where T : CommandSettings
{
    protected readonly IMediator Mediator;
    protected readonly IConsoleWriter Writer;
    protected readonly INotifier Notifier;

    protected BaseCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier)
    {
        Mediator = mediator;
        Writer = writer;
        Notifier = notifier;
    }
    
    protected async Task<int> Handle(Func<Task<int>> action)
    {
        if (!Notifier.HasNotifications()) return await action.Invoke();
        Writer.ShowErrors(Notifier.All());
        return 0;
    }

    protected DateTime? ParseDate(string? dateInString)
    {
        DateTime? date = null;
        if (!string.IsNullOrEmpty(dateInString))
        {
            date = DateTime.Parse(dateInString, styles: DateTimeStyles.AssumeLocal);
        }

        return date;
    }
}