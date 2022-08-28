using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
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
                Background = null,
                Gravity = GravityFlags.Top,
                TextAlignment = Android.Views.TextAlignment.ViewStart,
            };
            _nativeView.SetSingleLine(false);
            _nativeView.SetHorizontallyScrolling(false);
            return _nativeView;
        }

        private void HandleNativeHasUnderline(bool hasUnderline, Color underlineColor)
        {
            //if (hasUnderline)
            //{
            //    var AndroidColor = underlineColor.ToNativeColor();
            //    var colorFilter = BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(
            //        AndroidColor, BlendModeCompat.SrcIn);
            //    PlatformView.Background?.SetColorFilter(colorFilter);
            //}
            //else
            //{
            //    PlatformView.Background?.ClearColorFilter();
            //}
        }
    }
}

