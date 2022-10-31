using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public class ItemDisappearingEventArgs : ItemsEventArgs
{
    public ItemDisappearingEventArgs(InteractionType type, bool isNextSelected, int index, object item) :
        base(type, isNextSelected, index, item)
    {
    }
}
