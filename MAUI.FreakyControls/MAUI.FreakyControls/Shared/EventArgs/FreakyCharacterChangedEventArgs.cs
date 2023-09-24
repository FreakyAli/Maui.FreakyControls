//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

public class FreakyCharacterChangedEventArgs : FreakyEventArgs
{
    public string SelectedCharacter { get; set; }
}