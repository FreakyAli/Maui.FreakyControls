using System.ComponentModel;
using System.Globalization;
using Maui.FreakyControls.Shared.Wrappers;

namespace Maui.FreakyControls;

public sealed partial class UriSvgImageSource : SvgImageSource, IStreamImageSource
{
    public static readonly BindableProperty UriProperty = BindableProperty.Create(
        nameof(Uri), typeof(Uri), typeof(UriSvgImageSource), default(Uri),
        propertyChanged: (bindable, oldvalue, newvalue) => ((UriSvgImageSource)bindable).OnUriChanged(),
        validateValue: (bindable, value) => value == null || ((Uri)value).IsAbsoluteUri);

    public static readonly BindableProperty CacheValidityProperty = BindableProperty.Create(
        nameof(CacheValidity), typeof(TimeSpan), typeof(UriSvgImageSource), TimeSpan.FromDays(1));

    public static readonly BindableProperty CachingEnabledProperty = BindableProperty.Create(
        nameof(CachingEnabled), typeof(bool), typeof(UriSvgImageSource), true);

    public override bool IsEmpty => Uri == null;

    public TimeSpan CacheValidity
    {
        get => (TimeSpan)GetValue(CacheValidityProperty);
        set => SetValue(CacheValidityProperty, value);
    }

    public bool CachingEnabled
    {
        get => (bool)GetValue(CachingEnabledProperty);
        set => SetValue(CachingEnabledProperty, value);
    }

    [System.ComponentModel.TypeConverter(typeof(UriTypeConverter))]
    public Uri Uri
    {
        get => (Uri)GetValue(UriProperty);
        set => SetValue(UriProperty, value);
    }

    async Task<Stream> IStreamImageSource.GetStreamAsync(CancellationToken userToken)
    {
        if (IsEmpty)
            return null;

        OnLoadingStarted();
        userToken.Register(CancellationTokenSource.Cancel);
        Stream stream;

        try
        {
            stream = await GetStreamAsync(Uri, CancellationTokenSource.Token);
            OnLoadingCompleted(false);
        }
        catch (OperationCanceledException)
        {
            OnLoadingCompleted(true);
            throw;
        }
        catch (Exception ex)
        {
            //Application.Current?.FindMauiContext()?.CreateLogger<UriSvgImageSource>()?.LogWarning(ex, "Error getting stream for {Uri}", Uri);
            throw;
        }

        return stream;
    }

    public override string ToString()
    {
        return $"Uri: {Uri}";
    }

    async Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken = default(CancellationToken))
    {
        cancellationToken.ThrowIfCancellationRequested();

        Stream stream = null;

        if (CachingEnabled)
        {
            // TODO: CACHING https://github.com/dotnet/runtime/issues/52332

            // var key = GetKey();
            // var cached = TryGetFromCache(key, out stream)
            if (stream is null)
                stream = await DownloadStreamAsync(uri, cancellationToken).ConfigureAwait(false);
            // if (!cached)
            //    Cache(key, stream)
        }
        else
        {
            stream = await DownloadStreamAsync(uri, cancellationToken).ConfigureAwait(false);
        }

        return stream;
    }

    async Task<Stream> DownloadStreamAsync(Uri uri, CancellationToken cancellationToken)
    {
        try
        {
            using var client = new HttpClient();

            // Do not remove this await otherwise the client will dispose before
            // the stream even starts
            return await StreamWrapper.GetStreamAsync(uri, cancellationToken, client).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            //Application.Current?.FindMauiContext()?.CreateLogger<UriSvgImageSource>()?.LogWarning(ex, "Error getting stream for {Uri}", Uri);
            return null;
        }
    }

    void OnUriChanged()
    {
        CancellationTokenSource?.Cancel();
        OnSourceChanged();
    }
}