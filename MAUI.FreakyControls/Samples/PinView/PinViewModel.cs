using CommunityToolkit.Mvvm.ComponentModel;

namespace Samples.PinView;

public partial class PinViewModel : BaseViewModel
{
    [ObservableProperty]
    private partial double HeightRequest {get; set;}

    [ObservableProperty]
    public partial double WidthRequest {get; set;}

    public PinViewModel()
    {
        WidthRequest = 50;
        HeightRequest = 100;
    }
}