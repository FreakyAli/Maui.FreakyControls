namespace Maui.FreakyControls
{
    public partial class FreakyDatePickerHandler
    {
        internal Task HandleAndAlignImageSourceAsync(FreakyDatePicker entry)
        {
            // TODO: Image alignment is not yet implemented for Windows DatePicker.
            return Task.CompletedTask;
        }
    }
}
