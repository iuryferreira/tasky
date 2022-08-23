using System.Text.Json;
using Tasky.Core.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Tasky.Core.Infrastructure;

public class FileDbContext : IContext
{
    private readonly Configuration _configuration;

    public FileDbContext(Configuration configuration)
    {
        _configuration = configuration;
    }

    public async Task SaveAsync(IEnumerable<Board> boards)
    {
        var data = JsonSerializer.Serialize(new Database(DateTime.Now, boards),
            new JsonSerializerOptions(JsonSerializerDefaults.Web) {WriteIndented = true});
        await File.WriteAllTextAsync(_configuration.DatabasePath, data);
    }

    public async Task<IEnumerable<Board>> ReadAsync()
    {
        if (!File.Exists(_configuration.DatabasePath))
        {
            await SaveAsync(Array.Empty<Board>());
        }

        var data = await File.ReadAllTextAsync(_configuration.DatabasePath);
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        var database = JsonSerializer.Deserialize<Database>(data, options) ?? Database.CreateEmpty();
        return database.Boards;
    }
}

public record Database(DateTime UpdatedAt, IEnumerable<Board> Boards)
{
    public static Database CreateEmpty() => new(DateTime.Now, new List<Board>());
};