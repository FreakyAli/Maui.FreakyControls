using System;
using System.Drawing;
using Maui.FreakyControls.Platforms.iOS.NativeControls;
using UIKit;
using Size = Microsoft.Maui.Graphics.Size;

namespace Maui.FreakyControls;

public partial class AutoCompleteHandler
{
    static readonly int baseHeight = 10;

    protected override iOSAutoSuggestBox CreatePlatformView()
    {
        var iosSuggestion = new iOSAutoSuggestBox();
        //iosSuggestion.Frame = new RectangleF(0, 20, 320, 40);
        return iosSuggestion;
    }

    public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        var baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);
        var testString = new Foundation.NSString("Tj");
        var testSize = testString.GetSizeUsingAttributes(new UIStringAttributes { Font = PlatformView?.Font });
        double height = baseHeight + testSize.Height;
        height = Math.Round(height);
        return new Size(baseResult.Width, height);
    }

    protected override void ConnectHandler(iOSAutoSuggestBox platformView)
    {
        base.ConnectHandler(platformView);
        platformView.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
        platformView.TextChanged += AutoSuggestBox_TextChanged;
        platformView.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
        platformView.EditingDidBegin += Control_EditingDidBegin;
        platformView.EditingDidEnd += Control_EditingDidEnd;
    }

    protected override void DisconnectHandler(iOSAutoSuggestBox platformView)
    {
        base.DisconnectHandler(platformView);
        platformView.SuggestionChosen -= AutoSuggestBox_SuggestionChosen;
        platformView.TextChanged -= AutoSuggestBox_TextChanged;
        platformView.QuerySubmitted -= AutoSuggestBox_QuerySubmitted;
        platformView.EditingDidBegin -= Control_EditingDidBegin;
        platformView.EditingDidEnd -= Control_EditingDidEnd;
    }

    private void Control_EditingDidBegin(object sender, EventArgs e)
    {
        VirtualView?.SetValue(VisualElement.IsFocusedPropertyKey, true);
    }
    private void Control_EditingDidEnd(object sender, EventArgs e)
    {
        VirtualView?.SetValue(VisualElement.IsFocusedPropertyKey, false);
    }
}

