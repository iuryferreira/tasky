global using Spectre.Console.Cli;
global using JetBrains.Annotations;
using System.Globalization;
using System.Text;
using Tasky.Cli.Commands;
using Tasky.Cli.Initialization;

Console.OutputEncoding = Encoding.UTF8;
FluentValidation.ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");

Container.BuildProvider();

var app = new CommandApp<DefaultCommand>(Container.Register);
app.Configure(Container.ConfigureCommands);

await app.RunAsync(args);