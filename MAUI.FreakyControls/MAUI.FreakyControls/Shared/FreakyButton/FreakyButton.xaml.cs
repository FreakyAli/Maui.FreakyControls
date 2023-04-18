using Maui.FreakyControls.Extensions;

namespace Maui.FreakyControls;

public partial class FreakyButton : ContentView
{
    public FreakyButton()
    {
        InitializeComponent();
    }

    async void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        #region You will not need this block, it is just to choose a random color for change to
        var colors = new[] { Colors.Red, Colors.Pink, Colors.Silver, Colors.Yellow, Colors.Black, Colors.Green };
        var rnd = new Random();

        var actualColor = button.BackgroundColor;
        var randomColor = colors.Where(c => c != actualColor).ToArray()[rnd.Next(0, colors.Length - 2)];
        #endregion

        // Here is the effective use of the smooth background color change animation
        await button.ChangeBackgroundColorTo(randomColor, 150, Easing.CubicOut);
        await button.ChangeBackgroundColorTo(actualColor, 100, Easing.SinOut);
    }
}
