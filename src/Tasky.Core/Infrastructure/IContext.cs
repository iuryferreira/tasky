using Tasky.Core.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Tasky.Core.Infrastructure;

public interface IContext
{
    Task Save(IEnumerable<Board> boards);
    Task<IEnumerable<Board>> Read();
}