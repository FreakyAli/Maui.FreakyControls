﻿using Maui.FreakyControls;
using System.Windows.Input;

namespace Samples.RadioButtons;

public class RadioButtonsViewModel : MainViewModel
{
    private int checkedRadioButton;

    public int CheckedRadioButton
    {
        get => checkedRadioButton;
        set => SetProperty(ref checkedRadioButton, value);
    }

    public ICommand SelectedIndexCommand { get; set; }

    public RadioButtonsViewModel()
    {
        CheckedRadioButton = 2;
        SelectedIndexCommand = new Command<FreakyRadioButtonEventArgs>(ExecuteSelectedIndexCommand) { };
    }

    private void ExecuteSelectedIndexCommand(FreakyRadioButtonEventArgs obj)
    {
    }
}