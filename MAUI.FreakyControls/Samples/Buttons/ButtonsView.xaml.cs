using Maui.FreakyControls.Shared.Enums;

namespace Samples.Buttons;

public partial class ButtonsView : ContentPage
{
	public ButtonsView()
	{
		InitializeComponent();
        BindingContext = new ButtonsViewModel();
    }

    void DemoButton_TouchedDown(object sender, EventArgs e)
    {
        ButtonStatus.Text = "Pressed";
    }

    void DemoButton_TouchedUp(object sender, EventArgs e)
    {
        ButtonStatus.Text = "Released";
    }

    void DemoButton_TouchedCanceled(object sender, EventArgs e)
    {
        ButtonStatus.Text = "Canceled";
    }

    void ToggleIsEnabled_Clicked(object sender, EventArgs e)
    {
        ((ButtonsViewModel)BindingContext).IsButtonEnabled = !((ButtonsViewModel)BindingContext).IsButtonEnabled;
        ((ButtonsViewModel)BindingContext).ButtonClickedCommand.ChangeCanExecute();
    }

    void ToggleIconOrientation_Clicked(object sender, EventArgs e)
    {
        switch (WideButton.IconOrientation)
        {
            case IconOrientation.Left:
                WideButton.IconOrientation = IconOrientation.Right;
                break;
            case IconOrientation.Right:
                WideButton.IconOrientation = IconOrientation.Top;
                break;
            case IconOrientation.Top:
                WideButton.IconOrientation = IconOrientation.Left;
                break;
        }
    }

    public void ToggleHasShadow_Clicked(object sender, EventArgs e)
    {
        WideButton.HasShadow = !WideButton.HasShadow;
    }

    void ButtonWithoutBackground_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Hello from Code Behind", "The Flex Button rocks! ", "Yeah");
    }

    public void Handle_Toggled(object sender, ToggledEventArgs e)
    {
        ((Maui.FreakyControls.FreakyButton)sender).Text = e.Value.ToString();
    }

    public void ToggleIsToggled_Clicked(object sender, EventArgs e)
    {
        ((ButtonsViewModel)BindingContext).IsToggled = !((ButtonsViewModel)BindingContext).IsToggled;
    }

    private void ToggleIconTintEnabled(object sender, EventArgs e)
    {
        ColorIconButton.IconTintEnabled = !ColorIconButton.IconTintEnabled;
    }
}