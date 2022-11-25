//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

public class IsBlankRequestedEventArgs : FreakyEventArgs
{
    public bool IsBlank { get; set; } = true;
}