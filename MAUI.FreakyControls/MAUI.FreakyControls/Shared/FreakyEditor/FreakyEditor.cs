using System;
using MAUI.FreakyControls.Shared;
using MAUI.FreakyControls.Shared.Enums;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Color = Microsoft.Maui.Graphics.Color;

namespace MAUI.FreakyControls
{
    public class FreakyEditor : Editor
    {
        public static readonly BindableProperty AllowCopyPasteProperty = BindableProperty.Create(
             nameof(AllowCopyPaste),
             typeof(bool),
             typeof(FreakyEditor),
             true);

        public bool AllowCopyPaste
        {
            get => (bool)GetValue(AllowCopyPasteProperty);
            set => SetValue(AllowCopyPasteProperty, value);
        }
    }
}
