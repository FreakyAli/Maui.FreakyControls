using System;
namespace Samples.Buttons;

public class ButtonsViewModel : BaseViewModel
{
    public ButtonsViewModel()
    {
    }

    public bool IsButtonEnabled = true;

    bool isToggled;
    public bool IsToggled
    {
        get => isToggled;
        set => SetProperty(ref isToggled, value);
    }

    // Just demonstrates the use of Commands
    Command buttonClickedCommand;

    public Command ButtonClickedCommand => buttonClickedCommand ?? (buttonClickedCommand = new Command(() =>
    {
        Application.Current.MainPage.DisplayAlert("Hello from the View Model", "The Flex Button rocks!", "Yeah");
    }, () => IsButtonEnabled));
}