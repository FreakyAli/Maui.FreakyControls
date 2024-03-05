using Android.Content.Res;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using Maui.FreakyControls.Platforms.Android.NativeControls;

namespace Maui.FreakyControls;

internal partial class FreakyInternalEntryHandler
{
    protected override AppCompatEditText CreatePlatformView()
    {
        var _nativeView = new AppCompatEditText(Context);
        var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        ViewCompat.SetBackgroundTintList(_nativeView, colorStateList);
        return _nativeView;
    }

    internal void HandleAllowCopyPaste(FreakyEntry entry)
    {
        if (entry.AllowCopyPaste)
        {
            PlatformView.CustomInsertionActionModeCallback = null;
            PlatformView.CustomSelectionActionModeCallback = null;
        }
        else
        {
            PlatformView.CustomInsertionActionModeCallback = new CustomInsertionActionModeCallback();
            PlatformView.CustomSelectionActionModeCallback = new CustomSelectionActionModeCallback();
        }
    }
}