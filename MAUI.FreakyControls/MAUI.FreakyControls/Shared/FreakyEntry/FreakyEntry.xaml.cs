using System.ComponentModel;
using System.Windows.Input;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public partial class FreakyEntry : ContentView
{
    public FreakyEntry()
    {
        InitializeComponent();
    }

    #region BindableProperties
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
       nameof(Text),
       typeof(string),
       typeof(FreakyTextInputLayout),
       string.Empty,
       BindingMode.TwoWay,
       null
       );

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder),
        typeof(string),
        typeof(FreakyTextInputLayout),
        string.Empty,
        BindingMode.TwoWay,
        null
        );

    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
        nameof(ReturnType),
        typeof(ReturnType),
        typeof(FreakyTextInputLayout),
        ReturnType.Default
        );

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword),
        typeof(bool),
        typeof(FreakyTextInputLayout),
        default(bool)
        );

    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
        nameof(Keyboard),
        typeof(Microsoft.Maui.Keyboard),
        typeof(FreakyTextInputLayout),
        Keyboard.Default,
        coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default
        );

    public static readonly BindableProperty BorderStrokeThicknessProperty = BindableProperty.Create(
        nameof(BorderStrokeThickness),
        typeof(double),
        typeof(FreakyTextInputLayout),
        default(double)
        );

    public static readonly BindableProperty BorderStrokeProperty = BindableProperty.Create(
       nameof(BorderStroke),
       typeof(Brush),
       typeof(FreakyTextInputLayout),
       Brush.Black
       );

    public static readonly BindableProperty BorderCornerRadiusProperty = BindableProperty.Create(
       nameof(BorderCornerRadius),
       typeof(CornerRadius),
       typeof(FreakyTextInputLayout),
       default(CornerRadius)
       );

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
      nameof(FontSize),
      typeof(double),
      typeof(FreakyTextInputLayout),
      default(double)
       );

    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource),
        typeof(ImageSource),
        typeof(FreakyTextInputLayout),
        default(ImageSource)
        );

    public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
        nameof(ImageHeight),
        typeof(int),
        typeof(FreakyTextInputLayout),
        30);

    public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
        nameof(ImageWidth),
        typeof(int),
        typeof(FreakyTextInputLayout),
        30);

    public static readonly BindableProperty ImagePaddingProperty = BindableProperty.Create(
        nameof(ImagePadding),
        typeof(int),
        typeof(FreakyTextInputLayout),
        5);

    public static readonly BindableProperty ImageCommandProperty = BindableProperty.Create(
        nameof(ImageCommand),
        typeof(ICommand),
        typeof(FreakyTextInputLayout),
        default(ICommand));

    public static readonly BindableProperty ImageCommandParameterProperty = BindableProperty.Create(
        nameof(ImageCommandParameter),
        typeof(object),
        typeof(FreakyTextInputLayout),
        default(object));

    public static readonly BindableProperty AllowCopyPasteProperty = BindableProperty.Create(
        nameof(AllowCopyPaste),
        typeof(bool),
        typeof(FreakyTextInputLayout),
        true);

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
          nameof(TextColor),
          typeof(Color),
          typeof(FreakyTextInputLayout),
          Colors.Black);

    public static readonly BindableProperty UnderlineColorProperty = BindableProperty.Create(
         nameof(UnderlineColor),
         typeof(Color),
         typeof(FreakyTextInputLayout),
         Colors.Black);

    public static readonly BindableProperty UnderlineThicknessProperty = BindableProperty.Create(
         nameof(UnderlineThickness),
         typeof(double),
         typeof(FreakyTextInputLayout),
         default(double));

    public static readonly BindableProperty CharacterSpacingProperty = BindableProperty.Create(
         nameof(CharacterSpacing),
         typeof(double),
         typeof(FreakyTextInputLayout),
         default(double));

    public static readonly BindableProperty ClearButtonVisibilityProperty = BindableProperty.Create(
         nameof(ClearButtonVisibility),
         typeof(ClearButtonVisibility),
         typeof(FreakyTextInputLayout),
         Microsoft.Maui.ClearButtonVisibility.Never);

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
         nameof(FontAttributes),
         typeof(FontAttributes),
         typeof(FreakyTextInputLayout),
         Microsoft.Maui.Controls.FontAttributes.None);

    public static readonly BindableProperty CursorPositionProperty = BindableProperty.Create(
         nameof(CursorPosition),
         typeof(int),
         typeof(FreakyTextInputLayout));

    public static readonly BindableProperty FontAutoScalingEnabledProperty = BindableProperty.Create(
        nameof(FontAutoScalingEnabled),
        typeof(bool),
        typeof(FreakyTextInputLayout),
        true);

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(FreakyTextInputLayout));

    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
        nameof(HorizontalTextAlignment),
        typeof(TextAlignment),
        typeof(FreakyTextInputLayout));

    public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create(
       nameof(VerticalTextAlignment),
       typeof(TextAlignment),
       typeof(FreakyTextInputLayout));

    public static readonly BindableProperty IsTextPredictionEnabledProperty = BindableProperty.Create(
       nameof(IsTextPredictionEnabled),
       typeof(bool),
       typeof(FreakyTextInputLayout),
       true);

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
      nameof(ReturnCommand),
      typeof(ICommand),
      typeof(FreakyTextInputLayout));

    public static readonly BindableProperty ReturnCommandParameterProperty = BindableProperty.Create(
     nameof(ReturnCommandParameter),
     typeof(object),
     typeof(FreakyTextInputLayout));

    public static readonly BindableProperty SelectionLengthProperty = BindableProperty.Create(
     nameof(SelectionLength),
     typeof(int),
     typeof(FreakyTextInputLayout));

    public static readonly BindableProperty IsSpellCheckEnabledProperty = BindableProperty.Create(
     nameof(IsSpellCheckEnabled),
     typeof(bool),
     typeof(FreakyTextInputLayout));

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
     nameof(IsReadOnly),
     typeof(bool),
     typeof(FreakyTextInputLayout));

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
     nameof(MaxLength),
     typeof(int),
     typeof(FreakyTextInputLayout),
     int.MaxValue);

    public static readonly BindableProperty TextTransformProperty = BindableProperty.Create(
     nameof(TextTransform),
     typeof(TextTransform),
     typeof(FreakyTextInputLayout),
     Microsoft.Maui.TextTransform.Default);

    public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
     nameof(PlaceholderColor),
     typeof(Color),
     typeof(FreakyTextInputLayout),
     Colors.Black);

    public static readonly BindableProperty ControlBackgroundColorProperty = BindableProperty.Create(
        nameof(ControlBackgroundColor),
        typeof(Color),
        typeof(FreakyTextInputLayout),
        Colors.Transparent);


    public static readonly BindableProperty ImageAlignmentProperty = BindableProperty.Create(
           nameof(ImageAlignment),
           typeof(ImageAlignment),
           typeof(FreakyEntry),
           ImageAlignment.Right);


    /// <summary>
    /// Gets and Sets if your Entry allows Copy Paste. default is true!
    /// </summary>
    public bool AllowCopyPaste
    {
        get => (bool)GetValue(AllowCopyPasteProperty);
        set => SetValue(AllowCopyPasteProperty, value);
    }

    /// <summary>
    /// Command parameter for your Image tap command
    /// </summary>
    public object ImageCommandParameter
    {
        get => GetValue(ImageCommandParameterProperty);
        set => SetValue(ImageCommandParameterProperty, value);
    }

    /// <summary>
    /// A command that you can use to bind with your Image that you added to your Entry's ViewPort
    /// </summary>
    public ICommand ImageCommand
    {
        get => (ICommand)GetValue(ImageCommandProperty);
        set => SetValue(ImageCommandProperty, value);
    }

    /// <summary>
    /// Padding of the Image that you added to the ViewPort
    /// </summary>
    public int ImagePadding
    {
        get => (int)GetValue(ImagePaddingProperty);
        set => SetValue(ImagePaddingProperty, value);
    }

    /// <summary>
    /// Width of the Image in your ViewPort
    /// </summary>
    public int ImageWidth
    {
        get => (int)GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }

    /// <summary>
    /// Height of the Image in your ViewPort
    /// </summary>
    public int ImageHeight
    {
        get => (int)GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }

    /// <summary>
    /// An ImageSource that you want to add to your ViewPort
    /// </summary>
    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    /// <summary>
    /// Alignment for your Image's ViewPort, By default set to Right.
    /// </summary>
    public ImageAlignment ImageAlignment
    {
        get => (ImageAlignment)GetValue(ImageAlignmentProperty);
        set => SetValue(ImageAlignmentProperty, value);
    }

    /// <summary>
    /// Color of your Placeholder Label's background when border type is outlined
    /// </summary>
    public Color ControlBackgroundColor
    {
        get => (Color)GetValue(ControlBackgroundColorProperty);
        set => SetValue(ControlBackgroundColorProperty, value);
    }

    /// <summary>
    /// Color of your Placeholder Label
    /// </summary>
    public Color PlaceholderColor
    {
        get => (Color)GetValue(PlaceholderColorProperty);
        set => SetValue(PlaceholderColorProperty, value);
    }

    /// <summary>
    /// of type TextTransform, specifies the casing of the entered text.
    /// </summary>
    public TextTransform TextTransform
    {
        get => (TextTransform)GetValue(TextTransformProperty);
        set => SetValue(TextTransformProperty, value);
    }

    /// <summary>
    /// of type bool, defines whether the user should be prevented from modifying text. The default value of this property is false.
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>
    /// of type int, defines the maximum input length.
    /// </summary>
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    /// <summary>
    /// of type bool, controls whether spell checking is enabled.
    /// </summary>
    public bool IsSpellCheckEnabled
    {
        get => (bool)GetValue(IsSpellCheckEnabledProperty);
        set => SetValue(IsSpellCheckEnabledProperty, value);
    }

    /// <summary>
    /// of type int, represents the length of selected text within the <see cref="FreakyTextInputLayout".
    /// </summary>
    public int SelectionLength
    {
        get => (int)GetValue(SelectionLengthProperty);
        set => SetValue(SelectionLengthProperty, value);
    }

    /// <summary>
    /// of type object, specifies the parameter for the ReturnCommand
    /// </summary>
    public object ReturnCommandParameter
    {
        get => GetValue(ReturnCommandParameterProperty);
        set => SetValue(ReturnCommandParameterProperty, value);
    }

    /// <summary>
    /// of type ICommand, defines the command to be executed when the return key is pressed.
    /// </summary>
    public ICommand ReturnCommand
    {
        get => (ICommand)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    /// <summary>
    /// of type bool, controls whether text prediction and automatic text correction is enabled.
    /// </summary>
    public bool IsTextPredictionEnabled
    {
        get => (bool)GetValue(IsTextPredictionEnabledProperty);
        set => SetValue(IsTextPredictionEnabledProperty, value);
    }

    /// <summary>
    /// of type TextAlignment, defines the vertical alignment of the text.
    /// </summary>
    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    /// <summary>
    /// of type TextAlignment, defines the horizontal alignment of the text.
    /// </summary>
    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    /// <summary>
    /// of type string, defines the font family.
    /// </summary>
    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    /// <summary>
    /// of type bool, defines whether the text will reflect scaling preferences set in the operating system.
    /// The default value of this property is true.
    /// </summary>
    public bool FontAutoScalingEnabled
    {
        get => (bool)GetValue(FontAutoScalingEnabledProperty);
        set => SetValue(FontAutoScalingEnabledProperty, value);
    }

    /// <summary>
    /// of type int, defines the position of the cursor within the <see cref="FreakyTextInputLayout"/>.
    /// </summary>
    public int CursorPosition
    {
        get => (int)GetValue(CursorPositionProperty);
        set => SetValue(CursorPositionProperty, value);
    }

    /// <summary>
    /// of type FontAttributes, determines text style.
    /// </summary>
    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    /// <summary>
    /// of type ClearButtonVisibility, controls whether a clear button is displayed,
    /// which enables the user to clear the text.
    /// The default value of this property ensures that a clear button isn't displayed.
    /// </summary>
    public ClearButtonVisibility ClearButtonVisibility
    {
        get => (ClearButtonVisibility)GetValue(ClearButtonVisibilityProperty);
        set => SetValue(ClearButtonVisibilityProperty, value);
    }

    /// <summary>
    /// of type double, sets the spacing between characters in the entered text.
    /// </summary>
    public double CharacterSpacing
    {
        get => (double)GetValue(CharacterSpacingProperty);
        set => SetValue(CharacterSpacingProperty, value);
    }

    /// <summary>
    /// Thickness of the Underline of your <see cref="FreakyTextInputLayout"
    /// </summary>
    public double UnderlineThickness
    {
        get => (double)GetValue(UnderlineThicknessProperty);
        set => SetValue(UnderlineThicknessProperty, value);
    }

    /// <summary>
    /// Color of your <see cref="FreakyTextInputLayout" Underline
    /// </summary>
    public Color UnderlineColor
    {
        get => (Color)GetValue(UnderlineColorProperty);
        set => SetValue(UnderlineColorProperty, value);
    }

    /// <summary>
    /// of type Color, defines the color of the entered text.
    /// </summary>
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }


    /// <summary>
    /// of type double, defines the font size.
    /// </summary>
    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <summary>
    /// of type CornerRadius, and defines the Cornder Radius of your Border.
    /// </summary>
    public CornerRadius BorderCornerRadius
    {
        get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
        set => SetValue(BorderCornerRadiusProperty, value);
    }

    /// <summary>
    /// of type Brush, and defines the Stroke of your Border.
    /// </summary>
    public Brush BorderStroke
    {
        get => (Brush)GetValue(BorderStrokeProperty);
        set => SetValue(BorderStrokeProperty, value);
    }

    /// <summary>
    /// of type double, and defines the Thickness of the border stroke.
    /// </summary>
    public double BorderStrokeThickness
    {
        get => (double)GetValue(BorderStrokeThicknessProperty);
        set => SetValue(BorderStrokeThicknessProperty, value);
    }

    /// <summary>
    /// of type string, defines the text entered into the <see cref="FreakyTextInputLayout".
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// of type string, defines the text to become the placeholder for the <see cref="FreakyTextInputLayout".
    /// </summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    /// of type ReturnType, specifies the appearance of the return button.
    /// </summary>
    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }

    /// <summary>
    /// of type bool, specifies whether the <see cref="FreakyTextInputLayout" should visually obscure typed text.
    /// </summary>
    public bool IsPassword
    {
        get { return (bool)GetValue(IsPasswordProperty); }
        set { SetValue(IsPasswordProperty, value); }
    }

    /// <summary>
    /// of type Keyboard, specifies the virtual keyboard that's displayed when entering text.
    /// </summary>
    [System.ComponentModel.TypeConverter(typeof(Microsoft.Maui.Converters.KeyboardTypeConverter))]
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    #endregion

    public new void Focus()
    {
        if (IsEnabled)
        {
            EntryField.Focus();
        }
    }

    /// <summary>
    /// raised when the user finalizes text in the <see cref="FreakyTextInputLayout"/> with the return key.
    /// </summary>
    public event EventHandler Completed;

    /// <summary>
    ///  raised when the text in the <see cref="FreakyTextInputLayout"/> changes.
    ///  The <see cref="TextChangedEventArgs"/> object that accompanies the TextChanged event has NewTextValue and OldTextValue properties,
    ///  which specify the new and old text, respectively.
    /// </summary>
    public event EventHandler<TextChangedEventArgs> TextChanged;

    void Handle_Completed(System.Object sender, System.EventArgs e)
    {
    }

    void Handle_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
    }

    void Handle_Unfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
    }

    void EntryField_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
    }
}