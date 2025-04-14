using System.Diagnostics;
using System.Threading;

namespace Maui.FreakyControls.Wrappers
{
    internal class StreamWrapper : Stream
    {
        private readonly Stream _wrapped;
        private IDisposable _additionalDisposable;

        public StreamWrapper(Stream wrapped)
            : this(wrapped, null)
        {
        }

        public StreamWrapper(Stream wrapped, IDisposable additionalDisposable)
        {
            ArgumentNullException.ThrowIfNull(wrapped);

            _wrapped = wrapped;
            _additionalDisposable = additionalDisposable;
        }

        public override bool CanRead
        {
            get { return _wrapped.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _wrapped.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _wrapped.CanWrite; }
        }

        public override long Length
        {
            get { return _wrapped.Length; }
        }

        public override long Position
        {
            get { return _wrapped.Position; }
            set { _wrapped.Position = value; }
        }

        public event EventHandler Disposed;

        public override void Flush()
        {
            _wrapped.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _wrapped.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _wrapped.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _wrapped.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _wrapped.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            _wrapped.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);
            _additionalDisposable?.Dispose();
            _additionalDisposable = null;

            base.Dispose(disposing);
        }

        public static async Task<Stream> GetStreamAsync(Uri uri, HttpClient client, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                Trace.WriteLine("Could not retrieve {Uri}, status code {StatusCode}");
                Trace.WriteLine(uri);
                Trace.WriteLine(response.StatusCode);
                return null;
            }

            // the HttpResponseMessage needs to be disposed of after the calling code is done with the stream
            // otherwise the stream may get disposed before the caller can use it
            return new StreamWrapper(await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false), response);
        }
    }
}