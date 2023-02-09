using System;
namespace Samples.Checkboxes;

public class CheckboxesViewModel : MainViewModel
{
    private int checkedRadioButton;

    public int CheckedRadioButton
    {
        get => checkedRadioButton;
        set => SetProperty(ref checkedRadioButton, value);
    }
}
