namespace FileGateway.Application;

public class TrackingStream : Stream
{
    private readonly Stream _inner;
    private readonly Func<Task> _onCompleted;
    private bool _disposed;

    public TrackingStream(Stream inner, Func<Task> onCompleted)
    {
        _inner = inner;
        _onCompleted = onCompleted;
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _inner.Dispose();
            _onCompleted().GetAwaiter().GetResult();
        }
        _disposed = true;
        base.Dispose(disposing);
    }

    public override async ValueTask DisposeAsync()
    {
        await _inner.DisposeAsync();
        await _onCompleted();
        await base.DisposeAsync();
    }

    // Forward tất cả các method còn lại
    public override bool CanRead => _inner.CanRead;
    public override bool CanSeek => _inner.CanSeek;
    public override bool CanWrite => _inner.CanWrite;
    public override long Length => _inner.Length;
    public override long Position { get => _inner.Position; set => _inner.Position = value; }
    public override void Flush() => _inner.Flush();
    public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);
    public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
    public override void SetLength(long value) => _inner.SetLength(value);
    public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);
}
