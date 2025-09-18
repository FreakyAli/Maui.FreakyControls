using Maui.FreakyControls.Extensions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.FreakyControls;

public partial class FreakyChip : ContentView
{
    private const string defaultName = "Chip";
    private const string IsSelectedVisualState = "Selected";
    private readonly TapGestureRecognizer tapped = new();
    private static readonly double size = 25.0;

    /// <summary>
    /// Raised when <see cref="FreakyChip.IsSelected"/> changes.
    /// </summary>
    public event EventHandler<CheckedChangedEventArgs> SelectedChanged;

    public FreakyChip()
    {
        InitializeComponent();
        tapped.Tapped += CheckBox_Tapped;
        GestureRecognizers.Add(tapped);
        trailingIcon.HeightRequest = trailingIcon.WidthRequest =
        leadingIcon.HeightRequest = leadingIcon.WidthRequest = size;
    }

    private void CheckBox_Tapped(object sender, EventArgs e)
    {
        if (IsEnabled)
        {
            IsSelected = !IsSelected;
        }
    }

    public static readonly BindableProperty FontAttributesProperty =
       BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(FreakyChip), defaultValue: FontAttributes.None);

    public static readonly BindableProperty FontAutoScalingEnabledProperty =
       BindableProperty.Create(nameof(FontAutoScalingEnabled), typeof(bool), typeof(FreakyChip), defaultValue: true);

    public static readonly BindableProperty FontFamilyProperty =
       BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(FreakyChip), defaultValue: null);

    public static readonly BindableProperty FontSizeProperty =
       BindableProperty.Create(nameof(FontSize), typeof(double), typeof(FreakyChip), defaultValue: Microsoft.Maui.Font.Default.Size);

    public static readonly BindableProperty TextDecorationsProperty =
       BindableProperty.Create(nameof(TextDecorations), typeof(TextDecorations), typeof(FreakyChip), defaultValue: TextDecorations.None);

    public static readonly BindableProperty TextProperty =
       BindableProperty.Create(nameof(Text), typeof(string), typeof(FreakyChip), defaultValue: null);

    public static readonly BindableProperty TextTransformProperty =
       BindableProperty.Create(nameof(TextTransform), typeof(TextTransform), typeof(FreakyChip), defaultValue: TextTransform.None);

    public static readonly BindableProperty TextTypeProperty =
       BindableProperty.Create(nameof(TextType), typeof(TextType), typeof(FreakyChip), defaultValue: TextType.Text);

    public static readonly BindableProperty VerticalTextAlignmentProperty =
       BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(FreakyChip), TextAlignment.Center);

    public static readonly BindableProperty HorizontalTextAlignmentProperty =
       BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(FreakyChip), TextAlignment.Center);

    public static readonly BindableProperty SelectedChangedCommandProperty =
       BindableProperty.Create(nameof(SelectedChangedCommand), typeof(ICommand), typeof(FreakyCheckbox));

    public static readonly BindableProperty IsSelectedProperty =
       BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(FreakyChip), false, BindingMode.TwoWay, propertyChanged: OnSelectedChanged);

    public static readonly BindableProperty CornerRadiusProperty =
       BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(FreakyChip), default(CornerRadius));

    public static readonly BindableProperty SelectedBackgroundColorProperty =
       BindableProperty.Create(nameof(SelectedBackgroundColor), typeof(Color), typeof(FreakyChip), defaultValue: Colors.LightGray);

    public static readonly BindableProperty UnselectedBackgroundColorProperty =
       BindableProperty.Create(nameof(UnselectedBackgroundColor), typeof(Color), typeof(FreakyChip), defaultValue: Colors.Transparent);

    public static readonly BindableProperty SelectedTextColorProperty =
       BindableProperty.Create(nameof(SelectedTextColor), typeof(Color), typeof(FreakyChip), defaultValue: Colors.Black);

    public static readonly BindableProperty UnselectedTextColorProperty =
       BindableProperty.Create(nameof(UnselectedTextColor), typeof(Color), typeof(FreakyChip), defaultValue: Colors.Black);

    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(FreakyChip), defaultName);

    public static readonly BindableProperty SizeRequestProperty =
        BindableProperty.Create(nameof(SizeRequest), typeof(double), typeof(FreakyChip), propertyChanged: OnSizeRequestChanged);

    public static readonly BindableProperty SvgAssemblyProperty =
        BindableProperty.Create(nameof(SvgAssembly), typeof(Assembly), typeof(FreakyChip), default(Assembly));

    public static readonly BindableProperty LeadingResourceIdProperty =
        BindableProperty.Create(nameof(LeadingResourceId), typeof(string), typeof(FreakyChip), default(string));

    public static readonly BindableProperty LeadingBase64StringProperty =
        BindableProperty.Create(nameof(LeadingBase64String), typeof(string), typeof(FreakyChip), default(string));

    public static readonly BindableProperty TrailingResourceIdProperty =
        BindableProperty.Create(nameof(TrailingResourceId), typeof(string), typeof(FreakyChip), default(string));

    public static readonly BindableProperty TrailingBase64StringProperty =
        BindableProperty.Create(nameof(TrailingBase64String), typeof(string), typeof(FreakyChip), default(string));

    public static readonly BindableProperty ImageColorProperty =
        BindableProperty.Create(nameof(ImageColor), typeof(Color), typeof(FreakyChip), Colors.Transparent);

    public static readonly BindableProperty AnimationColorProperty =
        BindableProperty.Create(nameof(AnimationColor), typeof(Color), typeof(FreakyChip), ControlConstants.DefaultControlRipple);

    public new static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(FreakyChip), new Thickness(10));

    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(FreakyChip), default(Brush));

    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(FreakyChip), 0.0);

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public Brush Stroke
    {
        get => (Brush)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public Color AnimationColor
    {
        get => (Color)GetValue(AnimationColorProperty);
        set => SetValue(AnimationColorProperty, value);
    }

    public Color ImageColor
    {
        get => (Color)GetValue(ImageColorProperty);
        set => SetValue(ImageColorProperty, value);
    }

    public string TrailingResourceId
    {
        get => (string)GetValue(TrailingResourceIdProperty);
        set => SetValue(TrailingResourceIdProperty, value);
    }

    public string TrailingBase64String
    {
        get => (string)GetValue(TrailingBase64StringProperty);
        set => SetValue(TrailingBase64StringProperty, value);
    }

    public string LeadingResourceId
    {
        get => (string)GetValue(LeadingResourceIdProperty);
        set => SetValue(LeadingResourceIdProperty, value);
    }

    public string LeadingBase64String
    {
        get => (string)GetValue(LeadingBase64StringProperty);
        set => SetValue(LeadingBase64StringProperty, value);
    }

    public Assembly SvgAssembly
    {
        get { return (Assembly)GetValue(SvgAssemblyProperty); }
        set { SetValue(SvgAssemblyProperty, value); }
    }

    public double SizeRequest
    {
        get { return (double)GetValue(SizeRequestProperty); }
        set { SetValue(SizeRequestProperty, value); }
    }

    public string Name
    {
        get { return (string)GetValue(NameProperty); }
        set { SetValue(NameProperty, value); }
    }

    public Color UnselectedTextColor
    {
        get => (Color)GetValue(UnselectedTextColorProperty);
        set => SetValue(UnselectedTextColorProperty, value);
    }

    public Color SelectedTextColor
    {
        get => (Color)GetValue(SelectedTextColorProperty);
        set => SetValue(SelectedTextColorProperty, value);
    }

    public Color UnselectedBackgroundColor
    {
        get => (Color)GetValue(UnselectedBackgroundColorProperty);
        set => SetValue(UnselectedBackgroundColorProperty, value);
    }

    public Color SelectedBackgroundColor
    {
        get => (Color)GetValue(SelectedBackgroundColorProperty);
        set => SetValue(SelectedBackgroundColorProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public ICommand SelectedChangedCommand
    {
        get => (ICommand)GetValue(SelectedChangedCommandProperty);
        set => SetValue(SelectedChangedCommandProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
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

    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    private static void OnSelectedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not FreakyChip chip) return;
        chip.ChangeVisualState();
        chip.SelectedChanged?.Invoke(chip, new CheckedChangedEventArgs((bool)newValue));
        chip.SelectedChangedCommand?.ExecuteWhenAvailable(newValue);
    }

    private static void OnSizeRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FreakyChip chip && newValue is double newV)
        {
            chip.trailingIcon.HeightRequest = chip.trailingIcon.WidthRequest =
                chip.leadingIcon.HeightRequest = chip.leadingIcon.WidthRequest = newV;
        }
    }

    protected override void ChangeVisualState()
    {
        if (IsEnabled && IsSelected)
            VisualStateManager.GoToState(this, IsSelectedVisualState);
        else
            base.ChangeVisualState();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(IsSelected) ||
            propertyName == nameof(SelectedBackgroundColor) ||
            propertyName == nameof(SelectedBackgroundColor))
        {
            border.BackgroundColor = IsSelected ? SelectedBackgroundColor : UnselectedBackgroundColor;
        }

        if (propertyName == nameof(IsSelected) ||
           propertyName == nameof(SelectedBackgroundColor) ||
           propertyName == nameof(SelectedBackgroundColor))
        {
            this.chipLabel.TextColor = IsSelected ? SelectedTextColor : UnselectedTextColor;
        }
    }
}