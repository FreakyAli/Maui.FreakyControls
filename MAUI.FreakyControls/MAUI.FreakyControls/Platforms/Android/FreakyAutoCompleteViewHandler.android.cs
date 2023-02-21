using Android.Content;
using Android.Graphics.Drawables;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;

namespace Maui.FreakyControls;


public partial class FreakyAutoCompleteViewHandler : ViewHandler<FreakyAutoCompleteView, AppCompatAutoCompleteTextView>
{
    private AppCompatAutoCompleteTextView NativeControl => PlatformView as AppCompatAutoCompleteTextView;

    protected override AppCompatAutoCompleteTextView CreatePlatformView()
    {
        var autoComplete = new AppCompatAutoCompleteTextView(Context)
        {
            Text = VirtualView?.Text,
        };

        GradientDrawable gd = new GradientDrawable();
        gd.SetColor(global::Android.Graphics.Color.Transparent);
        autoComplete.SetBackground(gd);
        autoComplete.SetSingleLine(true);
        autoComplete.ImeOptions = ImeAction.Done;
        if (VirtualView != null)
        {
            autoComplete.SetTextColor(VirtualView.TextColor.ToAndroid());
        }

        return autoComplete;
    }
    protected override void ConnectHandler(AppCompatAutoCompleteTextView platformView)
    {
        PlatformView.TextChanged += PlatformView_TextChanged;
        PlatformView.EditorAction += PlatformView_EditorAction;
        PlatformView.ItemClick += PlatformView_ItemClicked;
    }

    protected override void DisconnectHandler(AppCompatAutoCompleteTextView platformView)
    {
        PlatformView.TextChanged -= PlatformView_TextChanged;
        PlatformView.EditorAction -= PlatformView_EditorAction;
        PlatformView.ItemClick -= PlatformView_ItemClicked;
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

    private void PlatformView_ItemClicked(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
    {
        if (VirtualView.SelectedText != PlatformView.Text)
        {
            VirtualView.SelectedText = PlatformView.Text;
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
