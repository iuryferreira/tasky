using System.Text;
using Spectre.Console.Cli;
using Tasky.Cli.Commands.Input;
using Tasky.Cli.Initialization;

Console.OutputEncoding = Encoding.UTF8;

Container.BuildProvider();

var app = new CommandApp<DefaultCommand>(Container.Register);
app.Configure(Container.ConfigureCommands);

await app.RunAsync(args);