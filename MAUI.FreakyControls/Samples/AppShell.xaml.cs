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
<<<<<<< HEAD
    internal const string jumpList = "JumpList";
=======
    internal const string jumpList = "JumpList"; 
>>>>>>> master

    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(inputViews, typeof(InputViews.InputViews));
        Routing.RegisterRoute(pickers, typeof(Pickers.PickersView));
        Routing.RegisterRoute(textInputLayout, typeof(TextInputLayout.TextInputLayoutView));
        Routing.RegisterRoute(imageViews, typeof(ImageViews.ImagesPage));
        Routing.RegisterRoute(signatureView, typeof(SignatureView.SignatureView));
        Routing.RegisterRoute(signaturePreview, typeof(SignatureView.ImageDisplay));
        Routing.RegisterRoute(checkboxes, typeof(Checkboxes.CheckboxesView));
        Routing.RegisterRoute(radioButtons, typeof(RadioButtons.RadioButtonsView));
        Routing.RegisterRoute(buttons, typeof(ButtonsView.ButtonsView));
        Routing.RegisterRoute(jumpList, typeof(JumpList.JumpListView));
    }
}