using Spectre.Console;

namespace Tasky.Cli.UserInterface;

public class Render : IRender
{
    public void Print(string message) => AnsiConsole.Write(new Markup(message));
    public void Print(Content content) => AnsiConsole.Write(new Markup(content.Text));
}

public interface IRender
{
    void Print(string message);
    void Print(Content content);
}