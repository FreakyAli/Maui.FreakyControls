using System;
using CoreGraphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using CoreAnimation;
using static Maui.FreakyControls.Platforms.iOS.NativeExtensions;
using Microsoft.Maui;
using Maui.FreakyControls.Platforms.iOS.NativeControls;

namespace Maui.FreakyControls
{
    public partial class FreakyEditorHandler
    {
        protected override MauiTextView CreatePlatformView()
        {
            var mauiTextview = new FreakyUITextView();
            return mauiTextview;
        }

        internal void HandleAllowCopyPaste(FreakyEditor entry)
        {
            (PlatformView as FreakyUITextView).AllowCopyPaste = entry.AllowCopyPaste;
        }
    }
}