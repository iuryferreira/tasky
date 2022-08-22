using System.Text.Json.Serialization;

namespace Tasky.Core.Domain.Entities;

public class Board
{
    [JsonConstructor]
    public Board(string name, List<Task> tasks)
    {
        Name = name;
        Tasks = tasks.ToList();
    }

    public string Name { get; }
    public List<Task> Tasks { get; }

    [JsonIgnore]
    public int Quantity => Tasks.Count;

    public void AddTask(Task task)
    {
        Tasks.Add(task);
    }

    public void SortTasks()
    {
        var count = 1;
        Tasks.ForEach(task =>
        {
            task.SetId($"{count}");
            task.SortSteps();
            count++;
        });
    }
}