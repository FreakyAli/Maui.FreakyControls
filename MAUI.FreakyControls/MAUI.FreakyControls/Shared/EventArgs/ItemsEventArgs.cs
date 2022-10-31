using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public abstract class ItemsEventArgs : FreakyEventArgs
{
    public ItemsEventArgs(InteractionType type, bool isNextSelected, int index, object item)
    {
        Type = type;
        IsNextSelected = isNextSelected;
        Index = index;
        Item = item;
    }

    public InteractionType Type { get; }
    public bool IsNextSelected { get; }
    public int Index { get; }
    public object Item { get; }
}
