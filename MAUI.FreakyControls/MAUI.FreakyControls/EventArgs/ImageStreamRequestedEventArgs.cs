//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

public class ImageStreamRequestedEventArgs : FreakyEventArgs
{
    public ImageStreamRequestedEventArgs(SignatureImageFormat imageFormat, ImageConstructionSettings settings)
    {
        ImageFormat = imageFormat;
        Settings = settings;
    }

    public SignatureImageFormat ImageFormat { get; private set; }

    public ImageConstructionSettings Settings { get; private set; }

    public Task<Stream> ImageStreamTask { get; set; } = Task.FromResult<Stream>(null);
}