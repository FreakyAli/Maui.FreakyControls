using System;
using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Utility;
using Microsoft.Maui.Layouts;
using System.ComponentModel;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Collections.Specialized;
using Maui.FreakyControls.Shared.Behavior;
using System.Collections;
using static System.Math;
using Maui.FreakyControls.Shared.Enums;
using static Maui.FreakyControls.Utility.ResourcesInfo;
using static Maui.FreakyControls.Utility.DefaultIndicatorItemStyles;

namespace Maui.FreakyControls;

public abstract class BaseSKCanvas : SKCanvasView
{
    protected BaseSKCanvas()
    {
        BackgroundColor = Colors.Transparent;
    }

    protected sealed override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        //TODO: Figure out why drawing on coachmark is leading to surface being null
        if (e.Surface == null)
        {
            return;
        }

        e.Surface.Canvas.Clear(SKColors.Transparent);

        // make sure no previous transforms still apply
        e.Surface.Canvas.ResetMatrix();

        base.OnPaintSurface(e);

        DoPaintSurface(e);
    }

    protected abstract void DoPaintSurface(SKPaintSurfaceEventArgs skPaintSurfaceEventArgs);

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName != nameof(IsVisible))
        {
            return;
        }

        InvalidateSurface();
    }
}

public class IndicatorItemView : CircleFrame
{
    public IndicatorItemView()
    {
        Size = 10;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public new static void Preserve()
    {
    }
}

public class LeftArrowControl : ArrowControl
{
    public LeftArrowControl()
    {
        IsRight = false;
        AbsoluteLayout.SetLayoutBounds(this, new Rect(0, .5, -1, -1));
    }

    protected override ImageSource DefaultImageSource => WhiteLeftArrowImageSource;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public new static void Preserve()
    {
    }
}

public class IndicatorsControl : StackLayout
{
    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(IndicatorsControl), 0, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsIndicatorsControl().ResetIndicatorsStyles();
    });

    public static readonly BindableProperty ItemsCountProperty = BindableProperty.Create(nameof(ItemsCount), typeof(int), typeof(IndicatorsControl), -1);

    public static readonly BindableProperty SelectedIndicatorStyleProperty = BindableProperty.Create(nameof(SelectedIndicatorStyle), typeof(Style), typeof(IndicatorsControl), DefaultSelectedIndicatorItemStyle, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsIndicatorsControl().ResetIndicatorsStyles();
    });

    public static readonly BindableProperty UnselectedIndicatorStyleProperty = BindableProperty.Create(nameof(UnselectedIndicatorStyle), typeof(Style), typeof(IndicatorsControl), DefaultUnselectedIndicatorItemStyle, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsIndicatorsControl().ResetIndicatorsStyles();
    });

    public static readonly BindableProperty IsUserInteractionRunningProperty = BindableProperty.Create(nameof(IsUserInteractionRunning), typeof(bool), typeof(IndicatorsControl), true, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsIndicatorsControl().ResetVisibility();
    });

    public static readonly BindableProperty IsAutoInteractionRunningProperty = BindableProperty.Create(nameof(IsAutoInteractionRunning), typeof(bool), typeof(IndicatorsControl), true, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsIndicatorsControl().ResetVisibility();
    });

    public static readonly BindableProperty HidesForSingleIndicatorProperty = BindableProperty.Create(nameof(HidesForSingleIndicator), typeof(bool), typeof(IndicatorsControl), true, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsIndicatorsControl().ResetVisibility();
    });

    public static readonly BindableProperty MaximumVisibleIndicatorsCountProperty = BindableProperty.Create(nameof(MaximumVisibleIndicatorsCount), typeof(int), typeof(IndicatorsControl), int.MaxValue, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsIndicatorsControl().ResetVisibility();
    });

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(IndicatorsControl), null, propertyChanged: (bindable, oldValue, _) =>
    {
        bindable.AsIndicatorsControl().ResetItemsSource(oldValue as IEnumerable);
    });

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(IndicatorsControl), new DataTemplate(typeof(IndicatorItemView)));

    public static readonly BindableProperty UseParentAsBindingContextProperty = BindableProperty.Create(nameof(UseParentAsBindingContext), typeof(bool), typeof(IndicatorsControl), true);

    public static readonly BindableProperty IsTapToNavigateEnabledProperty = BindableProperty.Create(nameof(IsTapToNavigateEnabled), typeof(bool), typeof(IndicatorsControl), true);

    public static readonly BindableProperty ToFadeDurationProperty = BindableProperty.Create(nameof(ToFadeDuration), typeof(int), typeof(IndicatorsControl), 0);

    static IndicatorsControl()
    {
    }

    private CancellationTokenSource _fadeAnimationTokenSource;

    public IndicatorsControl()
    {
        Spacing = 5;
        Orientation = StackOrientation.Horizontal;

        this.SetBinding(SelectedIndexProperty, nameof(CardsView.SelectedIndex));
        this.SetBinding(ItemsSourceProperty, nameof(CardsView.ItemsSource));
        this.SetBinding(IsUserInteractionRunningProperty, nameof(CardsView.IsUserInteractionRunning));
        this.SetBinding(IsAutoInteractionRunningProperty, nameof(CardsView.IsAutoInteractionRunning));

        Margin = new Thickness(10, 20);
        AbsoluteLayout.SetLayoutBounds(this, new Rect(.5, 1, -1, -1));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);

        Behaviors.Add(new ProtectedControlBehavior());
    }

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public int ItemsCount
    {
        get => (int)GetValue(ItemsCountProperty);
        set => SetValue(ItemsCountProperty, value);
    }

    public Style SelectedIndicatorStyle
    {
        get => GetValue(SelectedIndicatorStyleProperty) as Style;
        set => SetValue(SelectedIndicatorStyleProperty, value);
    }

    public Style UnselectedIndicatorStyle
    {
        get => GetValue(UnselectedIndicatorStyleProperty) as Style;
        set => SetValue(UnselectedIndicatorStyleProperty, value);
    }

    public bool IsUserInteractionRunning
    {
        get => (bool)GetValue(IsUserInteractionRunningProperty);
        set => SetValue(IsUserInteractionRunningProperty, value);
    }

    public bool IsAutoInteractionRunning
    {
        get => (bool)GetValue(IsAutoInteractionRunningProperty);
        set => SetValue(IsAutoInteractionRunningProperty, value);
    }

    public bool HidesForSingleIndicator
    {
        get => (bool)GetValue(HidesForSingleIndicatorProperty);
        set => SetValue(HidesForSingleIndicatorProperty, value);
    }

    public int MaximumVisibleIndicatorsCount
    {
        get => (int)GetValue(MaximumVisibleIndicatorsCountProperty);
        set => SetValue(MaximumVisibleIndicatorsCountProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => GetValue(ItemsSourceProperty) as IEnumerable;
        set => SetValue(ItemsSourceProperty, value);
    }

    public DataTemplate ItemTemplate
    {
        get => GetValue(ItemTemplateProperty) as DataTemplate;
        set => SetValue(ItemTemplateProperty, value);
    }

    public bool UseParentAsBindingContext
    {
        get => (bool)GetValue(UseParentAsBindingContextProperty);
        set => SetValue(UseParentAsBindingContextProperty, value);
    }

    public bool IsTapToNavigateEnabled
    {
        get => (bool)GetValue(IsTapToNavigateEnabledProperty);
        set => SetValue(IsTapToNavigateEnabledProperty, value);
    }

    public int ToFadeDuration
    {
        get => (int)GetValue(ToFadeDurationProperty);
        set => SetValue(ToFadeDurationProperty, value);
    }

    public new object this[int index] => ItemsSource?.FindValue(index);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Preserve()
    {
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        if (UseParentAsBindingContext && Parent is CardsView)
        {
            BindingContext = Parent;
        }
    }

    protected virtual void ApplySelectedStyle(View view, int index)
        => view.Style = SelectedIndicatorStyle;

    protected virtual void ApplyUnselectedStyle(View view, int index)
        => view.Style = UnselectedIndicatorStyle;

    protected virtual int IndexOf(View view) => Children.IndexOf(view);

    protected virtual void OnResetIndicatorsStyles(int currentIndex)
    {
        foreach (var child in Children.OfType<View>())
        {
            ApplyStyle(child, currentIndex);
        }
    }

    protected virtual async void ResetVisibility(uint? appearingTime = null, Easing appearingEasing = null, uint? dissappearingTime = null, Easing disappearingEasing = null)
    {
        _fadeAnimationTokenSource?.Cancel();

        if (ItemsCount > MaximumVisibleIndicatorsCount ||
            (HidesForSingleIndicator && ItemsCount <= 1 && ItemsCount >= 0))
        {
            Opacity = 0;
            IsVisible = false;
            return;
        }

        if (ToFadeDuration <= 0)
        {
            Opacity = 1;
            IsVisible = true;
            return;
        }

        if (IsUserInteractionRunning || IsAutoInteractionRunning)
        {
            IsVisible = true;

            await new AnimationWrapper(v => Opacity = v, Opacity)
                .Commit(this, nameof(ResetVisibility), 16, appearingTime ?? 330, appearingEasing ?? Easing.CubicInOut);
            return;
        }

        _fadeAnimationTokenSource = new CancellationTokenSource();
        var token = _fadeAnimationTokenSource.Token;

        await Task.Delay(ToFadeDuration);
        if (token.IsCancellationRequested)
        {
            return;
        }

        await new AnimationWrapper(v => Opacity = v, Opacity, 0)
            .Commit(this, nameof(ResetVisibility), 16, dissappearingTime ?? 330, disappearingEasing ?? Easing.SinOut);

        if (token.IsCancellationRequested)
        {
            return;
        }
        IsVisible = false;
    }

    private void ResetItemsSource(IEnumerable oldCollection)
    {
        if (oldCollection is INotifyCollectionChanged oldObservableCollection)
        {
            oldObservableCollection.CollectionChanged -= OnObservableCollectionChanged;
        }

        if (ItemsSource is INotifyCollectionChanged observableCollection)
        {
            observableCollection.CollectionChanged += OnObservableCollectionChanged;
        }

        OnObservableCollectionChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    private void OnObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        => ResetIndicatorsLayout();

    private void ApplyStyle(View view, int selectedIndex)
    {
        try
        {
            view.BatchBegin();
            var index = IndexOf(view);
            if (index == selectedIndex)
            {
                ApplySelectedStyle(view, index);
                return;
            }
            ApplyUnselectedStyle(view, index);
        }
        finally
        {
            view.BatchCommit();
        }
    }

    private void ResetIndicatorsStylesNonBatch()
    {
        var cyclicalIndex = SelectedIndex.ToCyclicalIndex(ItemsCount);
        OnResetIndicatorsStyles(cyclicalIndex);
    }

    private void ResetIndicatorsStyles()
    {
        try
        {
            BatchBegin();
            ResetIndicatorsStylesNonBatch();
        }
        finally
        {
            BatchCommit();
        }
    }

    private void ResetIndicatorsLayout()
    {
        try
        {
            BatchBegin();
            Children.Clear();
            if (ItemsSource == null)
            {
                return;
            }

            ItemsCount = ItemsSource.Count();
            foreach (var item in ItemsSource)
            {
                var view = ItemTemplate?.SelectTemplate(item)?.CreateView() ?? item as View;
                if (view == null)
                {
                    return;
                }

                if (!Equals(view, item))
                {
                    view.BindingContext = item;
                }

                view.GestureRecognizers.Clear();
                view.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    CommandParameter = item,
                    Command = new Command(p =>
                    {
                        if (!IsTapToNavigateEnabled)
                        {
                            return;
                        }
                        this.SetBinding(SelectedIndexProperty, nameof(CardsView.SelectedIndex), BindingMode.OneWayToSource);
                        SelectedIndex = ItemsSource.FindIndex(p);
                        this.SetBinding(SelectedIndexProperty, nameof(CardsView.SelectedIndex));
                    })
                });
                Children.Add(view);
            }
        }
        finally
        {
            ResetIndicatorsStylesNonBatch();
            ResetVisibility();
            BatchCommit();
        }
    }
}

public class CircleFrame : Frame
{
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(double), typeof(CircleFrame), 0.0, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsCircleFrame().OnSizeUpdated();
    });

    public CircleFrame()
    {
        VerticalOptions = LayoutOptions.Center;
        HorizontalOptions = LayoutOptions.Center;
        HasShadow = false;
        Padding = 0;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Preserve()
    {
    }

    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        if (width > 0 && height > 0)
        {
            SetCornerRadius(Min(width, height));
        }
    }

    protected void OnSizeUpdated()
    {
        var size = Size;
        if (size < 0)
        {
            return;
        }

        try
        {
            BatchBegin();
            HeightRequest = size;
            WidthRequest = size;
            SetCornerRadius(size);
        }
        finally
        {
            BatchCommit();
        }
    }

    private void SetCornerRadius(double size)
        => CornerRadius = (float)size / 2;
}

public class ArrowControl : ContentView
{
    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(ArrowControl), 0, BindingMode.TwoWay, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsArrowControl().ResetVisibility();
    });

    public static readonly BindableProperty ItemsCountProperty = BindableProperty.Create(nameof(ItemsCount), typeof(int), typeof(ArrowControl), 0, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsArrowControl().ResetVisibility();
    });

    public static readonly BindableProperty IsCyclicalProperty = BindableProperty.Create(nameof(IsCyclical), typeof(bool), typeof(ArrowControl), false, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsArrowControl().ResetVisibility();
    });

    public static readonly BindableProperty IsUserInteractionRunningProperty = BindableProperty.Create(nameof(IsUserInteractionRunning), typeof(bool), typeof(ArrowControl), true, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsArrowControl().ResetVisibility();
    });

    public static readonly BindableProperty IsAutoInteractionRunningProperty = BindableProperty.Create(nameof(IsAutoInteractionRunning), typeof(bool), typeof(ArrowControl), true, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsArrowControl().ResetVisibility();
    });

    public static readonly BindableProperty IsRightToLeftFlowDirectionEnabledProperty = BindableProperty.Create(nameof(IsRightToLeftFlowDirectionEnabled), typeof(bool), typeof(ArrowControl), false, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsArrowControl().OnIsRightToLeftFlowDirectionEnabledChnaged();
    });

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(ArrowControl), defaultValueCreator: b => b.AsArrowControl().DefaultImageSource, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsArrowControl().OnImageSourceChanged();
    });

    public static readonly BindableProperty UseParentAsBindingContextProperty = BindableProperty.Create(nameof(UseParentAsBindingContext), typeof(bool), typeof(ArrowControl), true);

    public static readonly BindableProperty ToFadeDurationProperty = BindableProperty.Create(nameof(ToFadeDuration), typeof(int), typeof(ArrowControl), 0);

    public static readonly BindableProperty IsRightProperty = BindableProperty.Create(nameof(IsRight), typeof(bool), typeof(ArrowControl), true);

    protected internal static readonly BindableProperty ShouldAutoNavigateToNextProperty = BindableProperty.Create(nameof(ShouldAutoNavigateToNext), typeof(bool?), typeof(CardsView), null, BindingMode.OneWayToSource);

    private CancellationTokenSource _fadeAnimationTokenSource;

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public int ItemsCount
    {
        get => (int)GetValue(ItemsCountProperty);
        set => SetValue(ItemsCountProperty, value);
    }

    public bool IsCyclical
    {
        get => (bool)GetValue(IsCyclicalProperty);
        set => SetValue(IsCyclicalProperty, value);
    }

    public bool IsUserInteractionRunning
    {
        get => (bool)GetValue(IsUserInteractionRunningProperty);
        set => SetValue(IsUserInteractionRunningProperty, value);
    }

    public bool IsAutoInteractionRunning
    {
        get => (bool)GetValue(IsAutoInteractionRunningProperty);
        set => SetValue(IsAutoInteractionRunningProperty, value);
    }

    public bool UseParentAsBindingContext
    {
        get => (bool)GetValue(UseParentAsBindingContextProperty);
        set => SetValue(UseParentAsBindingContextProperty, value);
    }

    public bool IsRightToLeftFlowDirectionEnabled
    {
        get => (bool)GetValue(IsRightToLeftFlowDirectionEnabledProperty);
        set => SetValue(IsRightToLeftFlowDirectionEnabledProperty, value);
    }

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public int ToFadeDuration
    {
        get => (int)GetValue(ToFadeDurationProperty);
        set => SetValue(ToFadeDurationProperty, value);
    }

    public bool IsRight
    {
        get => (bool)GetValue(IsRightProperty);
        set => SetValue(IsRightProperty, value);
    }

    internal bool? ShouldAutoNavigateToNext
    {
        get => GetValue(ShouldAutoNavigateToNextProperty) as bool?;
        set => SetValue(ShouldAutoNavigateToNextProperty, value);
    }

    protected Image ContentImage { get; }

    public ArrowControl()
    {
        Content = ContentImage = new Image
        {
            Aspect = Aspect.AspectFill,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
            InputTransparent = true,
            Source = ImageSource
        };

        WidthRequest = 40;
        HeightRequest = 40;

        Margin = new Thickness(20, 10);
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);

        this.SetBinding(SelectedIndexProperty, nameof(CardsView.SelectedIndex));
        this.SetBinding(ItemsCountProperty, nameof(CardsView.ItemsCount));
        this.SetBinding(IsCyclicalProperty, nameof(CardsView.IsCyclical));
        this.SetBinding(IsUserInteractionRunningProperty, nameof(CardsView.IsUserInteractionRunning));
        this.SetBinding(IsAutoInteractionRunningProperty, nameof(CardsView.IsAutoInteractionRunning));
        this.SetBinding(IsRightToLeftFlowDirectionEnabledProperty, nameof(CardsView.IsRightToLeftFlowDirectionEnabled));
        this.SetBinding(ShouldAutoNavigateToNextProperty, nameof(CardsView.ShouldAutoNavigateToNext));

        Behaviors.Add(new ProtectedControlBehavior());

        GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(OnTapped)
        });
    }

    protected virtual ImageSource DefaultImageSource => null;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Preserve()
    {
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        if (UseParentAsBindingContext && Parent is CardsView)
        {
            BindingContext = Parent;
        }
    }

    protected virtual async void ResetVisibility(uint? appearingTime = null, Easing appearingEasing = null, uint? dissappearingTime = null, Easing disappearingEasing = null)
    {
        _fadeAnimationTokenSource?.Cancel();

        var isAvailable = CheckAvailability();

        IsEnabled = isAvailable;

        if (ToFadeDuration <= 0 && isAvailable)
        {
            IsVisible = true;

            await new AnimationWrapper(v => Opacity = v, Opacity)
                .Commit(this, nameof(ResetVisibility), 16, appearingTime ?? 330, appearingEasing ?? Easing.CubicInOut);
            return;
        }

        if (isAvailable && (IsUserInteractionRunning || IsAutoInteractionRunning))
        {
            IsVisible = true;

            await new AnimationWrapper(v => Opacity = v, Opacity)
                .Commit(this, nameof(ResetVisibility), 16, appearingTime ?? 330, appearingEasing ?? Easing.CubicInOut);
            return;
        }

        _fadeAnimationTokenSource = new CancellationTokenSource();
        var token = _fadeAnimationTokenSource.Token;

        await Task.Delay(ToFadeDuration > 0 && isAvailable ? ToFadeDuration : 5);
        if (token.IsCancellationRequested)
        {
            return;
        }

        await new AnimationWrapper(v => Opacity = v, Opacity, 0)
            .Commit(this, nameof(ResetVisibility), 16, dissappearingTime ?? 330, disappearingEasing ?? Easing.SinOut);

        if (token.IsCancellationRequested)
        {
            return;
        }
        IsVisible = false;
    }

    protected virtual void OnIsRightToLeftFlowDirectionEnabledChnaged()
    {
        Rotation = IsRightToLeftFlowDirectionEnabled ? 180 : 0;
        ResetVisibility();
    }

    protected virtual void OnImageSourceChanged() => ContentImage.Source = ImageSource;

    protected virtual void OnTapped()
    {
        if (IsUserInteractionRunning || IsAutoInteractionRunning)
        {
            return;
        }
        SetSelectedWithShouldAutoNavigateToNext();
    }

    private bool CheckAvailability()
    {
        if (ItemsCount < 2)
        {
            return false;
        }

        if (IsCyclical)
        {
            return true;
        }

        var cyclicalIndex = SelectedIndex.ToCyclicalIndex(ItemsCount);

        if (cyclicalIndex == (IsRight ? ItemsCount - 1 : 0))
        {
            return false;
        }

        return true;
    }

    private void SetSelectedWithShouldAutoNavigateToNext()
    {
        try
        {
            ShouldAutoNavigateToNext = IsRight;
            SelectedIndex = (SelectedIndex + (IsRight ? 1 : -1)).ToCyclicalIndex(ItemsCount);
        }
        finally
        {
            ShouldAutoNavigateToNext = null;
        }
    }
}

public class RightArrowControl : ArrowControl
{
    public RightArrowControl()
    {
        AbsoluteLayout.SetLayoutBounds(this, new Rect(1, .5, -1, -1));
    }

    protected override ImageSource DefaultImageSource => WhiteRightArrowImageSource;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public new static void Preserve()
    {
    }
}

public class TabsControl : AbsoluteLayout
{
    public static readonly BindableProperty DiffProperty = BindableProperty.Create(nameof(Diff), typeof(double), typeof(TabsControl), 0.0, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().UpdateStripePosition();
    });

    public static readonly BindableProperty MaxDiffProperty = BindableProperty.Create(nameof(MaxDiff), typeof(double), typeof(TabsControl), 0.0, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().UpdateStripePosition();
    });

    public static readonly BindableProperty ItemsCountProperty = BindableProperty.Create(nameof(ItemsCount), typeof(int), typeof(TabsControl), -1);

    public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(TabsControl), 0, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().OnSelectedIndexChanged();
    });

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(TabsControl), null, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().ResetItemsLayout();
    });

    public static readonly BindableProperty StripeColorProperty = BindableProperty.Create(nameof(StripeColor), typeof(Color), typeof(TabsControl), Colors.CadetBlue, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().ResetStripeView();
    });

    public static readonly BindableProperty StripeHeightProperty = BindableProperty.Create(nameof(StripeHeight), typeof(double), typeof(TabsControl), 3.0, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().ResetStripeView();
    });

    public static readonly BindableProperty IsCyclicalProperty = BindableProperty.Create(nameof(IsCyclical), typeof(bool), typeof(TabsControl), false, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().ResetItemsLayout();
    });

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TabsControl), null, propertyChanged: (bindable, oldValue, _) =>
    {
        bindable.AsTabsView().ResetItemsSource(oldValue as IEnumerable);
    });

    public static readonly BindableProperty IsUserInteractionRunningProperty = BindableProperty.Create(nameof(IsUserInteractionRunning), typeof(bool), typeof(TabsControl), true, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().ResetVisibility();
    });

    public static readonly BindableProperty IsAutoInteractionRunningProperty = BindableProperty.Create(nameof(IsAutoInteractionRunning), typeof(bool), typeof(TabsControl), true, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().ResetVisibility();
    });

    public static readonly BindableProperty ToFadeDurationProperty = BindableProperty.Create(nameof(ToFadeDuration), typeof(int), typeof(TabsControl), 0);

    public static readonly BindableProperty UseParentAsBindingContextProperty = BindableProperty.Create(nameof(UseParentAsBindingContext), typeof(bool), typeof(TabsControl), true);

    public static readonly BindableProperty StripePositionProperty = BindableProperty.Create(nameof(StripePosition), typeof(StripePosition), typeof(TabsControl), StripePosition.Bottom, propertyChanged: (bindable, _, _) =>
    {
        bindable.AsTabsView().ResetItemsLayout();
    });

    static TabsControl()
    {
    }

    private CancellationTokenSource _fadeAnimationTokenSource;

    public TabsControl()
    {
        AbsoluteLayout.SetLayoutBounds(ItemsStackLayout, new Rect(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(ItemsStackLayout, AbsoluteLayoutFlags.All);
        Children.Add(ItemsStackLayout);

        AbsoluteLayout.SetLayoutBounds(MainStripeView, new Rect(0, 1, 0, 0));
        AbsoluteLayout.SetLayoutFlags(MainStripeView, AbsoluteLayoutFlags.YProportional);
        Children.Add(MainStripeView);

        AbsoluteLayout.SetLayoutBounds(AdditionalStripeView, new Rect(0, 1, 0, 0));
        AbsoluteLayout.SetLayoutFlags(AdditionalStripeView, AbsoluteLayoutFlags.YProportional);
        Children.Add(AdditionalStripeView);

        this.SetBinding(DiffProperty, nameof(CardsView.ProcessorDiff));
        this.SetBinding(MaxDiffProperty, nameof(Width));
        this.SetBinding(SelectedIndexProperty, nameof(CardsView.SelectedIndex));
        this.SetBinding(ItemsSourceProperty, nameof(CardsView.ItemsSource));
        this.SetBinding(IsCyclicalProperty, nameof(CardsView.IsCyclical));
        this.SetBinding(IsUserInteractionRunningProperty, nameof(CardsView.IsUserInteractionRunning));
        this.SetBinding(IsAutoInteractionRunningProperty, nameof(CardsView.IsAutoInteractionRunning));

        AbsoluteLayout.SetLayoutBounds(this, new Rect(.5, 1, -1, -1));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
        Behaviors.Add(new ProtectedControlBehavior());
    }

    private StackLayout ItemsStackLayout { get; } = new()
    {
        Spacing = 0,
        Orientation = StackOrientation.Horizontal
    };

    private BoxView MainStripeView { get; set; } = new();

    private BoxView AdditionalStripeView { get; set; } = new();

    public double Diff
    {
        get => (double)GetValue(DiffProperty);
        set => SetValue(DiffProperty, value);
    }

    public double MaxDiff
    {
        get => (double)GetValue(MaxDiffProperty);
        set => SetValue(MaxDiffProperty, value);
    }

    public int ItemsCount
    {
        get => (int)GetValue(ItemsCountProperty);
        set => SetValue(ItemsCountProperty, value);
    }

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public DataTemplate ItemTemplate
    {
        get => GetValue(ItemTemplateProperty) as DataTemplate;
        set => SetValue(ItemTemplateProperty, value);
    }

    public Color StripeColor
    {
        get => (Color)GetValue(StripeColorProperty);
        set => SetValue(StripeColorProperty, value);
    }

    public double StripeHeight
    {
        get => (double)GetValue(StripeHeightProperty);
        set => SetValue(StripeHeightProperty, value);
    }

    public bool IsCyclical
    {
        get => (bool)GetValue(IsCyclicalProperty);
        set => SetValue(IsCyclicalProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => GetValue(ItemsSourceProperty) as IEnumerable;
        set => SetValue(ItemsSourceProperty, value);
    }

    public bool IsUserInteractionRunning
    {
        get => (bool)GetValue(IsUserInteractionRunningProperty);
        set => SetValue(IsUserInteractionRunningProperty, value);
    }

    public bool IsAutoInteractionRunning
    {
        get => (bool)GetValue(IsAutoInteractionRunningProperty);
        set => SetValue(IsAutoInteractionRunningProperty, value);
    }

    public int ToFadeDuration
    {
        get => (int)GetValue(ToFadeDurationProperty);
        set => SetValue(ToFadeDurationProperty, value);
    }

    public bool UseParentAsBindingContext
    {
        get => (bool)GetValue(UseParentAsBindingContextProperty);
        set => SetValue(UseParentAsBindingContextProperty, value);
    }

    public StripePosition StripePosition
    {
        get => (StripePosition)GetValue(StripePositionProperty);
        set => SetValue(StripePositionProperty, value);
    }

    public new object this[int index] => ItemsSource?.FindValue(index);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Preserve()
    {
    }

    protected virtual async void ResetVisibility(uint? appearingTime = null, Easing appearingEasing = null, uint? dissappearingTime = null, Easing disappearingEasing = null)
    {
        _fadeAnimationTokenSource?.Cancel();

        if (ItemsCount == 0)
        {
            Opacity = 0;
            IsVisible = false;
            return;
        }

        if (ToFadeDuration <= 0)
        {
            Opacity = 1;
            IsVisible = true;
            return;
        }

        if (IsUserInteractionRunning || IsAutoInteractionRunning)
        {
            IsVisible = true;

            await new AnimationWrapper(v => Opacity = v, Opacity)
                .Commit(this, nameof(ResetVisibility), 16, appearingTime ?? 330, appearingEasing ?? Easing.CubicInOut);
            return;
        }

        _fadeAnimationTokenSource = new CancellationTokenSource();
        var token = _fadeAnimationTokenSource.Token;

        await Task.Delay(ToFadeDuration);
        if (token.IsCancellationRequested)
        {
            return;
        }

        await new AnimationWrapper(v => Opacity = v, Opacity, 0)
            .Commit(this, nameof(ResetVisibility), 16, dissappearingTime ?? 330, disappearingEasing ?? Easing.SinOut);

        if (token.IsCancellationRequested)
        {
            return;
        }
        IsVisible = false;
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        if (UseParentAsBindingContext && Parent is CardsView)
        {
            BindingContext = Parent;
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ResetItemsLayout();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        UpdateStripePosition();
    }

    private void ResetItemsLayout()
    {
        if (Parent == null)
        {
            return;
        }

        try
        {
            BatchBegin();
            ItemsStackLayout.Children.Clear();
            if (ItemsSource == null)
            {
                return;
            }

            ItemsCount = ItemsSource.Count();
            foreach (var item in ItemsSource)
            {
                var view = ItemTemplate?.SelectTemplate(item)?.CreateView() ?? item as View;
                if (view == null)
                {
                    return;
                }

                if (!Equals(view, item))
                {
                    view.BindingContext = item;
                }

                view.GestureRecognizers.Clear();
                view.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    CommandParameter = item,
                    Command = new Command(p =>
                    {
                        this.SetBinding(SelectedIndexProperty, nameof(CardsView.SelectedIndex), BindingMode.OneWayToSource);
                        SelectedIndex = ItemsSource.FindIndex(p);
                        this.SetBinding(SelectedIndexProperty, nameof(CardsView.SelectedIndex));
                    })
                });
                ItemsStackLayout.Children.Add(view);
            }

            ResetStripeViewNonBatch();
            UpdateStripePositionNonBatch();
        }
        finally
        {
            BatchCommit();
        }
    }

    private void ResetItemsSource(IEnumerable oldCollection)
    {
        if (oldCollection is INotifyCollectionChanged oldObservableCollection)
        {
            oldObservableCollection.CollectionChanged -= OnObservableCollectionChanged;
        }

        if (ItemsSource is INotifyCollectionChanged observableCollection)
        {
            observableCollection.CollectionChanged += OnObservableCollectionChanged;
        }

        OnObservableCollectionChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    private void OnObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        => ResetItemsLayout();

    private void ResetStripeView()
    {
        try
        {
            BatchBegin();
            ResetStripeViewNonBatch();
        }
        finally
        {
            BatchCommit();
        }
    }

    private void ResetStripeViewNonBatch()
    {
        ItemsStackLayout.Margin = new Thickness(0, 0, 0, StripeHeight);
        MainStripeView.Color = StripeColor;
        AdditionalStripeView.Color = StripeColor;
    }

    private void UpdateStripePosition()
    {
        try
        {
            BatchBegin();
            UpdateStripePositionNonBatch();
        }
        finally
        {
            BatchCommit();
        }
    }

    private void UpdateStripePositionNonBatch()
    {
        var itemsCount = ItemsCount;
        var selectedIndex = SelectedIndex.ToCyclicalIndex(itemsCount);
        var maxDiff = MaxDiff;

        if (selectedIndex < 0 || maxDiff < 0)
        {
            return;
        }

        var diff = Diff;
        var affectedIndex = diff < 0
            ? (selectedIndex + 1)
            : diff > 0
                ? (selectedIndex - 1)
                : selectedIndex;

        if (IsCyclical)
        {
            affectedIndex = affectedIndex.ToCyclicalIndex(itemsCount);
        }

        if (affectedIndex < 0 || affectedIndex >= ItemsStackLayout.Children.Count)
        {
            return;
        }

        var itemProgress = Min(Abs(diff) / maxDiff, 1);

        var currentItemView = ItemsStackLayout.Children[selectedIndex];
        var affectedItemView = ItemsStackLayout.Children[affectedIndex];
        if (diff <= 0)
        {
            CalculateStripePosition(currentItemView as View, affectedItemView as View, itemProgress, selectedIndex > affectedIndex);
            return;
        }
        CalculateStripePosition(affectedItemView as View, currentItemView as View, 1 - itemProgress, selectedIndex < affectedIndex);
    }

    private void CalculateStripePosition(View firstView, View secondView, double itemProgress, bool isSecondStripeVisible)
    {
        if (itemProgress <= 0 &&
           GetLayoutBounds(AdditionalStripeView as IView).Width >
           GetLayoutBounds(MainStripeView as IView).Width)
        {
            SwapStripeViews();
        }

        AdditionalStripeView.IsVisible = isSecondStripeVisible;
        var additionalStripeWidth = isSecondStripeVisible ? secondView.Width * itemProgress : 0;
        AbsoluteLayout.SetLayoutBounds(AdditionalStripeView, new Rect(secondView.X, StripePosition == StripePosition.Bottom ? 1 : 0, additionalStripeWidth, StripeHeight));

        var x = firstView.X + firstView.Width * itemProgress;
        var mainStripewidth = firstView.Width * (1 - itemProgress) + secondView.Width * itemProgress - additionalStripeWidth;
        AbsoluteLayout.SetLayoutBounds(MainStripeView, new Rect(x, StripePosition == StripePosition.Bottom ? 1 : 0, mainStripewidth, StripeHeight));
    }

    private void SwapStripeViews()
    {
        (MainStripeView, AdditionalStripeView) = (AdditionalStripeView, MainStripeView);
    }

    private void OnSelectedIndexChanged()
    {
        if (Parent is not ScrollView scroll)
        {
            return;
        }
        var index = SelectedIndex.ToCyclicalIndex(ItemsCount);
        if (index < 0)
        {
            return;
        }
        scroll.ScrollToAsync(ItemsStackLayout.Children[index] as View, ScrollToPosition.MakeVisible, true);
    }
}