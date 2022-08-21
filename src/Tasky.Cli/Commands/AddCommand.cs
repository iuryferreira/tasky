using System.ComponentModel;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Shared;

namespace Tasky.Cli.Commands;

public class AddCommand : BaseCommand<AddCommand.Settings>
{
    public AddCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier) : base(mediator, writer, notifier)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        Dto data;
        Requests.Request request;

        if (!string.IsNullOrEmpty(settings.StepOf))
        {
            data = new AddStepOnTaskRequestDto(settings.StepOf, settings.BoardName, settings.Text);
            request = new Requests.AddStepOnTask((AddStepOnTaskRequestDto)data);
        }
        else
        {
            data = new AddTaskOnBoardRequestDto(settings.BoardName, settings.Text, settings.StepOf);
            request = new Requests.AddTaskOnBoard((AddTaskOnBoardRequestDto)data);
        }

        await Mediator.Send(request);
        return await Handle(async () =>
        {
            var boards = await Mediator.Send(new Requests.ListBoardsWithTasks());
            Writer.ShowBoards(boards);
            return 0;
        });
    }

    public static void Configure(IConfigurator configurator)
    {
        configurator.AddCommand<AddCommand>(Settings.CommandName)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    [UsedImplicitly]
    public sealed class Settings : CommandSettings
    {
        public const string CommandName = "add";
        public const string CommandDescription = "Add a new task/step to a board";
        public static readonly string[] CommandExample = { "add", "add tomato to basket", "-b", "market", "-s", "1" };

        [Description("task/step content")]
        [CommandArgument(0, "[TEXT]")]
        public string Text { get; init; } = "";

        [Description("board to which the task/step belongs")]
        [CommandOption("-b|--board")]
        public string BoardName { get; init; } = "";


        [Description("create step for specified task id")]
        [CommandOption("-s|--step-of")]
        public string StepOf { get; init; } = "";
    }
}