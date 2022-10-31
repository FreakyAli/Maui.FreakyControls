using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public class ItemAppearingEventArgs : ItemsEventArgs
{
    public ItemAppearingEventArgs(InteractionType type, bool isNextSelected, int index, object item) :
        base(type, isNextSelected, index, item)
    {
    }
}
