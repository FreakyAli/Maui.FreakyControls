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
using Maui.FreakyControls.Platforms.Android.NativeControls;
using Microsoft.Maui.Handlers;
using Color = Microsoft.Maui.Graphics.Color;

namespace Maui.FreakyControls
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
            var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
            ViewCompat.SetBackgroundTintList(_nativeView, colorStateList);
            _nativeView.SetHorizontallyScrolling(false);
            return _nativeView;
        }

        internal void HandleAllowCopyPaste(FreakyEditor editor)
        {
            if (editor.AllowCopyPaste)
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
}