﻿using System.ComponentModel;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Core.Domain;
using Tasky.Shared;

namespace Tasky.Cli.Commands.Steps;

public sealed class BeginStepCommand : BaseCommand<BeginStepCommand.Settings>
{
    public BeginStepCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier) : base(mediator, writer, notifier)
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var ids = settings.Id.Split('.');
        if (ids.Length <= 1) return 0;

        var data = new Dtos.ChangeStepStatusRequestDto(settings.Id, ids[0], settings.BoardName);
        var request = new Requests.ChangeStepStatus(data, Status.InProgress);
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
        configurator.AddCommand<BeginStepCommand>(Settings.CommandName)
            .WithAlias(Settings.CommandAlias)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    [UsedImplicitly]
    public sealed class Settings : CommandSettings
    {
        public const string CommandName = "beginstep";
        public const string CommandAlias = "bs";

        public const string CommandDescription =
            "Marks a step already created as begin informing the id and board";

        public static readonly string[] CommandExample = {"beginstep", "1.1", "-b", "shopping"};

        [Description("step id")]
        [CommandArgument(0, "<STEP_ID>")]
        public string Id { get; init; } = "";

        [Description("board to which the task belongs")]
        [CommandOption("-b|--board")]
        [DefaultValue("board")]
        public string BoardName { get; init; } = "board";
    }
}