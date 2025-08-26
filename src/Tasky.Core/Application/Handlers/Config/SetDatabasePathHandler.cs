using System.Text.Json;
using MediatR;

namespace Tasky.Core.Application.Handlers.Config;

public class SetDatabasePathHandler : IRequestHandler<Requests.SetDatabasePath, Unit>
{
    private static readonly string OverrideFile;

    static SetDatabasePathHandler()
    {
        var basePath = OperatingSystem.IsWindows()
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tasky")
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "tasky");

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        OverrideFile = Path.Combine(basePath, "tasky.override.json");
    }

    public async Task<Unit> Handle(Requests.SetDatabasePath request, CancellationToken cancellationToken)
    {
        var config = new Dictionary<string, string?>
        {
            { "DatabasePathOverride", request.Path }
        };

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(OverrideFile, json, cancellationToken);

        return Unit.Value;
    }
}