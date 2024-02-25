using System.Windows.Input;

namespace Samples.ButtonsView;

public class ButtonsViewModel : MainViewModel
{
    public bool isButtonEnabled= false;

    public bool IsButtonEnabled
    {
        get => isButtonEnabled;
        set => SetProperty(ref isButtonEnabled, value);
    }

    public ICommand ButtonStateCommand { get; set; }

    public ButtonsViewModel() =>
        ButtonStateCommand = new Command(() => { IsButtonEnabled = !IsButtonEnabled; });
}