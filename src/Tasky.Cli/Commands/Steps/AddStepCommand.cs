using System.ComponentModel;
using MediatR;
using Spectre.Console.Cli;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Shared;

namespace Tasky.Cli.Commands.Steps;

public class AddStepCommand : BaseCommand<AddStepCommand.Settings>
{
    public AddStepCommand(IMediator mediator, IConsoleWriter writer) : base(mediator, writer)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var request =
            new Requests.AddStepOnTask(new Dtos.AddStepOnTaskRequestDto(settings.TaskId, settings.BoardName,
                settings.Text));
        await Mediator.Send(request);
        Writer.ShowBoards(await Mediator.Send(new Requests.ListBoardsWithTasks()));
        return 0;
    }

    public static void Configure(IConfigurator configurator)
    {
        configurator.AddCommand<AddStepCommand>(Settings.CommandName)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    public class Settings : CommandSettings
    {
        public const string CommandName = "step";
        public const string CommandDescription = "Add a new step to a task";
        public static readonly string[] CommandExample = {"step", "add tomato to basket", "-t", "1", "-b", "market"};

        [Description("task content")]
        [CommandArgument(0, "<TEXT>")]
        public string Text { get; init; } = "";

        [Description("")]
        [CommandOption("-t|--task")]
        public string TaskId { get; init; } = "";

        [Description("board to which the task belongs")]
        [CommandOption("-b|--board")]
        [DefaultValue("board")]
        public string BoardName { get; init; } = "board";
    }
}