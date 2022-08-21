using System.Text.Json;
using Tasky.Core.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Tasky.Core.Infrastructure;

public class FileDbContext : IContext
{
    public async Task SaveAsync(IEnumerable<Board> boards)
    {
        var data = JsonSerializer.Serialize(new Database(DateTime.Now, boards),
            new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true });
        await File.WriteAllTextAsync("database.json", data);
    }

    public async Task<IEnumerable<Board>> ReadAsync()
    {
        var data = await File.ReadAllTextAsync("database.json");
        var database =
            JsonSerializer.Deserialize<Database>(data, new JsonSerializerOptions(JsonSerializerDefaults.Web)) ??
            new Database(DateTime.Now, new List<Board>());
        return database.Boards;
    }
}

public record Database(DateTime UpdatedAt, IEnumerable<Board> Boards);