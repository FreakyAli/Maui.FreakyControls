using System;
using MAUI.FreakyControls.Shared.Enums;

namespace MAUI.FreakyControls
{
    public class FreakyEntry : Entry
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
               nameof(Image),
               typeof(ImageSource),
               typeof(FreakyEntry),
               default(ImageSource));

        public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
               nameof(ImageHeight),
               typeof(int),
               typeof(FreakyEntry),
               25);

        public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
               nameof(ImageWidth),
               typeof(int),
               typeof(FreakyEntry),
               25);

        public static readonly BindableProperty ImageAlignmentProperty = BindableProperty.Create(
               nameof(ImageAlignment),
               typeof(ImageAlignment),
               typeof(FreakyEntry),
               ImageAlignment.Right);

        public int ImageWidth
        {
            get => (int)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        public int ImageHeight
        {
            get => (int)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public ImageAlignment ImageAlignment
        {
            get => (ImageAlignment)GetValue(ImageAlignmentProperty);
            set => SetValue(ImageAlignmentProperty, value);
        }
    }
}