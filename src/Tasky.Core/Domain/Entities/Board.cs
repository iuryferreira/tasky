namespace Tasky.Core.Domain.Entities;

public class Board
{
    public string Name { get; init; } = "board";
    public List<Task> Tasks { get; init; } = new();

    public int Quantity => Tasks.Count;
}