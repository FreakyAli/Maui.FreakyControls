using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Maui.FreakyControls.Shared.Wrappers;

namespace Maui.FreakyControls;

[System.ComponentModel.TypeConverter(typeof(ImageSourceConverter))]
public abstract partial class SvgImageSource : Element
{
    readonly object _synchandle = new object();

    CancellationTokenSource _cancellationTokenSource;

    TaskCompletionSource<bool> _completionSource;

    readonly WeakEventManager _weakEventManager = new WeakEventManager();

    protected SvgImageSource()
    {

    }

    public virtual bool IsEmpty => false;

    public static bool IsNullOrEmpty(SvgImageSource imageSource) =>
        imageSource == null || imageSource.IsEmpty;

    private protected CancellationTokenSource CancellationTokenSource
    {
        get { return _cancellationTokenSource; }
        private set
        {
            if (_cancellationTokenSource == value)
                return;
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
            _cancellationTokenSource = value;
        }
    }

    bool IsLoading
    {
        get { return _cancellationTokenSource != null; }
    }

    public virtual Task<bool> Cancel()
    {
        if (!IsLoading)
            return Task.FromResult(false);

        var tcs = new TaskCompletionSource<bool>();
        TaskCompletionSource<bool> original = Interlocked.CompareExchange(ref _completionSource, tcs, null);
        if (original == null)
        {
            _cancellationTokenSource.Cancel();
        }
        else
            tcs = original;

        return tcs.Task;
    }

    public static SvgImageSource FromFile(string file)
    {
        return new FileSvgImageSource { File = file };
    }

    public static SvgImageSource FromResource(string resource, Type resolvingType)
    {
        return FromResource(resource, resolvingType.Assembly);
    }

    public static SvgImageSource FromResource(string resource, Assembly sourceAssembly = null)
    {
        sourceAssembly = sourceAssembly ?? Assembly.GetCallingAssembly();
        return FromStream(() => sourceAssembly.GetManifestResourceStream(resource));
    }

    public static SvgImageSource FromStream(Func<Stream> stream)
    {
        return new StreamSvgImageSource { Stream = token => Task.Run(stream, token) };
    }

    public static SvgImageSource FromStream(Func<CancellationToken, Task<Stream>> stream)
    {
        return new StreamSvgImageSource { Stream = stream };
    }

    public static SvgImageSource FromUri(Uri uri)
    {
        if (!uri.IsAbsoluteUri)
            throw new ArgumentException("uri is relative");
        return new UriSvgImageSource { Uri = uri };
    }

    public static implicit operator SvgImageSource(string source)
    {
        Uri uri;
        return Uri.TryCreate(source, UriKind.Absolute, out uri) && uri.Scheme != "file" ? FromUri(uri) : FromFile(source);
    }

    public static implicit operator SvgImageSource(Uri uri)
    {
        if (uri == null)
            return null;

        if (!uri.IsAbsoluteUri)
            throw new ArgumentException("uri is relative");
        return FromUri(uri);
    }

    private protected void OnLoadingCompleted(bool cancelled)
    {
        if (!IsLoading || _completionSource == null)
            return;

        TaskCompletionSource<bool> tcs = Interlocked.Exchange(ref _completionSource, null);
        if (tcs != null)
            tcs.SetResult(cancelled);

        lock (_synchandle)
        {
            CancellationTokenSource = null;
        }
    }

    private protected void OnLoadingStarted()
    {
        lock (_synchandle)
        {
            CancellationTokenSource = new CancellationTokenSource();
        }
    }

    protected void OnSourceChanged()
    {
        _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(SourceChanged));
    }

    internal event EventHandler SourceChanged
    {
        add { _weakEventManager.AddEventHandler(value); }
        remove { _weakEventManager.RemoveEventHandler(value); }
    }
}