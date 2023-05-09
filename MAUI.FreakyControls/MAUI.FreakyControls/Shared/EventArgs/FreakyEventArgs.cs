using System;

//Make sure .EventArgs is never created as a namespace.
namespace Maui.FreakyControls;

/// <summary>
/// EventArgs for your freaky custom controls
/// </summary>
public class FreakyEventArgs : EventArgs
{
    /// <summary>
    /// Generic data you receive from a freaky event
    /// </summary>
    public object Data { get; set; }
}