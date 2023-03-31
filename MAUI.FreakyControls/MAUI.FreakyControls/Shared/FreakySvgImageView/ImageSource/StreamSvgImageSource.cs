namespace Maui.FreakyControls;

public partial class StreamSvgImageSource : SvgImageSource, IStreamImageSource
{
    public static readonly BindableProperty StreamProperty = BindableProperty.Create(nameof(Stream), typeof(Func<CancellationToken, Task<Stream>>), typeof(StreamSvgImageSource),
        default(Func<CancellationToken, Task<Stream>>));

    public override bool IsEmpty => Stream == null;

    public virtual Func<CancellationToken, Task<Stream>> Stream
    {
        get { return (Func<CancellationToken, Task<Stream>>)GetValue(StreamProperty); }
        set { SetValue(StreamProperty, value); }
    }

    protected override void OnPropertyChanged(string propertyName)
    {
        if (propertyName == StreamProperty.PropertyName)
            OnSourceChanged();
        base.OnPropertyChanged(propertyName);
    }

    async Task<Stream> IStreamImageSource.GetStreamAsync(CancellationToken userToken)
    {
        if (IsEmpty)
            return null;

        OnLoadingStarted();
        userToken.Register(CancellationTokenSource.Cancel);
        Stream stream = null;
        try
        {
            stream = await Stream(CancellationTokenSource.Token);
            OnLoadingCompleted(false);
        }
        catch (OperationCanceledException)
        {
            OnLoadingCompleted(true);
            throw;
        }
        return stream;
    }
}