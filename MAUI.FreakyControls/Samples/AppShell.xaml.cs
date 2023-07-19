using Samples.Checkboxes;
using Samples.ImageViews;
using Samples.Pickers;
using Samples.RadioButtons;
using Samples.SignatureView;
using Samples.TextInputLayout;

namespace Samples;

public partial class AppShell : Shell
{
    internal const string buttons = "Buttons";
    internal const string checkboxes = "Checkboxes";
    internal const string imageViews = "ImageViews";
    internal const string inputViews = "InputViews";
    internal const string pickers = "Pickers";
    internal const string radioButtons = "RadioButtons";
    internal const string signaturePreview = "ImageDisplay";
    internal const string signatureView = "SignatureView";
    internal const string textInputLayout = "TextInputLayouts";
    internal const string slider = "SliderView";

    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(inputViews, typeof(InputViews.InputViews));
        Routing.RegisterRoute(pickers, typeof(PickersView));
        Routing.RegisterRoute(textInputLayout, typeof(TextInputLayoutView));
        Routing.RegisterRoute(imageViews, typeof(ImagesPage));
        Routing.RegisterRoute(signatureView, typeof(SignatureView.SignatureView));
        Routing.RegisterRoute(signaturePreview, typeof(ImageDisplay));
        Routing.RegisterRoute(checkboxes, typeof(CheckboxesView));
        Routing.RegisterRoute(radioButtons, typeof(RadioButtonsView));
        Routing.RegisterRoute(buttons, typeof(ButtonsView.ButtonsView));
        Routing.RegisterRoute(slider, typeof(SliderView.SliderView));
    }
}