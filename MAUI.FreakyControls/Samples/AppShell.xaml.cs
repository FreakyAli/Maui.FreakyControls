namespace Samples;

public partial class AppShell : Shell
{
    internal const string Buttons = "Buttons";
    internal const string Checkboxes = "Checkboxes";
    internal const string ImageViews = "ImageViews";
    internal const string InputViews = "InputViews";
    internal const string Pickers = "Pickers";
    internal const string RadioButtons = "RadioButtons";
    internal const string SignaturePreview = "ImageDisplay";
    internal const string SignatureView = "SignatureView";
    internal const string TextInputLayout = "TextInputLayouts";
    internal const string JumpList = "JumpList";
    internal const string PinView = "PinView";
    internal const string Switches = "Switch";
    internal const string ZoomImage = "ZoomImage";
    internal const string ScratchView = "ScratchView";

    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(InputViews, typeof(InputViews.InputViews));
        Routing.RegisterRoute(Pickers, typeof(Pickers.PickersView));
        Routing.RegisterRoute(TextInputLayout, typeof(TextInputLayout.TextInputLayoutView));
        Routing.RegisterRoute(ImageViews, typeof(ImageViews.ImagesPage));
        Routing.RegisterRoute(SignatureView, typeof(SignatureView.SignatureView));
        Routing.RegisterRoute(SignaturePreview, typeof(SignatureView.ImageDisplay));
        Routing.RegisterRoute(Checkboxes, typeof(Checkboxes.CheckboxesView));
        Routing.RegisterRoute(RadioButtons, typeof(RadioButtons.RadioButtonsView));
        Routing.RegisterRoute(Buttons, typeof(ButtonsView.ButtonsView));
        Routing.RegisterRoute(JumpList, typeof(JumpList.JumpListView));
        Routing.RegisterRoute(PinView, typeof(PinView.PinView));
        Routing.RegisterRoute(Switches, typeof(Switch.SwitchsView));
        Routing.RegisterRoute(ZoomImage, typeof(ZoomImage.ZoomImageView));
        Routing.RegisterRoute(ScratchView, typeof(ScratchView.ScratchView));
    }
}