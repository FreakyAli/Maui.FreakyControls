using System;

//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

/// <summary>
/// 
/// </summary>
public class FreakyEventArgs : EventArgs
{

}

/// <summary>
/// Provides event data for the AutoSuggestBox.QuerySubmitted event.
/// </summary>
public sealed class AutoSuggestBoxQuerySubmittedEventArgs : FreakyEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> class.
    /// </summary>
    /// <param name="queryText"></param>
    /// <param name="chosenSuggestion"></param>
    internal AutoSuggestBoxQuerySubmittedEventArgs(string queryText, object chosenSuggestion)
    {
        QueryText = queryText;
        ChosenSuggestion = chosenSuggestion;
    }

    /// <summary>
    /// Gets the suggested result that the use chose.
    /// </summary>
    /// <value>The suggested result that the use chose.</value>
    public object ChosenSuggestion { get; }

    /// <summary>
    /// The query text of the current search.
    /// </summary>
    /// <value>Gets the query text of the current search.</value>
    public string QueryText { get; }
}

/// <summary>
/// Provides data for the <see cref="SuggestionChosen"/> event.
/// </summary>
public sealed class AutoSuggestBoxSuggestionChosenEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoSuggestBoxSuggestionChosenEventArgs"/> class.
    /// </summary>
    /// <param name="selectedItem"></param>
    internal AutoSuggestBoxSuggestionChosenEventArgs(object selectedItem)
    {
        SelectedItem = selectedItem;
    }

    /// <summary>
    /// Gets a reference to the selected item.
    /// </summary>
    /// <value>A reference to the selected item.</value>
    public object SelectedItem { get; }
}

/// <summary>
/// Provides data for the TextChanged event.
/// </summary>
public sealed class AutoSuggestBoxTextChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoSuggestBoxTextChangedEventArgs"/> class.
    /// </summary>
    /// <param name="reason"></param>
    internal AutoSuggestBoxTextChangedEventArgs(AutoSuggestionBoxTextChangeReason reason)
    {
        Reason = reason;
    }

    /// <summary>
    /// Returns a Boolean value indicating if the current value of the TextBox is unchanged from the point in time when the TextChanged event was raised.
    /// </summary>
    /// <returns>Indicates if the current value of the TextBox is unchanged from the point in time when the TextChanged event was raised.</returns>
    public bool CheckCurrent() => true; //TODO

    /// <summary>
    /// Gets or sets a value that indicates the reason for the text changing in the AutoSuggestBox.
    /// </summary>
    /// <value>The reason for the text changing in the AutoSuggestBox.</value>
    public AutoSuggestionBoxTextChangeReason Reason { get; }
}

/// <summary>
/// Provides data for the <see cref="AutoCompleteView.TextChanged"/> event.
/// </summary>
public enum AutoSuggestionBoxTextChangeReason
{
    /// <summary>The user edited the text.</summary>
    UserInput = 0,

    /// <summary>The text was changed via code.</summary>
    ProgrammaticChange = 1,

    /// <summary>The user selected one of the items in the auto-suggestion box.</summary>
    SuggestionChosen = 2
}