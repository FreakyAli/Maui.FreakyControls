using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content.Resources;
using AndroidX.Core.Graphics;
using Microsoft.Maui.Handlers;
using Color = Microsoft.Maui.Graphics.Color;

namespace MAUI.FreakyControls
{
    public partial class FreakyEditorHandler
    {
        protected override AppCompatEditText CreatePlatformView()
        {
            var _nativeView = new AppCompatEditText(Context)
            {
                ImeOptions = ImeAction.Done,
                Gravity = GravityFlags.Top,
                TextAlignment = Android.Views.TextAlignment.ViewStart,
            };
            _nativeView.SetSingleLine(false);
            _nativeView.SetHorizontallyScrolling(false);
            return _nativeView;
        }


        private void HandleNativeHasUnderline(bool hasUnderline, Color underlineColor)
        {
            ColorFilter colorFilter;
            var AndroidColor = underlineColor.ToNativeColor();
            colorFilter = hasUnderline ?
            BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(
               AndroidColor, BlendModeCompat.SrcIn) :
            BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(
               Android.Graphics.Color.Transparent, BlendModeCompat.SrcIn);
            PlatformView.Background?.SetColorFilter(colorFilter);
        }
    }
}

