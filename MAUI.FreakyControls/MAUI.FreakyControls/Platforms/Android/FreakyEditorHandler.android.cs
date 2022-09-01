using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Content;
using AndroidX.Core.Content.Resources;
using AndroidX.Core.Graphics;
using AndroidX.Core.View;
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
            ColorStateList colorStateList;
            var AndroidColor = underlineColor.ToNativeColor();
            colorStateList = ColorStateList.ValueOf(hasUnderline ? AndroidColor : Android.Graphics.Color.Transparent);
            ViewCompat.SetBackgroundTintList(PlatformView, colorStateList);
            //colorFilter = hasUnderline ? ContextCompat.GetColor(this, AndroidColor, PorterDuff.Mode.SrcAtop);
            //BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(
            //   AndroidColor, BlendModeCompat.SrcAtop) :
            //BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(
            //   Android.Graphics.Color.Transparent, BlendModeCompat.SrcAtop);
            //PlatformView.Background?.SetColorFilter(colorFilter);
        }
    }
}


