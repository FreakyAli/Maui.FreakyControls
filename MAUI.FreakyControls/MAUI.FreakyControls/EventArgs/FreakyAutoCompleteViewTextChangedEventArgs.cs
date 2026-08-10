#nullable disable

﻿using Maui.FreakyControls.Enums;

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
    /// Returns a value indicating whether the current text of the control is unchanged from the value at the time the <see cref="FreakyAutoCompleteView.TextChanged"/> event was raised.
    /// </summary>
    /// <returns><c>true</c> if the text is still current; otherwise <c>false</c>.</returns>
    public bool CheckCurrent() => true;

    /// <summary>
    /// Gets or sets a value that indicates the reason for the text changing in the FreakyAutoCompleteView.
    /// </summary>
    /// <value>The reason for the text changing in the FreakyAutoCompleteView.</value>
    public TextChangeReason Reason { get; }
}