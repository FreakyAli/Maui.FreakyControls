using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.TouchPress;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.FreakyControls;

public partial class FreakyButton : ContentView, ITouchPressEffect
{
    public static readonly string IsBusyVisualState = "Busy";

    #region Bindable properties

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FreakyButton), defaultValue: null);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FreakyButton), defaultValue: null);

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public new static readonly BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(FreakyButton), defaultValue: true);

    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    public static readonly BindableProperty AreIconsDistantProperty =
       BindableProperty.Create(nameof(AreIconsDistant), typeof(bool), typeof(FreakyButton), defaultValue: true, propertyChanged: OnIconsAreExpandedChanged);

    private static void OnIconsAreExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var freakyButton = bindable as FreakyButton;
        var areIconsExpanded =(bool) newValue;
        freakyButton.mainGrid.HorizontalOptions = areIconsExpanded ? LayoutOptions.Fill : LayoutOptions.Center;
    }

    public bool FontAutoScalingEnabled
    {
        get => (bool)GetValue(FontAutoScalingEnabledProperty);
        set => SetValue(FontAutoScalingEnabledProperty, value);
    }

    public static readonly BindableProperty FontAutoScalingEnabledProperty =
        BindableProperty.Create(nameof(FontAutoScalingEnabled), typeof(bool), typeof(FreakyButton), defaultValue: true);

    public bool AreIconsDistant
    {
        get => (bool)GetValue(AreIconsDistantProperty);
        set => SetValue(AreIconsDistantProperty, value);
    }

    public static readonly BindableProperty AnimationProperty =
        BindableProperty.Create(nameof(Animation), typeof(AnimationTypes), typeof(FreakyButton), defaultValue: AnimationTypes.Fade);

    public AnimationTypes Animation
    {
        get => (AnimationTypes)GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    public static readonly BindableProperty FontAttributesProperty =
       BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(FreakyButton), defaultValue: FontAttributes.None);

    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public static readonly BindableProperty TextDecorationsProperty =
      BindableProperty.Create(nameof(TextDecorations), typeof(TextDecorations), typeof(FreakyButton), defaultValue: TextDecorations.None);

    public TextDecorations TextDecorations
    {
        get => (TextDecorations)GetValue(TextDecorationsProperty);
        set => SetValue(TextDecorationsProperty, value);
    }

    public static readonly BindableProperty LineBreakModeProperty =
     BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(FreakyButton), defaultValue: LineBreakMode.NoWrap);

    public LineBreakMode LineBreakMode
    {
        get => (LineBreakMode)GetValue(LineBreakModeProperty);
        set => SetValue(LineBreakModeProperty, value);
    }

    public static readonly BindableProperty TextTransformProperty =
     BindableProperty.Create(nameof(TextTransform), typeof(TextTransform), typeof(FreakyButton), defaultValue: TextTransform.None);

    public TextTransform TextTransform
    {
        get => (TextTransform)GetValue(TextTransformProperty);
        set => SetValue(TextTransformProperty, value);
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(FreakyButton), defaultValue: null);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty HorizontalTextAlignmentProperty =
       BindableProperty.Create(
           nameof(HorizontalTextAlignment),
           typeof(TextAlignment),
           typeof(FreakyButton),
           TextAlignment.Center);

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    public static readonly BindableProperty VerticalTextAlignmentProperty =
        BindableProperty.Create(
            nameof(VerticalTextAlignment),
            typeof(TextAlignment),
            typeof(FreakyButton),
            TextAlignment.Center);

    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(FreakyButton),
            new CornerRadius(10));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly BindableProperty IsBusyProperty =
        BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(FreakyButton), defaultValue: false, propertyChanged:OnIsBusyPropertyChanged);

    private async static void OnIsBusyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FreakyButton), defaultValue: Colors.White);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public new static readonly BindableProperty BackgroundColorProperty =
       BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(FreakyButton), defaultValue: Colors.Black);

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(FreakyButton), defaultValue: Colors.White);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public static readonly BindableProperty BusyColorProperty =
        BindableProperty.Create(nameof(BusyColor), typeof(Color), typeof(FreakyButton), defaultValue: Colors.White);

    public Color BusyColor
    {
        get => (Color)GetValue(BusyColorProperty);
        set => SetValue(BusyColorProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(FreakyButton), defaultValue: Microsoft.Maui.Font.Default.Size);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty BorderWidthProperty =
       BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(FreakyButton), defaultValue: Button.BorderWidthProperty.DefaultValue);

    public double BorderWidth
    {
        get => (double)GetValue(BorderWidthProperty);
        set { SetValue(BorderWidthProperty, value); }
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(FreakyButton), defaultValue: null);

    public string FontFamily
    {
        get { return (string)GetValue(FontFamilyProperty); }
        set { SetValue(FontFamilyProperty, value); }
    }

    public static readonly BindableProperty LeadingIconProperty =
        BindableProperty.Create(nameof(LeadingIcon), typeof(View), typeof(FreakyButton), defaultValue: null, propertyChanged: OnLeadingIconChanged);

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

    public View LeadingIcon
    {
        get { return (View)GetValue(LeadingIconProperty); }
        set { SetValue(LeadingIconProperty, value); }
    }

    public static readonly BindableProperty TrailingIconProperty =
        BindableProperty.Create(nameof(TrailingIcon), typeof(View), typeof(FreakyButton), defaultValue: null, propertyChanged: OnTrailingIconChanged);

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

    public View TrailingIcon
    {
        get { return (View)GetValue(TrailingIconProperty); }
        set { SetValue(TrailingIconProperty, value); }
    }

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(nameof(IconSize), typeof(double), typeof(FreakyButton), defaultValue: 24.0);

    public double IconSize
    {
        get { return (double)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
    }

    public static readonly BindableProperty CharacterSpacingProperty =
    BindableProperty.Create(nameof(CharacterSpacing), typeof(double), typeof(FreakyButton), Button.CharacterSpacingProperty.DefaultValue);

    public double CharacterSpacing
    {
        get { return (double)GetValue(CharacterSpacingProperty); }
        set { SetValue(CharacterSpacingProperty, value); }
    }

    public static readonly BindableProperty ActivityIndicatorSizeProperty =
        BindableProperty.Create(nameof(ActivityIndicatorSize), typeof(double), typeof(FreakyButton), defaultValue: 30.0);

    public double ActivityIndicatorSize
    {
        get { return (double)GetValue(ActivityIndicatorSizeProperty); }
        set { SetValue(ActivityIndicatorSizeProperty, value); }
    }

    public new static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(FreakyButton), defaultValue: new Thickness(12, 0));

    public new Thickness Padding
    {
        get { return (Thickness)GetValue(PaddingProperty); }
        set { SetValue(PaddingProperty, value); }
    }

    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(int), typeof(FreakyButton), defaultValue: 12);

    public int Spacing
    {
        get { return (int)GetValue(SpacingProperty); }
        set { SetValue(SpacingProperty, value); }
    }

    public static readonly BindableProperty TextTypeProperty =
       BindableProperty.Create(nameof(TextType), typeof(TextType), typeof(FreakyButton), defaultValue: TextType.Text);

    public TextType TextType
    {
        get { return (TextType)GetValue(TextTypeProperty); }
        set { SetValue(TextTypeProperty, value); }
    }

    public event EventHandler Clicked;

    #endregion Bindable properties

    #region Constructors

    public FreakyButton()
    {
        InitializeComponent();
        this.Effects.Add(new TouchAndPressEffect());
    }

    #endregion Constructors

    #region Methods

    protected override void ChangeVisualState()
    {
        if(IsBusy)
        {
            VisualStateManager.GoToState(this, FreakyButton.IsBusyVisualState);
        }
        else if (IsEnabled)
        {
            VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Normal);
        }
        else
        {
            VisualStateManager.GoToState(this, VisualStateManager.CommonStates.Disabled);
        }
    }

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

    #endregion Methods
}