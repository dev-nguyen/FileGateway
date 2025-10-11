using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using FileGateway.Domain.Entities;
using MediatR;

namespace FileGateway.Application.Commands;

public class CreateTextHandler : IRequestHandler<CreateTextCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretRepository _secretRepository;

    public CreateTextHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = Ensure.IsNotNull(unitOfWork);
        _secretRepository = Ensure.IsNotNull(unitOfWork.Secret);
    }
    public async Task<bool> Handle(CreateTextCommand request, CancellationToken cancellationToken)
    {
        var secret = new Secret
        {
            TextContent = request.Content,
            Owner = request.Onwer,
            DeleteAfterDownload = request.DeleteAfterDownload,
        };
        await _secretRepository.CreateAsync(secret, cancellationToken);
        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }
}
