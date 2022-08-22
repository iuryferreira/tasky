using Tasky.Core.Domain.Entities;

namespace Tasky.Core.Infrastructure.Repositories;

public interface IBoardRepository
{
    Task<Board?> GetByNameAsync(string name);
    Task<IEnumerable<Board>> AddAsync(Board board);
    Task<IEnumerable<Board>> UpdateAsync(Board board);
    Task<List<Board>> AllAsync();
    System.Threading.Tasks.Task SaveBoardsAsync(IEnumerable<Board> boards);
}

public class BoardRepository : IBoardRepository
{
    private readonly IContext _context;

    public BoardRepository(IContext context)
    {
        _context = context;
    }

    public async Task<List<Board>> AllAsync()
    {
        var boards = (await _context.ReadAsync()).ToList();
        return boards;
    }

    public async Task<Board?> GetByNameAsync(string name)
    {
        var boards = await _context.ReadAsync();
        return boards.FirstOrDefault(b => b.Name.Equals(name));
    }

    public async Task<IEnumerable<Board>> AddAsync(Board board)
    {
        var boards = await AllAsync();
        boards.Add(board);
        await _context.SaveAsync(boards);
        return boards;
    }

    public async Task<IEnumerable<Board>> UpdateAsync(Board board)
    {
        var boards = await AllAsync();
        var index = boards.FindIndex(b => b.Name.Equals(board.Name));
        boards[index] = board;
        await _context.SaveAsync(boards);
        return boards;
    }

    public async System.Threading.Tasks.Task SaveBoardsAsync(IEnumerable<Board> boards)
    {
        await _context.SaveAsync(boards);
    }
}