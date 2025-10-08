using FileGateway.Domain;
using FileGateway.Domain.Abstractions;

namespace FileGateway.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed = false;
    private readonly AppDbContext _context;

    private IUserRepository? _userRepository;

    public IUserRepository User
    {
        get { return _userRepository ??= new UserRepository(_context); }
    }

    public UnitOfWork(AppDbContext context)
    {
        _context = Ensure.IsNotNull(context);
    }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Giải phóng các resource managed
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
