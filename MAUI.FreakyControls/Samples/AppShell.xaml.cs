using Samples.InputViews;

namespace Samples;

public partial class AppShell : Shell
{
    internal const string inputViews = "InputViews";
    internal const string textInputLayout = "TextInputLayouts";
    internal const string pickers = "Pickers";
    internal const string imageViews = "ImageViews";
    internal const string signatureView = "SignatureView";
    internal const string signaturePreview = "ImageDisplay";
    internal const string scratchView = "ScratchView";

    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(inputViews, typeof(InputViews.InputViews));
        Routing.RegisterRoute(pickers, typeof(Pickers.PickersView));
        Routing.RegisterRoute(textInputLayout, typeof(TextInputLayout.TextInputLayoutView));
        Routing.RegisterRoute(imageViews, typeof(ImageViews.ImagesPage));
        Routing.RegisterRoute(signatureView, typeof(SignatureView.SignatureView));
        Routing.RegisterRoute(signaturePreview, typeof(SignatureView.ImageDisplay));
        Routing.RegisterRoute(scratchView, typeof(ScratchView.ScratchViewPage));
    }
}

