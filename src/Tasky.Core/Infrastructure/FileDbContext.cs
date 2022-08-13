using System.Text.Json;
using Tasky.Core.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Tasky.Core.Infrastructure;

public class FileDbContext : IContext
{
    public async Task Save(IEnumerable<Board> boards)
    {
        var data = JsonSerializer.Serialize(new Database(DateTime.Now, boards),
            new JsonSerializerOptions {WriteIndented = true});
        await File.WriteAllTextAsync("database.json", data);
    }

    public async Task<IEnumerable<Board>> Read()
    {
        var data = await File.ReadAllTextAsync("database.json");
        var database = JsonSerializer.Deserialize<Database>(data) ?? new Database(DateTime.Now, new List<Board>());
        return database.Boards;
    }
}

public record Database(DateTime UpdatedAt, IEnumerable<Board> Boards);