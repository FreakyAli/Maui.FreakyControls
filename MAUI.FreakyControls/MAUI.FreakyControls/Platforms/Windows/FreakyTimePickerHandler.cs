namespace Maui.FreakyControls
{
    public partial class FreakyTimePickerHandler
    {
        internal Task HandleAndAlignImageSourceAsync(FreakyTimePicker entry)
        {
            // TODO: Image alignment is not yet implemented for Windows TimePicker.
            return Task.CompletedTask;
        }
    }
}
