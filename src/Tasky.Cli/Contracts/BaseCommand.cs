using System.Globalization;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.UserInterface;

namespace Tasky.Cli.Contracts;

public abstract class BaseCommand<T> : AsyncCommand<T> where T : CommandSettings
{
    private readonly INotifier _notifier;

    protected readonly IMediator Mediator;
    protected readonly IConsoleWriter Writer;

    protected BaseCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier)
    {
        _notifier = notifier;
        Mediator = mediator;
        Writer = writer;
    }
    
    protected async Task<int> Handle(Func<Task<int>> action)
    {
        if (!_notifier.HasNotifications()) return await action.Invoke();
        Writer.ShowErrors(_notifier.All());
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