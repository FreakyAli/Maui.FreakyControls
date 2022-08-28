using System;
using Microsoft.Maui.Handlers;

namespace MAUI.FreakyControls
{
    public class FreakyEditor : Editor, IFreakyEditor
    {
        public readonly BindableProperty HasUnderlineProperty = BindableProperty.Create(
            nameof(HasUnderline),
            typeof(bool),
            typeof(FreakyEditor),
            true);

        public bool HasUnderline
        {
            get => (bool)GetValue(HasUnderlineProperty);
            set => SetValue(HasUnderlineProperty, value);
        }

        public readonly BindableProperty UnderlineColorProperty = BindableProperty.Create(
           nameof(UnderlineColor),
           typeof(Color),
           typeof(FreakyEditor),
           Colors.Black);

        public Color UnderlineColor
        {
            get => (Color)GetValue(HasUnderlineProperty);
            set => SetValue(HasUnderlineProperty, value);
        }
    }
}
