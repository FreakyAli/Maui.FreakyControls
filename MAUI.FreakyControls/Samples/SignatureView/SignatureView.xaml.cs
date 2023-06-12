using Maui.FreakyControls.Extensions;

namespace Samples.SignatureView;

public partial class SignatureView : ContentPage
{
    SignatureViewModel viewModel;
    public SignatureView()
    {
        InitializeComponent();
        this.BindingContext = viewModel = new SignatureViewModel();
    }

    async void FreakySignaturePadView_StrokeCompleted(System.Object sender, System.EventArgs e)
    {
        viewModel.ImageStream = Stream.Null;
        var imageStream = await svgPad.GetImageStreamAsync(Maui.FreakyControls.SignatureImageFormat.Jpeg, shouldCrop:false);
        viewModel.ImageStream = imageStream;
    }
}
