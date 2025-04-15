namespace Samples.SignatureView;

public class ImageDisplay : ContentPage
{
    public ImageDisplay(Stream stream)
    {
        var imageSource = ImageSource.FromStream(() => stream);
        var imageView = new Image
        {
            Aspect = Aspect.AspectFit
        };
        Content = imageView;
        imageView.Source = imageSource;
    }
}