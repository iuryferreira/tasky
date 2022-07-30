using Spectre.Console;

namespace Tasky.Cli.UserInterface;

public class Render : IRender
{
    public void Print(string message) => AnsiConsole.Write(new Markup(message));
}

public interface IRender
{
    void Print(string message);
}

public class Content
{
    public string Text { get; private set; } = string.Empty;

    public Content Set(string content)
    {
        Text = content;
        return this;
    }

    public override string ToString() => Text;
}

public static class ContentExtensions
{
    public static void Bold(this Content content, bool condition = true)
    {
        if (condition) content.Set($"[bold]{content.Text}[/]");
    }
}