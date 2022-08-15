using System.ComponentModel;
using MediatR;
using Spectre.Console.Cli;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Core.Domain;
using Tasky.Shared;

namespace Tasky.Cli.Commands.Tasks;

public sealed class DoneTaskCommand : BaseCommand<DoneTaskCommand.Settings>
{
    public DoneTaskCommand(IMediator mediator, IConsoleWriter writer) : base(mediator, writer)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var data = new Dtos.ChangeTaskStatusRequestDto(settings.Id.ToString(), settings.BoardName);
        var request = new Requests.ChangeTaskStatus(data, Status.Done);
        await Mediator.Send(request);
        Writer.ShowBoards(await Mediator.Send(new Requests.ListBoardsWithTasks()));
        return 0;
    }

    public static void Configure(IConfigurator configurator)
    {
        configurator.AddCommand<DoneTaskCommand>(Settings.CommandName)
            .WithAlias(Settings.CommandAlias)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    public sealed class Settings : CommandSettings
    {
        public const string CommandName = "donetask";
        public const string CommandAlias = "dt";
        public const string CommandDescription = "Marks a task already created as complete informing the id and board";
        public static readonly string[] CommandExample = {"done", "1", "-b", "shopping"};


        [Description("task id")]
        [CommandArgument(0, "<TASK_ID>")]
        public int Id { get; init; } = 0;

        [Description("board to which the task belongs")]
        [CommandOption("-b|--board")]
        [DefaultValue("board")]
        public string BoardName { get; init; } = "board";
    }
}