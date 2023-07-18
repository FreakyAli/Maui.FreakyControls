using Maui.FreakyControls;

namespace Samples.SignatureView;

public partial class SignatureView : ContentPage
{
    private readonly SignatureViewModel viewModel;

    public SignatureView()
    {
        InitializeComponent();
        BindingContext = viewModel = new SignatureViewModel();
    }

    private async void FreakySignaturePadView_StrokeCompleted(object sender, EventArgs e)
    {
        viewModel.ImageStream = Stream.Null;
        var imageStream = await svgPad.GetImageStreamAsync(SignatureImageFormat.Jpeg, false);
        viewModel.ImageStream = imageStream;
    }
}