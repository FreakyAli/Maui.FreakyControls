using Maui.FreakyControls.Shared.Enums;
using System.ComponentModel;
using System.Windows.Input;

namespace Maui.FreakyControls;

public partial class FreakyPinCodeControl : ContentView
{
    public event EventHandler<FreakyCodeCompletedEventArgs> CodeEntryCompleted;

    public FreakyPinCodeControl()
    {
        InitializeComponent();
    }

    #region BindableProperties

    public string CodeValue
    {
        get => (string)GetValue(CodeValueProperty);
        set => SetValue(CodeValueProperty, value);
    }

    public static readonly BindableProperty CodeValueProperty =
       BindableProperty.Create(
           nameof(CodeValue),
           typeof(string),
           typeof(FreakyCodeView),
           string.Empty,
           defaultBindingMode: BindingMode.TwoWay);

    public int CodeLength
    {
        get => (int)GetValue(CodeLengthProperty);
        set => SetValue(CodeLengthProperty, value);
    }

    public static readonly BindableProperty CodeLengthProperty =
      BindableProperty.Create(
          nameof(CodeLength),
          typeof(int),
          typeof(FreakyCodeView),
          CodeView.DefaultCodeLength,
          defaultBindingMode: BindingMode.OneWay);

    public KeyboardType CodeInputType
    {
        get => (KeyboardType)GetValue(CodeInputTypeProperty);
        set => SetValue(CodeInputTypeProperty, value);
    }

    public static readonly BindableProperty CodeInputTypeProperty =
       BindableProperty.Create(
           nameof(CodeInputType),
           typeof(KeyboardType),
           typeof(FreakyCodeView),
            KeyboardType.Numeric,
           defaultBindingMode: BindingMode.OneWay);

    public ICommand CodeEntryCompletedCommand
    {
        get { return (ICommand)GetValue(CodeEntryCompletedCommandProperty); }
        set { SetValue(CodeEntryCompletedCommandProperty, value); }
    }

    public static readonly BindableProperty CodeEntryCompletedCommandProperty =
       BindableProperty.Create(
          nameof(CodeEntryCompletedCommand),
          typeof(ICommand),
          typeof(FreakyCodeView),
          null);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty =
      BindableProperty.Create(
          nameof(IsPassword),
          typeof(bool),
          typeof(FreakyCodeView),
          true,
          defaultBindingMode: BindingMode.OneWay);

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly BindableProperty ColorProperty =
      BindableProperty.Create(
          nameof(Color),
          typeof(Color),
          typeof(FreakyCodeView),
          Colors.Black,
          defaultBindingMode: BindingMode.OneWay);

    public double ItemSpacing
    {
        get => (double)GetValue(ItemSpacingProperty);
        set => SetValue(ItemSpacingProperty, value);
    }

    public static readonly BindableProperty ItemSpacingProperty =
      BindableProperty.Create(
          nameof(ItemSpacing),
          typeof(double),
          typeof(FreakyCodeView),
          CodeView.DefaultItemSpacing,
          defaultBindingMode: BindingMode.OneWay);

    public double ItemSize
    {
        get => (double)GetValue(ItemSizeProperty);
        set => SetValue(ItemSizeProperty, value);
    }

    public static readonly BindableProperty ItemSizeProperty =
      BindableProperty.Create(
          nameof(ItemSize),
          typeof(double),
          typeof(FreakyCodeView),
          CodeView.DefaultItemSize,
          defaultBindingMode: BindingMode.OneWay);

    public ItemShape ItemShape
    {
        get => (ItemShape)GetValue(ItemShapeProperty);
        set => SetValue(ItemShapeProperty, value);
    }

    public static readonly BindableProperty ItemShapeProperty =
       BindableProperty.Create(
           nameof(ItemShape),
           typeof(ItemShape),
           typeof(FreakyCodeView),
           ItemShape.Circle,
           defaultBindingMode: BindingMode.OneWay);

    public Color ItemFocusColor
    {
        get => (Color)GetValue(ItemFocusColorProperty);
        set => SetValue(ItemFocusColorProperty, value);
    }

    public static readonly BindableProperty ItemFocusColorProperty =
      BindableProperty.Create(
          nameof(ItemFocusColor),
          typeof(Color),
          typeof(FreakyCodeView),
          Colors.Black,
          defaultBindingMode: BindingMode.OneWay);

    public FocusAnimation ItemFocusAnimation
    {
        get => (FocusAnimation)GetValue(ItemFocusAnimationProperty);
        set => SetValue(ItemFocusAnimationProperty, value);
    }

    public static readonly BindableProperty ItemFocusAnimationProperty =
       BindableProperty.Create(
           nameof(ItemFocusAnimation),
           typeof(FocusAnimation),
           typeof(FreakyCodeView),
           default(FocusAnimation),
           defaultBindingMode: BindingMode.OneWay);

    public Color ItemBorderColor
    {
        get => (Color)GetValue(ItemBorderColorProperty);
        set => SetValue(ItemBorderColorProperty, value);
    }

    public static readonly BindableProperty ItemBorderColorProperty =
      BindableProperty.Create(
          nameof(ItemBorderColor),
          typeof(Color),
          typeof(FreakyCodeView),
          Colors.Black,
          defaultBindingMode: BindingMode.OneWay);

    public Color ItemBackgroundColor
    {
        get => (Color)GetValue(ItemBackgroundColorProperty);
        set => SetValue(ItemBackgroundColorProperty, value);
    }

    public static readonly BindableProperty ItemBackgroundColorProperty =
      BindableProperty.Create(
          nameof(ItemBackgroundColor),
          typeof(Color),
          typeof(FreakyCodeView),
          default(Color),
          defaultBindingMode: BindingMode.OneWay);

    public double ItemBorderWidth
    {
        get => (double)GetValue(ItemBorderWidthProperty);
        set => SetValue(ItemBorderWidthProperty, value);
    }

    public static readonly BindableProperty ItemBorderWidthProperty =
      BindableProperty.Create(
          nameof(ItemBorderWidth),
          typeof(double),
          typeof(FreakyCodeView),
          5.0,
          defaultBindingMode: BindingMode.OneWay);

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
      BindableProperty.Create(
          nameof(FontSize),
          typeof(double),
          typeof(FreakyCodeView),
          FreakyCodeView.FontSizeProperty.DefaultValue,
          defaultBindingMode: BindingMode.OneWay);

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
      nameof(FontFamily),
      typeof(string),
      typeof(FreakyCodeView));

    private void FreakyCodeView_CodeEntryCompleted(object sender, FreakyCodeCompletedEventArgs e)
    {
        CodeEntryCompleted?.Invoke(this, e);
    }

    public ImageSource BackspaceButtonSource
    {
        get => (ImageSource)GetValue(BackspaceButtonSourceProperty);
        set => SetValue(BackspaceButtonSourceProperty, value);
    }

    public static readonly BindableProperty BackspaceButtonSourceProperty = BindableProperty.Create(
      nameof(BackspaceButtonSource),
      typeof(ImageSource),
      typeof(FreakyCodeView),
      null);

    public Color KeyboardBackgroundColor
    {
        get => (Color)GetValue(KeyboardBackgroundColorProperty);
        set => SetValue(KeyboardBackgroundColorProperty, value);
    }

    public static readonly BindableProperty KeyboardBackgroundColorProperty = BindableProperty.Create(
      nameof(KeyboardBackgroundColor),
      typeof(Color),
      typeof(FreakyCodeView),
      Colors.White);

    public Color CancelBackgroundColor
    {
        get => (Color)GetValue(CancelBackgroundColorProperty);
        set => SetValue(CancelBackgroundColorProperty, value);
    }

    public static readonly BindableProperty CancelBackgroundColorProperty = BindableProperty.Create(
      nameof(CancelBackgroundColor),
      typeof(Color),
      typeof(FreakyCodeView),
      Colors.White);

    public Color BackspaceBackgroundColor
    {
        get => (Color)GetValue(BackspaceBackgroundColorProperty);
        set => SetValue(BackspaceBackgroundColorProperty, value);
    }

    public static readonly BindableProperty BackspaceBackgroundColorProperty = BindableProperty.Create(
      nameof(BackspaceBackgroundColor),
      typeof(Color),
      typeof(FreakyCodeView),
      Colors.White);

    public Color KeyboardTextColor
    {
        get => (Color)GetValue(KeyboardTextColorProperty);
        set => SetValue(KeyboardTextColorProperty, value);
    }

    public static readonly BindableProperty KeyboardTextColorProperty = BindableProperty.Create(
      nameof(KeyboardTextColor),
      typeof(Color),
      typeof(FreakyCodeView),
      Colors.Black);

    public double KeyboardButtonSizeRequest
    {
        get => (double)GetValue(KeyboardButtonSizeRequestProperty);
        set => SetValue(KeyboardButtonSizeRequestProperty, value);
    }

    public static readonly BindableProperty KeyboardButtonSizeRequestProperty = BindableProperty.Create(
      nameof(BackspaceBackgroundColor),
      typeof(double),
      typeof(FreakyCodeView),
      100.0);

    public int KeyboardButtonCornerRadius
    {
        get => (int)GetValue(KeyboardButtonCornerRadiusProperty);
        set => SetValue(KeyboardButtonCornerRadiusProperty, value);
    }

    public static readonly BindableProperty KeyboardButtonCornerRadiusProperty = BindableProperty.Create(
      nameof(KeyboardButtonCornerRadius),
      typeof(int),
      typeof(FreakyCodeView),
      10);

    public Thickness CancelButtonPadding
    {
        get => (Thickness)GetValue(CancelButtonPaddingProperty);
        set => SetValue(CancelButtonPaddingProperty, value);
    }

    public static readonly BindableProperty CancelButtonPaddingProperty = BindableProperty.Create(
      nameof(CancelButtonPadding),
      typeof(Thickness),
      typeof(FreakyCodeView),
      new Thickness(20));

    private void Keyboard_Clicked(object sender, System.EventArgs e)
    {
        var button = (Button)sender;
        var text = button.Text;
        this.CodeValue += text;
    }

    private void Cancel_Clicked(object sender, System.EventArgs e)
    {

    }

    void ImageButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (CodeValue.Length != 0)
            CodeValue = CodeValue[..^1];
    }
    #endregion BindableProperties
}