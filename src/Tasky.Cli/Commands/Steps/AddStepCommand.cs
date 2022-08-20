﻿using System.ComponentModel;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Shared;

namespace Tasky.Cli.Commands.Steps;

public class AddStepCommand : BaseCommand<AddStepCommand.Settings>
{
    public AddStepCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier) : base(mediator, writer, notifier)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var data = new Dtos.AddStepOnTaskRequestDto(settings.TaskId, settings.BoardName, settings.Text);
        var request = new Requests.AddStepOnTask(data);
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
        configurator.AddCommand<AddStepCommand>(Settings.CommandName)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    [UsedImplicitly]
    public sealed class Settings : CommandSettings
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