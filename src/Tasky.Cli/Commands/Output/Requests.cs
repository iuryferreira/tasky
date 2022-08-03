using MediatR;
using Tasky.Core.Domain;

namespace Tasky.Cli.Commands.Output;

public static class Requests
{
    public record ListTaskOutputRequest(IReadOnlyList<Board> Boards) : IRequest;
}