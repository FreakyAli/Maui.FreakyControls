//Make sure .EventArgs is never created
namespace Maui.FreakyControls;

public class FreakyCodeCompletedEventArgs : EventArgs
{
    public string Code { get; set; }

    public FreakyCodeCompletedEventArgs(string code)
    {
        this.Code = code;
    }
}