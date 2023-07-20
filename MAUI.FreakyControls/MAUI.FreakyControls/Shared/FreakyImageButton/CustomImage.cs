namespace Maui.FreakyControls;

public class CustomImage : ContentView
{
    private Image image;

    private View customImage;

    public CustomImage()
    {
        Margin = new Thickness(0);
    }

    public void SetImage(string imageSource)
    {
        customImage = null;
        image = new Image
        {
            Aspect = Aspect.AspectFit,
            Source = imageSource
        };
        Content = image;
    }

    public void SetCustomImage(View view)
    {
        image = null;
        Content = view;
    }
}