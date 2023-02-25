//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

public class FreakyRadioButtonEventArgs : EventArgs
{
    public string RadioButtonName { get; }
    public int RadioButtonIndex { get; }

    public FreakyRadioButtonEventArgs(string name, int index)
    {
        RadioButtonName = name;
        RadioButtonIndex = index;
    }
}