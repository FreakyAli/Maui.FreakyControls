using Maui.FreakyControls;
using System.Windows.Input;

namespace Samples.RadioButtons;

public class RadioButtonsViewModel : MainViewModel
{
    private int checkedRadioButton;

    public RadioButtonsViewModel()
    {
        CheckedRadioButton = 2;
        SelectedIndexCommand = new Command<FreakyRadioButtonEventArgs>(ExecuteSelectedIndexCommand);
    }

    public int CheckedRadioButton
    {
        get => checkedRadioButton;
        set => SetProperty(ref checkedRadioButton, value);
    }

    public ICommand SelectedIndexCommand { get; set; }

    private void ExecuteSelectedIndexCommand(FreakyRadioButtonEventArgs obj)
    {
    }
}