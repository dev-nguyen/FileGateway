namespace FileGateway.Application;

public class TrackingStream : Stream
{
    private readonly Stream _inner;
    private readonly Func<Task>? _onCompleted;
    private bool _disposed;

    public TrackingStream(Stream inner, Func<Task>? onCompleted)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        _onCompleted = onCompleted;
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            try
            {
                _inner.Dispose();

                // Run callback in background, safely
                if (_onCompleted != null)
                {
                    _ = Task.Run(async () =>
                    {
                        try { await _onCompleted(); }
                        catch (Exception ex) { /* log ex */ }
                    });
                }
            }
            catch (Exception ex)
            {
                // log ex
            }
        }

        _disposed = true;
        // base.Dispose(disposing); // optional
    }

    public override async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        try
        {
            await _inner.DisposeAsync();
            if (_onCompleted != null)
                await _onCompleted();
        }
        catch (Exception ex)
        {
            // log ex
        }

        _disposed = true;
        GC.SuppressFinalize(this);
        await base.DisposeAsync();
    }

    // Stream forwarding
    public override bool CanRead => _inner.CanRead;
    public override bool CanSeek => _inner.CanSeek;
    public override bool CanWrite => _inner.CanWrite;
    public override bool CanTimeout => _inner.CanTimeout;
    public override long Length => _inner.Length;
    public override long Position { get => _inner.Position; set => _inner.Position = value; }
    public override int ReadTimeout { get => _inner.ReadTimeout; set => _inner.ReadTimeout = value; }
    public override int WriteTimeout { get => _inner.WriteTimeout; set => _inner.WriteTimeout = value; }

    public override void Flush() => _inner.Flush();
    public override Task FlushAsync(CancellationToken cancellationToken) => _inner.FlushAsync(cancellationToken);

    public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);
    public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
    public override void SetLength(long value) => _inner.SetLength(value);
    public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        => _inner.ReadAsync(buffer, offset, count, cancellationToken);
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        => _inner.ReadAsync(buffer, cancellationToken);
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        => _inner.WriteAsync(buffer, cancellationToken);
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        => _inner.WriteAsync(buffer, offset, count, cancellationToken);
}
