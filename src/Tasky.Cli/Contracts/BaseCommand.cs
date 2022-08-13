using System.Globalization;
using MediatR;
using Spectre.Console.Cli;
using Tasky.Cli.UserInterface;

namespace Tasky.Cli.Contracts;

public abstract class BaseCommand<T> : AsyncCommand<T> where T : CommandSettings
{
    protected readonly IMediator Mediator;
    protected readonly IConsoleWriter Writer;

    protected BaseCommand(IMediator mediator, IConsoleWriter writer)
    {
        Mediator = mediator;
        Writer = writer;
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