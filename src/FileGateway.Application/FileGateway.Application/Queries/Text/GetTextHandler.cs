using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using MediatR;

namespace FileGateway.Application.Queries.Text
{
    public class GetTextHandler : IRequestHandler<GetTextQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecretRepository _secretRepository;

        public GetTextHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = Ensure.IsNotNull(unitOfWork);
            _secretRepository = Ensure.IsNotNull(unitOfWork.Secret);
        }
        public async Task<string> Handle(GetTextQuery request, CancellationToken cancellationToken)
        {
            var secret = await _secretRepository.GetSecretByTokenAsync(request.Token, cancellationToken);
            if (secret is null)
            {
                return string.Empty;
            }

            if (secret.DeleteAfterDownload)
            {
                secret.Status = Domain.Enums.FileStatus.Removed;
                await _secretRepository.UpdateAsync(secret.Id, secret, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return secret.ContentType;
        }
    }
}
