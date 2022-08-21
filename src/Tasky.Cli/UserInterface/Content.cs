using Spectre.Console;

namespace Tasky.Cli.UserInterface;

public class Content
{
    public string Text { get; private set; } = string.Empty;

    public Content Set(string content)
    {
        Text = content;
        return this;
    }

    public Content Add(string content)
    {
        Text += content;
        return this;
    }

    public Content Add(Content content)
    {
        Text += content.Text;
        return this;
    }

    public override string ToString()
    {
        return Text;
    }
}

public static class ContentExtensions
{
    public static Content Bold(this Content content, bool condition = true)
    {
        if (condition) content.Set($"[bold]{content.Text}[/]");
        return content;
    }

    public static Content Underline(this Content content, bool condition = true)
    {
        if (condition) content.Set($"[underline]{content.Text}[/]");
        return content;
    }

    public static Content Italic(this Content content, bool condition = true)
    {
        if (condition) content.Set($"[italic]{content.Text}[/]");
        return content;
    }

    public static Content SpacesBefore(this Content content, int quantity, bool condition = true)
    {
        if (condition) content.Set(new string(' ', quantity) + content.Text);
        return content;
    }

    public static Content EscapeMarkup(this Content content, bool condition = true)
    {
        if (condition) content.Set(content.Text.EscapeMarkup());
        return content;
    }

    public static Content Grey(this Content content, bool condition = true)
    {
        if (condition) content.Set($"[grey]{content.Text}[/]");
        return content;
    }

    public static Content SlateBlue(this Content content, bool condition = true)
    {
        if (condition) content.Set($"[slateblue3]{content.Text}[/]");
        return content;
    }

    public static Content Green(this Content content, bool condition = true)
    {
        if (condition) content.Set($"[green]{content.Text}[/]");
        return content;
    }

    public static Content Purple(this Content content, bool condition = true)
    {
        if (condition) content.Set($"[purple]{content.Text}[/]");
        return content;
    }

    public static Content BreakLine(this Content content, int quantity = 1, bool condition = true)
    {
        if (condition) content.Set(content.Text + new string('\n', quantity));
        return content;
    }
}