using Tasky.Core.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Tasky.Core.Infrastructure;

public interface IContext
{
    Task SaveAsync(IEnumerable<Board> boards);
    Task<IEnumerable<Board>> ReadAsync();
}