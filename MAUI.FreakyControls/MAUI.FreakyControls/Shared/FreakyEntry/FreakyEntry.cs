using System;
using System.Windows.Input;
using MAUI.FreakyControls.Shared;
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

        public static readonly BindableProperty ImagePaddingProperty = BindableProperty.Create(
               nameof(ImagePadding),
               typeof(int),
               typeof(FreakyEntry),
               5);

        public static readonly BindableProperty ImageCommandProperty = BindableProperty.Create(
              nameof(ImagePadding),
              typeof(ICommand),
              typeof(FreakyEntry),
              default(ICommand));

        public static readonly BindableProperty ImageCommandParameterProperty = BindableProperty.Create(
              nameof(ImageCommandParameter),
              typeof(object),
              typeof(FreakyEntry),
              default(object));

        public static readonly BindableProperty AllowCopyPasteProperty = BindableProperty.Create(
              nameof(AllowCopyPaste),
              typeof(bool),
              typeof(FreakyEntry),
              true);

        public bool AllowCopyPaste
        {
            get => (bool)GetValue(AllowCopyPasteProperty);
            set => SetValue(AllowCopyPasteProperty, value);
        }

        public object ImageCommandParameter
        {
            get => GetValue(ImageCommandParameterProperty);
            set => SetValue(ImageCommandParameterProperty, value);
        }

        public ICommand ImageCommand
        {
            get => (ICommand)GetValue(ImageCommandProperty);
            set => SetValue(ImageCommandProperty, value);
        }

        public int ImagePadding
        {
            get => (int)GetValue(ImagePaddingProperty);
            set => SetValue(ImagePaddingProperty, value);
        }

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