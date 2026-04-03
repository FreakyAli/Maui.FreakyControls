namespace Maui.FreakyControls
{
    public partial class FreakyPickerHandler
    {
        internal Task HandleAndAlignImageSourceAsync(FreakyPicker entry)
        {
            // Image alignment inside ComboBox is not natively supported on Windows.
            return Task.CompletedTask;
        }
    }
}
