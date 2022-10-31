using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public class UserInteractedEventArgs : FreakyEventArgs
{
    public UserInteractedEventArgs(UserInteractionStatus status, double diff, int index, object item)
    {
        Status = status;
        Diff = diff;
        Index = index;
        Item = item;
    }

    public UserInteractionStatus Status { get; }
    public double Diff { get; }
    public int Index { get; }
    public object Item { get; }
}