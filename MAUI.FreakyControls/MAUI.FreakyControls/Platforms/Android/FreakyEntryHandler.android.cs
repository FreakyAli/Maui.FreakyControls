using System;
using Android.Content.Res;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using Microsoft.Maui.Handlers;

namespace MAUI.FreakyControls
{
    public partial class FreakyEntryHandler
    {
        protected override AppCompatEditText CreatePlatformView()
        {
            var _nativeView = new AppCompatEditText(Context);
            var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            ViewCompat.SetBackgroundTintList(_nativeView, colorStateList);
            return _nativeView;
        }
    }
}

