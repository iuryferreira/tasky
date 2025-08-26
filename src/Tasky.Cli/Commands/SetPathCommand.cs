using System.ComponentModel;
using MediatR;
using Notie.Contracts;
using Tasky.Cli.Contracts;
using Tasky.Cli.UserInterface;
using Tasky.Core.Application.Handlers;
using Tasky.Shared;

namespace Tasky.Cli.Commands;

public class SetPathCommand : BaseCommand<SetPathCommand.Settings>
{
    public SetPathCommand(IMediator mediator, IConsoleWriter writer, INotifier notifier) 
        : base(mediator, writer, notifier) { }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        return await Handle(async () =>
        {
            await Mediator.Send(new Requests.SetDatabasePath(settings.Path));

            Writer.WriteSuccess($"✅ Database path override saved: {settings.Path}");
            Writer.WriteInfo("It will be used next time Tasky runs.");
            return 0;
        });
    }

    public static void Configure(IConfigurator configurator)
    {
        configurator.AddCommand<SetPathCommand>(Settings.CommandName)
            .WithDescription(Settings.CommandDescription)
            .WithExample(Settings.CommandExample);
    }

    [UsedImplicitly]
    public sealed class Settings : CommandSettings
    {
        public const string CommandName = "set-path";
        public const string CommandDescription = "Override the location of the Tasky database file.";
        public static readonly string[] CommandExample = { "set-path --path D:/tasky/mydata.json" };

        public Settings(string path)
        {
            Path = path;
        }

        [Description("Full path to database file")]
        [CommandOption("-p|--path <PATH>")]
        public string Path { get; }
    }
}