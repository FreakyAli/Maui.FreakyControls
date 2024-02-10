using Maui.FreakyControls.Extensions;
using Microsoft.Maui.Layouts;
using System.Windows.Input;

namespace Maui.FreakyControls;

public class FreakySwipeButton : AbsoluteLayout
{
    private readonly PanGestureRecognizer panGesture;
    private readonly View gestureListener;
    private const double _fadeEffect = 0.5;
    private const uint _animLength = 50;

    public event EventHandler SlideCompleted;

    public static readonly BindableProperty SlideCompleteCommandProperty = BindableProperty.Create(
           nameof(SlideCompleteCommand),
           typeof(ICommand),
           typeof(FreakySwipeButton),
           defaultValue: default(ICommand));

    public ICommand SlideCompleteCommand
    {
        get => (ICommand)GetValue(SlideCompleteCommandProperty);
        set => SetValue(SlideCompleteCommandProperty, value);
    }

    public static readonly BindableProperty ThumbProperty = BindableProperty.Create(
            nameof(Thumb),
            typeof(View),
            typeof(FreakySwipeButton),
            defaultValue: default(View));

    public View Thumb
    {
        get => (View)GetValue(ThumbProperty);
        set => SetValue(ThumbProperty, value);
    }

    public static readonly BindableProperty TrackBarProperty = BindableProperty.Create(
            nameof(TrackBar),
            typeof(View),
            typeof(FreakySwipeButton),
            defaultValue: default(View));

    public View TrackBar
    {
        get => (View)GetValue(TrackBarProperty);
        set => SetValue(TrackBarProperty, value);
    }

    public static readonly BindableProperty FillBarProperty = BindableProperty.Create(
            nameof(FillBar),
            typeof(View),
            typeof(FreakySwipeButton),
            defaultValue: default(View));

    public View FillBar
    {
        get => (View)GetValue(FillBarProperty);
        set => SetValue(FillBarProperty, value);
    }

    public FreakySwipeButton()
    {
        panGesture = new();
        panGesture.PanUpdated += OnPanGestureUpdated;
        SizeChanged += OnSizeChanged;

        gestureListener = new ContentView
        {
            BackgroundColor = Colors.White,
            Opacity = 0.05
        };
        gestureListener.GestureRecognizers.Add(panGesture);
    }

    private async void OnPanGestureUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (Thumb == null || TrackBar == null || FillBar == null)
            return;

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                await TrackBar.FadeTo(_fadeEffect, _animLength);
                break;

            case GestureStatus.Running:
                // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                var x = Math.Max(0, e.TotalX);
                if (x > (Width - Thumb.Width))
                    x = (Width - Thumb.Width);

                //Uncomment this if you want only forward dragging.
                //if (e.TotalX < Thumb.TranslationX)
                //    return;
                Thumb.TranslationX = x;
                this.SetLayoutBounds(FillBar, new Rect(0, 0, x + Thumb.Width / 2, this.Height));
                break;

            case GestureStatus.Completed:
                var posX = Thumb.TranslationX;
                this.SetLayoutBounds(FillBar, new Rect(0, 0, 0, this.Height));

                // Reset translation applied during the pan
                await TaskExt.WhenAll(
                    TrackBar.FadeTo(1, _animLength),
                    Thumb.TranslateTo(0, 0, _animLength * 2, Easing.CubicIn)
                );

                if (posX >= (Width - Thumb.Width - 10/* keep some margin for error*/))
                {
                    SlideCompleteCommand.ExecuteCommandIfAvailable();
                    SlideCompleted?.Invoke(this, EventArgs.Empty);
                }
                break;
        }
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        if (Thumb == null || TrackBar == null || FillBar == null)
            return;

        Children.Clear();

        this.SetLayoutFlags(TrackBar, AbsoluteLayoutFlags.SizeProportional);
        this.SetLayoutBounds(TrackBar, new Rect(0, 0, 1, 1));
        Children.Add(TrackBar);

        this.SetLayoutFlags(FillBar, AbsoluteLayoutFlags.None);
        this.SetLayoutBounds(FillBar, new Rect(0, 0, 0, this.Height));
        Children.Add(FillBar);

        this.SetLayoutFlags(Thumb, AbsoluteLayoutFlags.None);
        this.SetLayoutBounds(Thumb, new Rect(0, 0, this.Width / 5, this.Height));
        Children.Add(Thumb);

        this.SetLayoutFlags(gestureListener, AbsoluteLayoutFlags.SizeProportional);
        this.SetLayoutBounds(gestureListener, new Rect(0, 0, 1, 1));
        Children.Add(gestureListener);
    }
}