//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

public class FreakyRadioButtonEventArgs : EventArgs
{
    public FreakyRadioButtonEventArgs(string name, int index)
    {
        RadioButtonName = name;
        RadioButtonIndex = index;
    }

    public int RadioButtonIndex { get; }
    public string RadioButtonName { get; }
}