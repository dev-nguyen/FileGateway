using MediatR;

namespace FileGateway.Application.Queries;

public record GetTextQuery : IRequest<string>
{
    public string Token { get; set; }

    public GetTextQuery(string token)
    {
        Token = token;
    }
}
