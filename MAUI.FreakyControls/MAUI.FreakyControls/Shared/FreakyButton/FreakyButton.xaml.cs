using Maui.FreakyControls.Shared.Enums;
using System.Windows.Input;
using Maui.FreakyControls.Extensions;
namespace Maui.FreakyControls;

public partial class FreakyButton : ContentView
{
    public static readonly string IsBusyVisualState = "Busy";
    public event EventHandler Clicked;

    #region Bindable properties

    public static readonly BindableProperty ActivityIndicatorSizeProperty =
        BindableProperty.Create(nameof(ActivityIndicatorSize), typeof(double), typeof(FreakyButton), defaultValue: 30.0);

    public static readonly BindableProperty AnimationProperty =
        BindableProperty.Create(nameof(Animation), typeof(ButtonAnimations), typeof(FreakyButton), defaultValue: ButtonAnimations.FadeAndScale);

    public static readonly BindableProperty AreIconsDistantProperty =
        BindableProperty.Create(nameof(AreIconsDistant), typeof(bool), typeof(FreakyButton), defaultValue: true, propertyChanged: OnIconsAreExpandedChanged);

    public new static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(FreakyButton), defaultValue: Colors.Black);

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(FreakyButton), defaultValue: Colors.White);

    public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(FreakyButton), defaultValue: Button.BorderWidthProperty.DefaultValue);

    public static readonly BindableProperty BusyColorProperty =
        BindableProperty.Create(nameof(BusyColor), typeof(Color), typeof(FreakyButton), defaultValue: Colors.White);

    public static readonly BindableProperty CharacterSpacingProperty =
        BindableProperty.Create(nameof(CharacterSpacing), typeof(double), typeof(FreakyButton), Button.CharacterSpacingProperty.DefaultValue);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FreakyButton), defaultValue: null);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FreakyButton), defaultValue: null);

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(FreakyButton), new CornerRadius(10));

    public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(FreakyButton), defaultValue: FontAttributes.None);

    public static readonly BindableProperty FontAutoScalingEnabledProperty =
        BindableProperty.Create(nameof(FontAutoScalingEnabled), typeof(bool), typeof(FreakyButton), defaultValue: true);

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(FreakyButton), defaultValue: null);

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(FreakyButton), defaultValue: Microsoft.Maui.Font.Default.Size);

    public static readonly BindableProperty HorizontalTextAlignmentProperty =
        BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(FreakyButton), TextAlignment.Center);

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(nameof(IconSize), typeof(double), typeof(FreakyButton), defaultValue: 24.0);

    public static readonly BindableProperty IsBusyProperty =
        BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(FreakyButton), defaultValue: false, propertyChanged: OnIsBusyPropertyChanged);

    public new static readonly BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(FreakyButton), defaultValue: true);

    public static readonly BindableProperty LeadingIconProperty =
        BindableProperty.Create(nameof(LeadingIcon), typeof(View), typeof(FreakyButton), defaultValue: null, propertyChanged: OnLeadingIconChanged);

    public static readonly BindableProperty LineBreakModeProperty =
        BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(FreakyButton), defaultValue: LineBreakMode.NoWrap);

    public new static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(FreakyButton), defaultValue: new Thickness(12, 0));

    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(int), typeof(FreakyButton), defaultValue: 12);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FreakyButton), defaultValue: Colors.White);

    public static readonly BindableProperty TextDecorationsProperty =
        BindableProperty.Create(nameof(TextDecorations), typeof(TextDecorations), typeof(FreakyButton), defaultValue: TextDecorations.None);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(FreakyButton), defaultValue: null);

    public static readonly BindableProperty TextTransformProperty =
        BindableProperty.Create(nameof(TextTransform), typeof(TextTransform), typeof(FreakyButton), defaultValue: TextTransform.None);

    public static readonly BindableProperty TextTypeProperty =
        BindableProperty.Create(nameof(TextType), typeof(TextType), typeof(FreakyButton), defaultValue: TextType.Text);

    public static readonly BindableProperty TrailingIconProperty =
        BindableProperty.Create(nameof(TrailingIcon), typeof(View), typeof(FreakyButton), defaultValue: null, propertyChanged: OnTrailingIconChanged);

    public static readonly BindableProperty VerticalTextAlignmentProperty =
        BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(FreakyButton), TextAlignment.Center);

    public static readonly BindableProperty NativeAnimationColorProperty =
        BindableProperty.Create(nameof(NativeAnimationColor), typeof(Color), typeof(FreakyButton), Colors.Transparent);

    public Color NativeAnimationColor
    {
        get => (Color)GetValue(NativeAnimationColorProperty);
        set => SetValue(NativeAnimationColorProperty, value);
    }

    public double ActivityIndicatorSize
    {
        get { return (double)GetValue(ActivityIndicatorSizeProperty); }
        set { SetValue(ActivityIndicatorSizeProperty, value); }
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
        set { SetValue(BorderWidthProperty, value); }
    }

    public Color BusyColor
    {
        get => (Color)GetValue(BusyColorProperty);
        set => SetValue(BusyColorProperty, value);
    }

    public double CharacterSpacing
    {
        get { return (double)GetValue(CharacterSpacingProperty); }
        set { SetValue(CharacterSpacingProperty, value); }
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
        get { return (string)GetValue(FontFamilyProperty); }
        set { SetValue(FontFamilyProperty, value); }
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
        get { return (double)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
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
        get { return (View)GetValue(LeadingIconProperty); }
        set { SetValue(LeadingIconProperty, value); }
    }

    public LineBreakMode LineBreakMode
    {
        get => (LineBreakMode)GetValue(LineBreakModeProperty);
        set => SetValue(LineBreakModeProperty, value);
    }

    public new Thickness Padding
    {
        get { return (Thickness)GetValue(PaddingProperty); }
        set { SetValue(PaddingProperty, value); }
    }

    public int Spacing
    {
        get { return (int)GetValue(SpacingProperty); }
        set { SetValue(SpacingProperty, value); }
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
        get { return (TextType)GetValue(TextTypeProperty); }
        set { SetValue(TextTypeProperty, value); }
    }

    public View TrailingIcon
    {
        get { return (View)GetValue(TrailingIconProperty); }
        set { SetValue(TrailingIconProperty, value); }
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
        if (newValue is not null)
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
        if (newValue is not null)
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

    #region Constructors

    public FreakyButton()
    {
        InitializeComponent();
    }

    #endregion Constructors

    #region Methods

    protected override void ChangeVisualState()
    {
        if (IsBusy)
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

    async void Button_Tapped(object sender, TappedEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        switch (this.Animation)
        {
            default:
                break;
            case ButtonAnimations.Fade:
                this.NativeAnimationColor = Colors.Transparent;
                await this.FadeTo(0.7, 100);
                await this.FadeTo(1, 500);
                break;
            case ButtonAnimations.Scale:
                this.NativeAnimationColor = Colors.Transparent;
                await this.ScaleTo(0.95, 100);
                await this.ScaleTo(1, 100);
                break;
            case ButtonAnimations.FadeAndScale:
                this.NativeAnimationColor = Colors.Transparent;
                await TaskExt.WhenAll(this.ScaleTo(0.95, 100), this.FadeTo(0.7, 100));
                await TaskExt.WhenAll(this.ScaleTo(1, 100), this.FadeTo(1, 500));
                break;
        }
        Clicked?.Invoke(sender, e);
        Command?.ExecuteCommandIfAvailable(CommandParameter);
    }

    #endregion Methods
}