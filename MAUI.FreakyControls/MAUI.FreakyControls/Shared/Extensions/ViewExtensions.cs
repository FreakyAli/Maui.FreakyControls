using System;
namespace Maui.FreakyControls.Extensions;

public static class ViewExtensions
{
    public static void DismissSoftKeyboard(this Entry entry)
    {
        entry.IsEnabled = false;
        entry.IsEnabled = true;
    }
}

