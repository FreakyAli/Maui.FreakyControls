using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;
using Maui.FreakyControls.Platforms.iOS;

namespace Maui.FreakyControls;
public partial class FreakyAutoCompleteViewHandler 
{
    protected override NativeAutoCompleteView CreatePlatformView()
    {
        var view = new NativeAutoCompleteView
        {
            AutoCompleteViewSource = new AutoCompleteDefaultDataSource(),
            SortingAlgorithm = (d, b) => b,
            Text = VirtualView.Text,
            TextColor = VirtualView.TextColor.ToPlatform(),
            ReturnKeyType = UIReturnKeyType.Done
        };
        view.AutoCompleteViewSource.Selected += AutoCompleteViewSourceOnSelected;
        return view;
    }

    public override void PlatformArrange(Rect rect)
    {
        base.PlatformArrange(rect);
        Draw(rect);
    }

    protected override void ConnectHandler(NativeAutoCompleteView platformView)
    {
        PlatformView.EditingChanged += PlatformView_TextChanged;
        PlatformView.EditingDidBegin += PlatformView_EditingDidBegin;
        PlatformView.EditingDidEndOnExit += PlatformView_EditingDidEndOnExit;
        PlatformView.EditingDidEnd += PlatformView_EditingDidEnd;
    }

    protected override void DisconnectHandler(NativeAutoCompleteView platformView)
    {
        PlatformView.EditingChanged -= PlatformView_TextChanged;
        PlatformView.EditingDidBegin -= PlatformView_EditingDidBegin;
        PlatformView.EditingDidEndOnExit -= PlatformView_EditingDidEndOnExit;
        PlatformView.EditingDidEnd -= PlatformView_EditingDidEnd;
    }

    private void PlatformView_TextChanged(object sender, EventArgs e)
    {
        if (VirtualView.Text != PlatformView.Text)
        {
            VirtualView.Text = PlatformView.Text;
        }
    }

    private void PlatformView_EditingDidBegin(object sender, EventArgs e)
    {
        //VirtualView.IsFocused = true;
    }

    private void PlatformView_EditingDidEndOnExit(object sender, EventArgs e)
    {
        VirtualView.TriggerCompleted();
    }

    private void PlatformView_EditingDidEnd(object sender, EventArgs e)
    {
        //VirtualView.IsFocused = false;
    }

    public static void MapText(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView view)
    {
        if (handler.PlatformView.Text != view.Text)
        {
            handler.PlatformView.Text = view.Text;
        }
    }

    public static void MapItemsSource(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView view)
    {
        handler.SetItemsSource();
    }

    private void SetItemsSource()
    {
        if (VirtualView.ItemsSource != null)
        {
            var items = VirtualView.ItemsSource.ToList();
            PlatformView.UpdateItems(items);
        }
    }

    public void Draw(CGRect rect)
    {
        var ctrl = UIApplication.SharedApplication.GetTopViewController();

        var relativePosition = UIApplication.SharedApplication.KeyWindow;
        var relativeFrame = PlatformView.Superview.ConvertRectToView(PlatformView.Frame, relativePosition);

        PlatformView.Draw(ctrl, PlatformView.Layer, VirtualView as FreakyAutoCompleteView, relativeFrame.X, relativeFrame.Y);
    }

    private void AutoCompleteViewSourceOnSelected(object sender, SelectedItemChangedEventArgs args)
    {
        var selectedItemText = args.SelectedItem?.ToString();

        if (VirtualView.SelectedText != selectedItemText)
        {
            VirtualView.SelectedText = selectedItemText;
        }
    }
}