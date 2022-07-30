using MediatR;

namespace Tasky.Cli.Commands.Output;

public static class Requests
{
    public record ListTaskOutputRequest : IRequest;
}