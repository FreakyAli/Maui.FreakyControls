using System;
using System.Windows.Input;
using Maui.FreakyControls;

namespace Samples.ColorPicker;

public class ColorPickerViewModel : MainViewModel
{
    private string colorHex;
    private double xPosition;
    private double yPosition;

    public string ColorHex
    {
        get => colorHex;
        set => SetProperty(ref colorHex, value);
    }

    public double XPosition
    {
        get => xPosition;
        set => SetProperty(ref xPosition, value);
    }

    public double YPosition
    {
        get => yPosition;
        set => SetProperty(ref yPosition, value);
    }

    public ICommand ColorPickerCommand { get; set; }

    public ColorPickerViewModel()
    {
        ColorPickerCommand = new Command<object>((colorArgs) =>
        {
            var eventArgs = colorArgs as FreakyColorPickerEventArgs;
            ColorHex = eventArgs.Color.ToArgbHex();
        });
    }
}