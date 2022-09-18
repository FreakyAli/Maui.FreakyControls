using System;
using Maui.FreakyControls.Shared;
using Maui.FreakyControls.Shared.Enums;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Color = Microsoft.Maui.Graphics.Color;

namespace Maui.FreakyControls
{
    public class FreakyEditor : Editor
    {
        public static readonly BindableProperty AllowCopyPasteProperty = BindableProperty.Create(
             nameof(AllowCopyPaste),
             typeof(bool),
             typeof(FreakyEditor),
             true);

        /// <summary>
        /// Gets and Sets if your Entry allows Copy Paste. default is true!
        /// </summary>
        public bool AllowCopyPaste
        {
            get => (bool)GetValue(AllowCopyPasteProperty);
            set => SetValue(AllowCopyPasteProperty, value);
        }
    }
}
