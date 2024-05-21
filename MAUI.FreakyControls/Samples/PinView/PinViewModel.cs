using CommunityToolkit.Mvvm.ComponentModel;

namespace Samples.PinView;

public partial class PinViewModel : BaseViewModel
{
    [ObservableProperty]
    private double heightRequest;

    [ObservableProperty]
    private double widthRequest;

    public PinViewModel()
    {
        var width = (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - 30;
        WidthRequest = width / 3;
        double height;
        if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
        {
            height = ((DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) / 7) - 30;
        }
        else
        {
            height = ((DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) / 2) - 30;
        }

        HeightRequest = height;
    }
}