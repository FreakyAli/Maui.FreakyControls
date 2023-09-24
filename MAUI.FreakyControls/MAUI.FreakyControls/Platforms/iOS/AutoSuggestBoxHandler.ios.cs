﻿using Maui.FreakyControls.Platforms.iOS.NativeControls;
using Microsoft.Maui.Handlers;
using System.Drawing;
using UIKit;

namespace Maui.FreakyControls;

public partial class FreakyAutoCompleteViewHandler : ViewHandler<IFreakyAutoCompleteView, FreakyAutoCompleteViewView>
{
    /// <inheritdoc />
    protected override FreakyAutoCompleteViewView CreatePlatformView() => new FreakyAutoCompleteViewView();

    protected override void ConnectHandler(FreakyAutoCompleteViewView platformView)
    {
        base.ConnectHandler(platformView);

        PlatformView.Text = VirtualView.Text ?? string.Empty;
        PlatformView.Frame = new RectangleF(0, 20, 320, 50);

        UpdateTextColor(platformView);
        UpdatePlaceholderText(platformView);
        UpdatePlaceholderTextColor(platformView);
        UpdateDisplayMemberPath(platformView);
        UpdateIsEnabled(platformView);
        platformView.UpdateTextOnSelect = VirtualView.UpdateTextOnSelect;
        platformView.IsSuggestionListOpen = VirtualView.IsSuggestionListOpen;

        UpdateItemsSource(platformView);

        platformView.SuggestionChosen += OnPlatformViewSuggestionChosen;
        platformView.TextChanged += OnPlatformViewTextChanged;
        platformView.QuerySubmitted += OnPlatformViewQuerySubmitted;

        PlatformView.EditingDidBegin += Control_EditingDidBegin;
        PlatformView.EditingDidEnd += Control_EditingDidEnd;
    }
    protected override void DisconnectHandler(FreakyAutoCompleteViewView platformView)
    {
        platformView.SuggestionChosen -= OnPlatformViewSuggestionChosen;
        platformView.TextChanged -= OnPlatformViewTextChanged;
        platformView.QuerySubmitted -= OnPlatformViewQuerySubmitted;
        PlatformView.EditingDidBegin -= Control_EditingDidBegin;
        PlatformView.EditingDidEnd -= Control_EditingDidEnd;

        platformView.Dispose();
        base.DisconnectHandler(platformView);
    }

    private void OnPlatformViewSuggestionChosen(object? sender, FreakyAutoCompleteViewSuggestionChosenEventArgs e)
    {
        VirtualView?.RaiseSuggestionChosen(e);
    }
    private void OnPlatformViewTextChanged(object? sender, FreakyAutoCompleteViewTextChangedEventArgs e)
    {
        VirtualView?.NativeControlTextChanged(e);
    }
    private void OnPlatformViewQuerySubmitted(object? sender, FreakyAutoCompleteViewQuerySubmittedEventArgs e)
    {
        VirtualView?.RaiseQuerySubmitted(e);
    }

    static readonly int baseHeight = 20;
    /// <inheritdoc />
    public override Microsoft.Maui.Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        var baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);
        var testString = new Foundation.NSString("Tj");
        var testSize = testString.GetSizeUsingAttributes(new UIStringAttributes { Font = PlatformView.Font });
        double height = baseHeight + testSize.Height;
        height = Math.Round(height);
        if (double.IsInfinity(widthConstraint) || double.IsInfinity(heightConstraint))
        {
            // If we drop an infinite value into base.GetDesiredSize for the Editor, we'll
            // get an exception; it doesn't know what do to with it. So instead we'll size
            // it to fit its current contents and use those values to replace infinite constraints

            PlatformView.SizeToFit();
            var sz = new Microsoft.Maui.Graphics.Size(PlatformView.Frame.Width, PlatformView.Frame.Height);
            return sz;
        }

        return base.GetDesiredSize(baseResult.Width, height);
    }

    void Control_EditingDidBegin(object sender, EventArgs e)
    {
        VirtualView.IsFocused = true;
    }
    void Control_EditingDidEnd(object sender, EventArgs e)
    {
        VirtualView.IsFocused = false;
    }

    public static void MapText(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        if (handler.PlatformView.Text != view.Text)
            handler.PlatformView.Text = view.Text;
    }

    public static void MapTextColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.SetTextColor(view.TextColor);
    }
    public static void MapPlaceholderText(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.PlaceholderText = view.PlaceholderText;
    }
    public static void MapPlaceholderTextColor(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.SetPlaceholderTextColor(view.PlaceholderTextColor);
    }
    public static void MapTextMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView?.SetItems(view.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath), (o) => FormatType(o, view.TextMemberPath));
    }
    public static void MapDisplayMemberPath(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view.DisplayMemberPath), (o) => FormatType(o, view.TextMemberPath));
    }
    public static void MapIsSuggestionListOpen(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.IsSuggestionListOpen = view.IsSuggestionListOpen;
    }
    public static void MapUpdateTextOnSelect(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.UpdateTextOnSelect = view.UpdateTextOnSelect;
    }
    public static void MapIsEnabled(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.UserInteractionEnabled = view.IsEnabled;
    }
    public static void MapItemsSource(FreakyAutoCompleteViewHandler handler, IFreakyAutoCompleteView view)
    {
        handler.PlatformView.SetItems(view?.ItemsSource?.OfType<object>(), (o) => FormatType(o, view?.DisplayMemberPath), (o) => FormatType(o, view?.TextMemberPath));
    }

    private void UpdateTextColor(FreakyAutoCompleteViewView platformView)
    {
        platformView.SetTextColor(VirtualView?.TextColor);
    }
    private void UpdateDisplayMemberPath(FreakyAutoCompleteViewView platformView)
    {
        platformView.SetItems(VirtualView.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView.DisplayMemberPath), (o) => FormatType(o, VirtualView.TextMemberPath));
    }
    private void UpdatePlaceholderTextColor(FreakyAutoCompleteViewView platformView)
    {
        platformView.SetPlaceholderTextColor(VirtualView?.PlaceholderTextColor);
    }
    private void UpdatePlaceholderText(FreakyAutoCompleteViewView platformView) => platformView.PlaceholderText = VirtualView?.PlaceholderText;

    private void UpdateIsEnabled(FreakyAutoCompleteViewView platformView)
    {
        platformView.UserInteractionEnabled = (bool)(VirtualView.IsEnabled);
    }

    private void UpdateItemsSource(FreakyAutoCompleteViewView platformView)
    {
        platformView.SetItems(VirtualView?.ItemsSource?.OfType<object>(), (o) => FormatType(o, VirtualView?.DisplayMemberPath), (o) => FormatType(o, VirtualView?.TextMemberPath));
    }
    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }
}