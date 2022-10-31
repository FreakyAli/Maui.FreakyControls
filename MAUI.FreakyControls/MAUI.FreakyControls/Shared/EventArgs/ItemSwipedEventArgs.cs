using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public class ItemSwipedEventArgs : FreakyEventArgs
{
    public ItemSwipedEventArgs(ItemSwipeDirection direction, int index, object item)
    {
        Direction = direction;
        Index = index;
        Item = item;
    }

    public ItemSwipeDirection Direction { get; }
    public object Item { get; }
    public int Index { get; }
}
