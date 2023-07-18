using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.Shared.Enums;
using Maui.FreakyControls.Shared.TouchPress;
using System.Windows.Input;
using Font = Microsoft.Maui.Font;

namespace Maui.FreakyControls;

public partial class FreakyButton : ContentView, ITouchPressEffect
{
    public static readonly string IsBusyVisualState = "Busy";

    #region Constructors

    public FreakyButton()
    {
        InitializeComponent();
        Effects.Add(new TouchAndPressRoutingEffect());
    }

    #endregion Constructors

    #region Bindable properties

    public static readonly BindableProperty ActivityIndicatorSizeProperty =
        BindableProperty.Create(nameof(ActivityIndicatorSize), typeof(double), typeof(FreakyButton), 30.0);

    public static readonly BindableProperty AnimationProperty =
        BindableProperty.Create(nameof(Animation), typeof(ButtonAnimations), typeof(FreakyButton),
            ButtonAnimations.Fade);

    public static readonly BindableProperty AreIconsDistantProperty =
        BindableProperty.Create(nameof(AreIconsDistant), typeof(bool), typeof(FreakyButton), true,
            propertyChanged: OnIconsAreExpandedChanged);

    public new static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(FreakyButton), Colors.Black);

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(FreakyButton), Colors.White);

    public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(FreakyButton),
            Button.BorderWidthProperty.DefaultValue);

    public static readonly BindableProperty BusyColorProperty =
        BindableProperty.Create(nameof(BusyColor), typeof(Color), typeof(FreakyButton), Colors.White);

    public static readonly BindableProperty CharacterSpacingProperty =
        BindableProperty.Create(nameof(CharacterSpacing), typeof(double), typeof(FreakyButton),
            Button.CharacterSpacingProperty.DefaultValue);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FreakyButton), null);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FreakyButton), null);

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(FreakyButton),
            new CornerRadius(10));

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(FreakyButton),
            FontAttributes.None);

    public static readonly BindableProperty FontAutoScalingEnabledProperty =
        BindableProperty.Create(nameof(FontAutoScalingEnabled), typeof(bool), typeof(FreakyButton), true);

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(FreakyButton), null);

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(FreakyButton), Font.Default.Size);

    public static readonly BindableProperty HorizontalTextAlignmentProperty =
        BindableProperty.Create(
            nameof(HorizontalTextAlignment),
            typeof(TextAlignment),
            typeof(FreakyButton),
            TextAlignment.Center);

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(nameof(IconSize), typeof(double), typeof(FreakyButton), 24.0);

    public static readonly BindableProperty IsBusyProperty =
        BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(FreakyButton), false,
            propertyChanged: OnIsBusyPropertyChanged);

    public new static readonly BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(FreakyButton), true);

    public static readonly BindableProperty LeadingIconProperty =
        BindableProperty.Create(nameof(LeadingIcon), typeof(View), typeof(FreakyButton), null,
            propertyChanged: OnLeadingIconChanged);

    public static readonly BindableProperty LineBreakModeProperty =
        BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(FreakyButton),
            LineBreakMode.NoWrap);

    public new static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(FreakyButton), new Thickness(12, 0));

    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(int), typeof(FreakyButton), 12);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FreakyButton), Colors.White);

    public static readonly BindableProperty TextDecorationsProperty =
        BindableProperty.Create(nameof(TextDecorations), typeof(TextDecorations), typeof(FreakyButton),
            TextDecorations.None);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(FreakyButton), null);

    public static readonly BindableProperty TextTransformProperty =
        BindableProperty.Create(nameof(TextTransform), typeof(TextTransform), typeof(FreakyButton), TextTransform.None);

    public static readonly BindableProperty TextTypeProperty =
        BindableProperty.Create(nameof(TextType), typeof(TextType), typeof(FreakyButton), TextType.Text);

    public static readonly BindableProperty TrailingIconProperty =
        BindableProperty.Create(nameof(TrailingIcon), typeof(View), typeof(FreakyButton), null,
            propertyChanged: OnTrailingIconChanged);

    public static readonly BindableProperty VerticalTextAlignmentProperty =
        BindableProperty.Create(
            nameof(VerticalTextAlignment),
            typeof(TextAlignment),
            typeof(FreakyButton),
            TextAlignment.Center);

    public event EventHandler Clicked;

    public double ActivityIndicatorSize
    {
        get => (double)GetValue(ActivityIndicatorSizeProperty);
        set => SetValue(ActivityIndicatorSizeProperty, value);
    }

    public ButtonAnimations Animation
    {
        get => (ButtonAnimations)GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    public bool AreIconsDistant
    {
        get => (bool)GetValue(AreIconsDistantProperty);
        set => SetValue(AreIconsDistantProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public double BorderWidth
    {
        get => (double)GetValue(BorderWidthProperty);
        set => SetValue(BorderWidthProperty, value);
    }

    public Color BusyColor
    {
        get => (Color)GetValue(BusyColorProperty);
        set => SetValue(BusyColorProperty, value);
    }

    public double CharacterSpacing
    {
        get => (double)GetValue(CharacterSpacingProperty);
        set => SetValue(CharacterSpacingProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public bool FontAutoScalingEnabled
    {
        get => (bool)GetValue(FontAutoScalingEnabledProperty);
        set => SetValue(FontAutoScalingEnabledProperty, value);
    }

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public View LeadingIcon
    {
        get => (View)GetValue(LeadingIconProperty);
        set => SetValue(LeadingIconProperty, value);
    }

    public LineBreakMode LineBreakMode
    {
        get => (LineBreakMode)GetValue(LineBreakModeProperty);
        set => SetValue(LineBreakModeProperty, value);
    }

    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public int Spacing
    {
        get => (int)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public TextDecorations TextDecorations
    {
        get => (TextDecorations)GetValue(TextDecorationsProperty);
        set => SetValue(TextDecorationsProperty, value);
    }

    public TextTransform TextTransform
    {
        get => (TextTransform)GetValue(TextTransformProperty);
        set => SetValue(TextTransformProperty, value);
    }

    public TextType TextType
    {
        get => (TextType)GetValue(TextTypeProperty);
        set => SetValue(TextTypeProperty, value);
    }

    public View TrailingIcon
    {
        get => (View)GetValue(TrailingIconProperty);
        set => SetValue(TrailingIconProperty, value);
    }

    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    private static void OnIconsAreExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var freakyButton = bindable as FreakyButton;
        var areIconsExpanded = (bool)newValue;
        freakyButton.mainGrid.HorizontalOptions = areIconsExpanded ? LayoutOptions.Fill : LayoutOptions.Center;
    }

    private static async void OnIsBusyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var freakyButton = bindable as FreakyButton;
        var isBusy = (bool)newValue;
        if (isBusy)
        {
            await freakyButton.txtLabel.TranslateTo(0, -35, 300, Easing.Linear);
            freakyButton.txtLabel.IsVisible = false;
            freakyButton.activityIndicator.IsVisible = true;
            await freakyButton.activityIndicator.TranslateTo(0, 0, 200, Easing.Linear);
        }
        else
        {
            freakyButton.txtLabel.IsVisible = false;
            freakyButton.txtLabel.IsVisible = true;
            freakyButton.activityIndicator.TranslationY = 35;
            freakyButton.txtLabel.TranslationY = 0;
        }
    }

    private static void OnLeadingIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var freakyButton = bindable as FreakyButton;
        if (newValue != null)
        {
            freakyButton.leadingContentView.IsVisible = true;
            freakyButton.leadingContentView.Content = newValue as View;
        }
        else
        {
            freakyButton.leadingContentView.IsVisible = true;
        }
    }

    private static void OnTrailingIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var freakyButton = bindable as FreakyButton;
        if (newValue != null)
        {
            freakyButton.trailingContentView.IsVisible = true;
            freakyButton.trailingContentView.Content = newValue as View;
        }
        else
        {
            freakyButton.trailingContentView.IsVisible = true;
        }
    }

    #endregion Bindable properties

    #region Methods

    public void ConsumeEvent(EventType gestureType)
    {
        if (IsEnabled)
            TouchAndPressAnimation.Animate(this, gestureType);
    }

    public void ExecuteAction()
    {
        if (IsEnabled)
            Command?.ExecuteCommandIfAvailable(CommandParameter);

        if (IsEnabled && Clicked != null)
            Clicked.Invoke(this, EventArgs.Empty);
    }

    protected override void ChangeVisualState()
    {
        if (IsBusy)
            VisualStateManager.GoToState(this, IsBusyVisualState);
        else if (IsEnabled)
            VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Normal);
        else
            VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Disabled);
    }

    #endregion Methods
}