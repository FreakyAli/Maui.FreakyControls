using System;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public class ItemAppearedEventArgs : ItemsEventArgs
{
    public ItemAppearedEventArgs(InteractionType type, bool isNextSelected, int index, object item) :
        base(type, isNextSelected, index, item)
    {
    }
}
