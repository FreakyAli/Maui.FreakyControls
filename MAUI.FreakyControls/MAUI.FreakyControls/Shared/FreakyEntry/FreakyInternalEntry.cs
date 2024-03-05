namespace Maui.FreakyControls
{
    internal class FreakyInternalEntry : Entry
    {
        public static readonly BindableProperty AllowCopyPasteProperty = BindableProperty.Create(
              nameof(AllowCopyPaste),
              typeof(bool),
              typeof(FreakyEntry),
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