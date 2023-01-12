using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Maui.FreakyControls;

public partial class FreakyAutoCompleteViewHandler
{
    private AppCompatAutoCompleteTextView NativeControl => PlatformView as AppCompatAutoCompleteTextView;

    protected override AppCompatAutoCompleteTextView CreatePlatformView()
    {
        var autoComplete = new AppCompatAutoCompleteTextView(Context)
        {
            Text = VirtualView?.Text,
        };

        var colorStateList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
        ViewCompat.SetBackgroundTintList(autoComplete, colorStateList);
        autoComplete.SetSingleLine(true);
        autoComplete.ImeOptions = ImeAction.Done;
        if (VirtualView != null)
        {
            autoComplete.SetTextColor(VirtualView.TextColor.ToPlatform());
        }

        return autoComplete;
    }
    protected override void ConnectHandler(AppCompatAutoCompleteTextView platformView)
    {
        PlatformView.TextChanged += PlatformView_TextChanged;
        PlatformView.EditorAction += PlatformView_EditorAction;
    }

    protected override void DisconnectHandler(AppCompatAutoCompleteTextView platformView)
    {
        PlatformView.TextChanged -= PlatformView_TextChanged;
        PlatformView.EditorAction -= PlatformView_EditorAction;
    }

    private void PlatformView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
    {
        if (VirtualView.Text != PlatformView.Text)
        {
            VirtualView.Text = PlatformView.Text;
        }
    }

    private void PlatformView_EditorAction(object sender, TextView.EditorActionEventArgs e)
    {
        if (e.ActionId == Android.Views.InputMethods.ImeAction.Done)
        {
            VirtualView.TriggerCompleted();
        }
    }

    private void SetItemsSource()
    {
        if (VirtualView.ItemsSource == null) return;

        ResetAdapter();
    }

    private void ResetAdapter()
    {
        var adapter = new BoxArrayAdapter(Context,
            Android.Resource.Layout.SimpleDropDownItem1Line,
            VirtualView.ItemsSource.ToList());

        NativeControl.Adapter = adapter;

        adapter.NotifyDataSetChanged();
    }

    public static void MapText(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView view)
    {
        if (handler.NativeControl.Text != view.Text)
        {
            handler.NativeControl.Text = view.Text;
        }
    }

    public static void MapBackgroundColor(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView view)
    {
        var nativeColor = view.BackgroundColor.ToPlatform();
        handler.PlatformView.SetBackgroundColor(nativeColor);

    }

    public static void MapItemsSource(FreakyAutoCompleteViewHandler handler, FreakyAutoCompleteView view)
    {
        handler.SetItemsSource();
    }
}

internal class BoxArrayAdapter : ArrayAdapter
{
    private readonly IList<string> _objects;

    public BoxArrayAdapter(
        Context context,
        int textViewResourceId,
        List<string> objects) : base(context, textViewResourceId, objects)
    {
        _objects = objects;
    }
}

