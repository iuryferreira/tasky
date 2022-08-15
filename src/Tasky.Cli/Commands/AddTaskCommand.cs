using System.ComponentModel;
using MediatR;
using Spectre.Console.Cli;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Shared;

namespace Tasky.Cli.Commands;

public class AddTaskCommand : BaseCommand<AddTaskCommand.Settings>
{
    public AddTaskCommand(IMediator mediator, IConsoleWriter writer) : base(mediator, writer)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var request = new Requests.AddTaskOnBoard(new Dtos.AddTaskOnBoardRequestDto(settings.BoardName, settings.Text));
        var boards = await Mediator.Send(request);
        Writer.ShowBoards(boards);
        return 0;
    }

    public static void Configure(IConfigurator configurator)
    {
        configurator.AddCommand<AddTaskCommand>(Settings.CommandName)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    public class Settings : CommandSettings
    {
        public const string CommandName = "add";
        public const string CommandDescription = "Add a new task to a board";
        public static readonly string[] CommandExample = {"add", "add tomato to basket", "-b", "market"};

        [Description("task content")]
        [CommandArgument(0, "<TEXT>")]
        public string Text { get; init; } = "";

        [Description("board to which the task belongs")]
        [CommandOption("-b|--board")]
        [DefaultValue("board")]
        public string BoardName { get; init; } = "board";
    }
}