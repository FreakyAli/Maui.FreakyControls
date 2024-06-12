using Maui.FreakyControls.Enums;

namespace Maui.FreakyControls;

public sealed class FreakyAutoCompleteViewTextChangedEventArgs : FreakyEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FreakyAutoCompleteViewTextChangedEventArgs"/> class.
    /// </summary>
    /// <param name="reason"></param>
    internal FreakyAutoCompleteViewTextChangedEventArgs(string text, TextChangeReason reason)
    {
        Text = text;
        Reason = reason;
    }

    public string Text { get; }

    /// <summary>
    /// Returns a Boolean value indicating if the current value of the TextBox is unchanged from the point in time when the TextChanged event was raised.
    /// </summary>
    /// <returns>Indicates if the current value of the TextBox is unchanged from the point in time when the TextChanged event was raised.</returns>
    public bool CheckCurrent() => true; //TODO

    /// <summary>
    /// Gets or sets a value that indicates the reason for the text changing in the FreakyAutoCompleteView.
    /// </summary>
    /// <value>The reason for the text changing in the FreakyAutoCompleteView.</value>
    public TextChangeReason Reason { get; }
}