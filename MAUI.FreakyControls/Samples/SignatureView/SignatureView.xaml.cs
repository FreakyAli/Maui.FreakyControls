namespace Samples.SignatureView;

public partial class SignatureView : ContentPage
{
    private SignatureViewModel viewModel;

    public SignatureView()
    {
        InitializeComponent();
        this.BindingContext = viewModel = new SignatureViewModel();
    }

    private async void FreakySignaturePadView_StrokeCompleted(System.Object sender, System.EventArgs e)
    {
        viewModel.ImageStream = Stream.Null;
        var imageStream = await svgPad.GetImageStreamAsync(Maui.FreakyControls.SignatureImageFormat.Jpeg, shouldCrop: false);
        viewModel.ImageStream = imageStream;
    }
}