namespace Maui.FreakyControls;

public sealed class FreakyAutoCompleteViewSuggestionChosenEventArgs : FreakyEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FreakyAutoCompleteViewSuggestionChosenEventArgs"/> class.
    /// </summary>
    /// <param name="selectedItem"></param>
    internal FreakyAutoCompleteViewSuggestionChosenEventArgs(object selectedItem)
    {
        SelectedItem = selectedItem;
    }

    /// <summary>
    /// Gets a reference to the selected item.
    /// </summary>
    /// <value>A reference to the selected item.</value>
    public object SelectedItem { get; }
}