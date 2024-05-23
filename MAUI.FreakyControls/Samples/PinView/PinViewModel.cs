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
        WidthRequest = 50;
        HeightRequest = 100;
    }
}