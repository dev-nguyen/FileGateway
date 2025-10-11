using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using MediatR;

namespace FileGateway.Application.Commands;

public class UpdateTextHandler : IRequestHandler<UpdateTextCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecretRepository _secretRepository;

    public UpdateTextHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = Ensure.IsNotNull(unitOfWork);
        _secretRepository = Ensure.IsNotNull(unitOfWork.Secret);
    }
    public async Task<bool> Handle(UpdateTextCommand request, CancellationToken cancellationToken)
    {
        var secret = await _secretRepository.GetByIdAsync(request.Id, cancellationToken);
        if (secret is null)
        {
            return false;
        }
        secret.TextContent = request.Content;
        secret.DeleteAfterDownload = request.DeleteAfterDownload;

        await _secretRepository.UpdateAsync(secret.Id, secret, cancellationToken);
        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }
}
