using System;
using MAUI.FreakyControls;

namespace Samples
{
	public class ExtendedLabel : Label, IExtendedLabel
	{
        public readonly BindableProperty HasUnderlineProperty = BindableProperty.Create(
            nameof(HasUnderline),
            typeof(bool),
            typeof(ExtendedLabel),
            true);

        public bool HasUnderline
        {
            get => (bool)GetValue(HasUnderlineProperty);
            set => SetValue(HasUnderlineProperty, value);
        }

        public readonly BindableProperty UnderlineColorProperty = BindableProperty.Create(
           nameof(UnderlineColor),
           typeof(Color),
           typeof(ExtendedLabel),
           Colors.Black);

        public Color UnderlineColor
        {
            get => (Color)GetValue(HasUnderlineProperty);
            set => SetValue(HasUnderlineProperty, value);
        }
    }
}

