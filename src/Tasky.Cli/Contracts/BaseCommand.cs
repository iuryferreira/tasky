using System.Globalization;
using MediatR;
using Spectre.Console.Cli;

namespace Tasky.Cli.Contracts;

public abstract class BaseCommand<T> : AsyncCommand<T> where T : CommandSettings
{
    protected readonly IMediator Mediator;

    protected BaseCommand(IMediator mediator)
    {
        Mediator = mediator;
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