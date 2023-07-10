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
        /// Gets and Sets if your Controls allows Copy Paste. default is true!
        /// </summary>
        public bool AllowCopyPaste
        {
            get => (bool)GetValue(AllowCopyPasteProperty);
            set => SetValue(AllowCopyPasteProperty, value);
        }
    }
}