namespace Tasky.Core.Domain.Entities;

public class Task
{
    public string Id { get; init; } = "";
    public string Text { get; init; } = "";
    public Status Status { get; init; }
    public List<Step> Steps { get; init; } = new();
}

public class Step
{
    public string Id { get; init; } = "";
    public string Text { get; init; } = "";
    public Status Status { get; init; }
}