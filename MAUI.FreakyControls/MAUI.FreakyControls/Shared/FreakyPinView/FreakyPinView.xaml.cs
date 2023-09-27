using System.Diagnostics;
using System.Windows.Input;

namespace Maui.FreakyControls;

public partial class FreakyPinView : ContentView
{
    #region Fields
    /// <summary>
    /// An event which is raised/invoked when PIN entry is completed
    /// This will help user to execute any code when entry completed
    /// </summary>
    public event EventHandler<PINCompletedEventArgs> PINEntryCompleted;
    #endregion

    #region Constructor and Initializations
    public FreakyPinView()
    {
        InitializeComponent();

        hiddenTextEntry.TextChanged += FreakyPinView_TextChanged;
        hiddenTextEntry.Focused += HiddenTextEntry_Focused;
        hiddenTextEntry.Unfocused += HiddenTextEntry_Unfocused;
        CreateControl();
    }

    private void HiddenTextEntry_Unfocused(object sender, FocusEventArgs e)
    {
        var pinBoxArray = PINBoxContainer.Children.Select(x => x as BoxTemplate).ToArray();

        for (int i = 0; i < PINLength; i++)
        {
            pinBoxArray[i].UnFocusAnimation();
        }
    }

    private void HiddenTextEntry_Focused(object sender, FocusEventArgs e)
    {
        var length = PINValue == null ? 0 : PINValue.Length;

        // When textbox is focused, Android brings cursor to the start of value, instead of end
        // To fix this issue, added this programatic cursor movement to the last when focused
        hiddenTextEntry.CursorPosition = length;

        var pinBoxArray = PINBoxContainer.Children.Select(x => x as BoxTemplate).ToArray();

        if (length == PINLength)
        {
            pinBoxArray[length - 1].FocusAnimation();
        }
        else
        {
            for (int i = 0; i < PINLength; i++)
            {
                if (i == length)
                {
                    pinBoxArray[i].FocusAnimation();
                }
                else
                {
                    pinBoxArray[i].UnFocusAnimation();
                }
            }
        }
    }



    ///// <summary>
    ///// Calling this, will bring up the soft keyboard, or will help focus the control
    ///// </summary>
    //public void FocusBox()
    //{
    //    boxTapGestureRecognizer?.Command?.Execute(null);
    //}
    #endregion

    #region Methods
    /// <summary>
    /// Initializes the UI for the FreakyPinView
    /// </summary>
    public void CreateControl()
    {
        hiddenTextEntry.MaxLength = PINLength;
        SetInputType(PINInputType);

        var count = PINBoxContainer.Children.Count;

        if (count < PINLength)
        {
            int newBoxesToAdd = PINLength - count;
            char[] pinCharsArray = PINValue.ToCharArray();

            for (int i = 1; i <= newBoxesToAdd; i++)
            {
                BoxTemplate boxTemplate = CreateBox();
                PINBoxContainer.Children.Add(boxTemplate);

                // When we assign PINValue in XAML directly, the Boxes outside the default length (4), will not get any value in it, eventhough we have assigned it in XAML
                // To correct this behavior, we have programatically assigned value to those Boxes which are added after the Default Length
                if (PINValue.Length >= PINLength)
                {
                    boxTemplate.SetValueWithAnimation(pinCharsArray[Constants.DefaultPINLength + i - 1]);
                }
            }
        }
        else if (count > PINLength)
        {
            int boxesToRemove = count - PINLength;
            for (int i = 1; i <= boxesToRemove; i++)
            {
                PINBoxContainer.Children.RemoveAt(PINBoxContainer.Children.Count - 1);
            }
        }
    }

    /// <summary>
    /// Creates the instance of one single PIN box UI
    /// </summary>
    /// <returns></returns>
    private BoxTemplate CreateBox(char? charValue = null)
    {
        BoxTemplate boxTemplate = new BoxTemplate();
        boxTemplate.HeightRequest = BoxSize;
        boxTemplate.WidthRequest = BoxSize;
        boxTemplate.Box.BackgroundColor = BoxBackgroundColor;
        boxTemplate.CharLabel.FontSize = BoxSize / 2;
        boxTemplate.BoxFocusColor = BoxFocusColor;
        boxTemplate.InputTransparent = true;
        boxTemplate.FocusAnimationType = BoxFocusAnimation;
        boxTemplate.SecureMode(IsPassword);
        boxTemplate.SetColor(Color, BoxBorderColor);
        boxTemplate.SetRadius(BoxShape);

        if (charValue.HasValue)
        {
            boxTemplate.SetValueWithAnimation(charValue.Value);
        }

        return boxTemplate;
    }

    #endregion

    #region Events
    /// <summary>
    /// Invokes when user type the PIN, Assignes value to PINValue property or Text changes in the hidden textbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FreakyPinView_TextChanged(object sender, TextChangedEventArgs e)
    {
        PINValue = e.NewTextValue;

        // To have some delay so that till the next execution all assigned values to the properties in XAML gets sets and we get the right value at the time after this delay
        // Otherwise due to sequence of calls, some properties gets their actual assigned value after the completion of this event
        // Also To have some delay, before invoking any Action, otherwise, (if) while navigation, it will be quick and you won't see your last entry / or animation.
        await Task.Delay(200);

        if (e.NewTextValue.Length >= PINLength)
        {
            // Dismiss the keyboard, once entry is completed up to the defined length and if AutoDismissKeyboard property is true 
            if (AutoDismissKeyboard)
            {
                (sender as Entry).Unfocus();
            }

            PINEntryCompleted?.Invoke(this, new PINCompletedEventArgs(PINValue));
            PINEntryCompletedCommand?.Execute(PINValue);
        }
    }
    #endregion

    /// <summary>
    /// Gets or Sets the PIN value.
    /// </summary>
    public string PINValue
    {
        get => (string)GetValue(PINValueProperty);
        set => SetValue(PINValueProperty, value);
    }

    public static readonly BindableProperty PINValueProperty =
       BindableProperty.Create(
           nameof(PINValue),
           typeof(string),
           typeof(FreakyPinView),
           string.Empty,
           defaultBindingMode: BindingMode.TwoWay,
           propertyChanged: PINValuePropertyChanged);

    private static async void PINValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        try
        {
            var control = (FreakyPinView)bindable;

            string newPIN = Convert.ToString(newValue);
            string oldPIN = Convert.ToString(oldValue);

            int newPINLength = newPIN.Length;
            int oldPINLength = oldPIN.Length;

            // If no any chars entered, return from here
            if (newPINLength == 0 && oldPINLength == 0)
            {
                return;
            }

            char[] newPINChars = newPIN.ToCharArray();

            control.hiddenTextEntry.Text = newPIN;
            var pinBoxArray = control.PINBoxContainer.Children.Select(x => x as BoxTemplate).ToArray();

            bool isPINEnteredProgramatically = (oldPINLength == 0 && newPINLength == control.PINLength) || newPINLength == oldPINLength;

            if (isPINEnteredProgramatically)
            {
                //Clear all Previous Entries, and then enter new one, to show proper Entry sequence animation
                for (int i = 0; i < control.PINLength; i++)
                {
                    pinBoxArray[i].ClearValueWithAnimation();
                }
            }

            for (int i = 0; i < control.PINLength; i++)
            {
                if (i < newPINLength)
                {
                    // If user sets PIN value programatically show a bit of Entry sequence animation
                    if (isPINEnteredProgramatically)
                    {
                        await Task.Delay(50);
                    }

                    pinBoxArray[i].SetValueWithAnimation(newPINChars[i]);
                }
                else
                {
                    if (pinBoxArray.Length >= control.PINLength)
                    {
                        pinBoxArray[i].ClearValueWithAnimation();
                        pinBoxArray[i].UnFocusAnimation();
                    }
                }
            }

            // Set or move Current Focus
            if (control.hiddenTextEntry.IsFocused)
            {
                if (newPINLength < control.PINLength)
                {
                    pinBoxArray[newPINLength].FocusAnimation();
                }
                // When while typing, if you reach to the last charecter, keep focus there (on last character Box), If PIN entry is focused
                else if (newPINLength == control.PINLength)
                {
                    pinBoxArray[newPINLength - 1].FocusAnimation();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    /// <summary>
    /// Gets or Sets the Length of the PIN.
    /// The number of PIN boxes will be layed out based on this Property.
    /// </summary>
    public int PINLength
    {
        get => (int)GetValue(PINLengthProperty);
        set => SetValue(PINLengthProperty, value);
    }

    public static readonly BindableProperty PINLengthProperty =
      BindableProperty.Create(
          nameof(PINLength),
          typeof(int),
          typeof(FreakyPinView),
          Constants.DefaultPINLength,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: PINLengthPropertyChanged);

    private static void PINLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((int)newValue <= 0)
        {
            return;
        }

       ((FreakyPinView)bindable).CreateControl();
    }

    /// <summary>
    /// Gets or Sets the Input Type (Keyboard Type) of the PIN Box from InputKeyboardType enum:
    /// 
    ///     Numeric,
    ///     AlphaNumeric
    ///     
    /// </summary>
    public InputKeyboardType PINInputType
    {
        get => (InputKeyboardType)GetValue(PINInputTypeProperty);
        set => SetValue(PINInputTypeProperty, value);
    }

    public static readonly BindableProperty PINInputTypeProperty =
       BindableProperty.Create(
           nameof(PINInputType),
           typeof(InputKeyboardType),
           typeof(FreakyPinView),
            InputKeyboardType.Numeric,
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: PINInputTypePropertyChanged);

    private static void PINInputTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyPinView)bindable);
        control.SetInputType((InputKeyboardType)newValue);
    }

    /// <summary>
    /// Sets the Keyboard Type as per the InputKeyboardType Bindable Property
    /// </summary>
    /// <param name="inputKeyboardType"></param>
    public void SetInputType(InputKeyboardType inputKeyboardType)
    {
        if (inputKeyboardType == InputKeyboardType.Numeric)
        {
            hiddenTextEntry.Keyboard = Keyboard.Numeric;
        }
        else if (inputKeyboardType == InputKeyboardType.AlphaNumeric)
        {
            // Keyboard.Create(0); : To remove the Hints on top of Keyboard, while typing
            hiddenTextEntry.Keyboard = Keyboard.Create(0);
        }
    }

    /// <summary>
    /// A Command to Bind and invoked when PIN Entry is completed
    /// </summary>
    public ICommand PINEntryCompletedCommand
    {
        get { return (ICommand)GetValue(PINEntryCompletedCommandProperty); }
        set { SetValue(PINEntryCompletedCommandProperty, value); }
    }

    public static readonly BindableProperty PINEntryCompletedCommandProperty =
       BindableProperty.Create(
          nameof(PINEntryCompletedCommand),
          typeof(ICommand),
          typeof(FreakyPinView),
          null);

    /// <summary>
    /// Set true if you dont want to show input value charecters, false otherwise
    /// True will show Dots inside box while typing
    /// False will show actual input value
    /// </summary>
    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public static readonly BindableProperty IsPasswordProperty =
      BindableProperty.Create(
          nameof(IsPassword),
          typeof(bool),
          typeof(FreakyPinView),
          true,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: IsPasswordPropertyChanged);

    private static void IsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyPinView)bindable);
        foreach (var x in control.PINBoxContainer.Children)
        {
            var boxTemplate = (BoxTemplate)x;
            boxTemplate.SecureMode((bool)newValue);
        }
    }

    /// <summary>
    /// Gets or Sets the Color of the PIN Boxes.
    /// Generally the Color of the Border and Dot inside Box
    /// </summary>
    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public static readonly BindableProperty ColorProperty =
      BindableProperty.Create(
          nameof(Color),
          typeof(Color),
          typeof(FreakyPinView),
          Colors.Black,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: ColorPropertyChanged);

    private static void ColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FreakyPinView)bindable;
        foreach (var x in control.PINBoxContainer.Children)
        {
            var boxTemplate = (BoxTemplate)x;
            boxTemplate.SetColor(color: (Color)newValue, boxBorderColor: control.BoxBorderColor);
        }
    }

    /// <summary>
    /// Gets or Sets the Space among the PIN Boxes
    /// </summary>
    public double BoxSpacing
    {
        get => (double)GetValue(BoxSpacingProperty);
        set => SetValue(BoxSpacingProperty, value);
    }

    public static readonly BindableProperty BoxSpacingProperty =
      BindableProperty.Create(
          nameof(BoxSpacing),
          typeof(double),
          typeof(FreakyPinView),
          Constants.DefaultBoxSpacing,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: BoxSpacingPropertyChanged);

    private static void BoxSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((double)newValue < 0)
        {
            return;
        }

        var control = ((FreakyPinView)bindable);

        control.PINBoxContainer.Spacing = (double)newValue;
    }

    /// <summary>
    /// Gets or Sets the Height / Width of each PIN Box.
    /// Please try to give Even number size to get the proper UI.
    /// Please, try to give such size that all PIN boxes fit properly in your device's screen
    /// Providing larger size, can shrink the Boxes if there is no more room to fit them on screen
    /// </summary>
    public double BoxSize
    {
        get => (double)GetValue(BoxSizeProperty);
        set => SetValue(BoxSizeProperty, value);
    }

    public static readonly BindableProperty BoxSizeProperty =
      BindableProperty.Create(
          nameof(BoxSize),
          typeof(double),
          typeof(FreakyPinView),
          Constants.DefaultBoxSize,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: BoxSizePropertyChanged);

    private static void BoxSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((double)newValue < 0)
        {
            return;
        }

        var control = ((FreakyPinView)bindable);

        foreach (var x in control.PINBoxContainer.Children)
        {
            var boxTemplate = (BoxTemplate)x;
            boxTemplate.HeightRequest = (double)newValue;
            boxTemplate.WidthRequest = (double)newValue;
            boxTemplate.CharLabel.FontSize = ((double)newValue / 2);
            boxTemplate.SetRadius(control.BoxShape);
        }
    }

    /// <summary>
    /// Gets or Sets the Shape of the PIN Box from BoxShapeType enum:
    /// 
    ///     Circle,
    ///     RoundCorner
    ///     Squere
    ///     
    /// </summary>
    public BoxShapeType BoxShape
    {
        get => (BoxShapeType)GetValue(BoxShapeProperty);
        set => SetValue(BoxShapeProperty, value);
    }

    public static readonly BindableProperty BoxShapeProperty =
       BindableProperty.Create(
           nameof(BoxShape),
           typeof(BoxShapeType),
           typeof(FreakyPinView),
            BoxShapeType.Circle,
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: BoxShapePropertyChanged);

    private static void BoxShapePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyPinView)bindable);
        foreach (var x in control.PINBoxContainer.Children)
        {
            var boxTemplate = (BoxTemplate)x;
            boxTemplate.SetRadius((BoxShapeType)newValue);
        }
    }

    /// <summary>
    /// Gets or Sets the FocusIndicatorColor of the PIN Boxes.
    /// </summary>
    public Color BoxFocusColor
    {
        get => (Color)GetValue(BoxFocusColorProperty);
        set => SetValue(BoxFocusColorProperty, value);
    }

    public static readonly BindableProperty BoxFocusColorProperty =
      BindableProperty.Create(
          nameof(BoxFocusColor),
          typeof(Color),
          typeof(FreakyPinView),
          Colors.Black,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: BoxFocusColorPropertyChanged);

    private static void BoxFocusColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FreakyPinView)bindable;
        foreach (var x in control.PINBoxContainer.Children)
        { 
            var boxTemplate = (BoxTemplate)x;
            boxTemplate.BoxFocusColor = (Color)newValue;
        }
    }

    /// <summary>
    /// Gets or Sets the Focus Animation of the PIN Box from FocusAnimationType enum:
    /// 
    ///     None (Default),
    ///     ZoomInOut
    ///     ScaleUp
    ///     
    /// </summary>
    public FocusAnimationType BoxFocusAnimation
    {
        get => (FocusAnimationType)GetValue(BoxFocusAnimationProperty);
        set => SetValue(BoxFocusAnimationProperty, value);
    }

    public static readonly BindableProperty BoxFocusAnimationProperty =
       BindableProperty.Create(
           nameof(BoxFocusAnimation),
           typeof(FocusAnimationType),
           typeof(FreakyPinView),
            FocusAnimationType.None,
           defaultBindingMode: BindingMode.OneWay,
           propertyChanged: BoxFocusAnimationPropertyChanged);

    private static void BoxFocusAnimationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = ((FreakyPinView)bindable);
        foreach (var x in control.PINBoxContainer.Children)
        {
            var boxTemplate = (BoxTemplate)x;
            boxTemplate.FocusAnimationType = (FocusAnimationType)newValue;
        }
    }

    /// <summary>
    /// Gets or Sets the Border color of the PIN Box.
    /// If you do not set this Property, By default it will use the "Color" property for BoxBorderColor 
    /// </summary>
    public Color BoxBorderColor
    {
        get => (Color)GetValue(BoxBorderColorProperty);
        set => SetValue(BoxBorderColorProperty, value);
    }

    public static readonly BindableProperty BoxBorderColorProperty =
      BindableProperty.Create(
          nameof(BoxBorderColor),
          typeof(Color),
          typeof(FreakyPinView),
          Colors.Black,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: BoxBorderColorPropertyChanged);

    private static void BoxBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FreakyPinView)bindable;

        // Apply the BoxBorderColor only if it is different then the value in "Color" Property
        if (control.Color != (Color)newValue)
        {
            foreach (var x in control.PINBoxContainer.Children)
            {
                var boxTemplate = (BoxTemplate)x;
                boxTemplate.SetColor(color: control.Color, boxBorderColor: (Color)newValue);
            }
        }
    }

    /// <summary>
    /// Gets or Sets the Background color of the PIN Box.
    /// </summary>
    public Color BoxBackgroundColor
    {
        get => (Color)GetValue(BoxBackgroundColorProperty);
        set => SetValue(BoxBackgroundColorProperty, value);
    }

    public static readonly BindableProperty BoxBackgroundColorProperty =
      BindableProperty.Create(
          nameof(BoxBackgroundColor),
          typeof(Color),
          typeof(FreakyPinView),
          Colors.Transparent,
          defaultBindingMode: BindingMode.OneWay,
          propertyChanged: BoxBackgroundColorPropertyChanged);

    private static void BoxBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FreakyPinView)bindable;
        foreach (var x in control.PINBoxContainer.Children)
        {
            var boxTemplate = (BoxTemplate)x;
            boxTemplate.Box.BackgroundColor = (Color)newValue;
        }
    }

    /// <summary>
    /// Set true if you want to dismiss the soft keyboard, when PIN entry is completed
    /// </summary>
    public bool AutoDismissKeyboard
    {
        get => (bool)GetValue(AutoDismissKeyboardProperty);
        set => SetValue(AutoDismissKeyboardProperty, value);
    }

    public static readonly BindableProperty AutoDismissKeyboardProperty =
      BindableProperty.Create(
          nameof(AutoDismissKeyboard),
          typeof(bool),
          typeof(FreakyPinView),
          false,
          defaultBindingMode: BindingMode.OneWay);

    void TapGestureRecognizer_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        hiddenTextEntry.Focus();
    }
}

internal class BoxTemplate : Frame
{
    private string _inputChar;
    private Color _color;
    private Color _boxBorderColor;

    public FocusAnimationType FocusAnimationType { get; set; }
    public Color BoxFocusColor { get; set; }

    public Frame Box { get { return this; } }
    public Frame Dot { get; } = null;
    public Label CharLabel { get; } = null;

    public BoxTemplate()
    {
        Padding = 0;
        BackgroundColor = Constants.DefaultBoxBackgroundColor;
        BorderColor = Constants.DefaultColor;
        CornerRadius = (float)Constants.DefaultBoxSize / 2;
        HasShadow = false;
        HeightRequest = WidthRequest = Constants.DefaultBoxSize;
        VerticalOptions = LayoutOptions.Center;

        Dot = new Frame()
        {
            BackgroundColor = Constants.DefaultColor,
            CornerRadius = (float)Constants.DefaultDotSize / 2,
            HeightRequest = Constants.DefaultDotSize,
            WidthRequest = Constants.DefaultDotSize,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Scale = 0,
            Padding = 0,
            HasShadow = false,
        };

        CharLabel = new Label()
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Constants.DefaultColor,
            FontAttributes = FontAttributes.Bold,
            VerticalTextAlignment = TextAlignment.Center,
            Scale = 0,
        };

        Content = Dot;
    }

    private void GrowAnimation()
    {
        Content.ScaleTo(1.0, 100);
    }

    private void ShrinkAnimation()
    {
        Content.ScaleTo(0, 100);
    }

    /// <summary>
    /// Sets the Color of Border, Dot, Input CharLabel
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color, Color boxBorderColor)
    {
        _color = color;
        _boxBorderColor = boxBorderColor;

        SetBorderColor();

        Dot.BackgroundColor = color;
        CharLabel.TextColor = color;
    }

    /// <summary>
    /// Applies the Corner Radius to the PIN Box based on the ShapeType
    /// </summary>
    /// <param name="shapeType"></param>
    public void SetRadius(BoxShapeType shapeType)
    {
        if (shapeType == BoxShapeType.Circle)
        {
            CornerRadius = (float)HeightRequest / 2;
        }
        else if (shapeType == BoxShapeType.Squere)
        {
            CornerRadius = 0;
        }
        else if (shapeType == BoxShapeType.RoundCorner)
        {
            CornerRadius = 10;
        }
    }

    /// <summary>
    /// Method sets the visibility of Input Characters or Dots.
    /// IsPassword = True  : Displays Dots
    /// IsPassword = False : Displays Chars
    /// </summary>
    /// <param name="isPassword"></param>
    public void SecureMode(bool isPassword)
    {
        if (isPassword)
        {
            Content = Dot;
        }
        else
        {
            Content = CharLabel;
        }

        if (!string.IsNullOrEmpty(_inputChar))
        {
            GrowAnimation();
        }
        else
        {
            ShrinkAnimation();
        }
    }

    /// <summary>
    /// Clears the input value along with showing the Clear value Animation
    /// </summary>
    /// <returns></returns>
    public void ClearValueWithAnimation()
    {
        _inputChar = null;
        ShrinkAnimation();
    }

    /// <summary>
    /// Sets the input value along with showing the Set value animation
    /// </summary>
    /// <param name="inputChar"></param>
    /// <returns></returns>
    public void SetValueWithAnimation(char inputChar)
    {
        UnFocusAnimation();

        CharLabel.Text = inputChar.ToString();
        _inputChar = inputChar.ToString();
        GrowAnimation();
    }

    // Sets the focus indication color
    public async void FocusAnimation()
    {
        BorderColor = BoxFocusColor;

        if (FocusAnimationType == FocusAnimationType.ZoomInOut)
        {
            await this.ScaleTo(1.2, 100);
            await this.ScaleTo(1, 100);
        }
        else if (FocusAnimationType == FocusAnimationType.ScaleUp)
        {
            await this.ScaleTo(1.2, 100);
        }
    }

    // Removes the focusindication color and set back to original
    public void UnFocusAnimation()
    {
        SetBorderColor();
        this.ScaleTo(1, 100);
    }

    private void SetBorderColor()
    {
        BorderColor = _boxBorderColor == Colors.Black ? _color : _boxBorderColor;
    }
}

public class PINCompletedEventArgs : FreakyEventArgs
{
    public string PIN { get; set; }

    public PINCompletedEventArgs(string pin)
    {
        this.PIN = pin;
    }
}

public enum InputKeyboardType
{
    Numeric,
    AlphaNumeric
}

public enum FocusAnimationType
{
    None,
    ZoomInOut,
    ScaleUp,
}

internal static class Constants
{
    public const double DefaultBoxSize = 50.0;
    public const double DefaultDotSize = 20.0;
    public const double DefaultBoxSpacing = 5.0;
    public const int DefaultPINLength = 4;


    public static Color DefaultColor = Colors.Black;
    public static Color DefaultBoxBackgroundColor = Colors.White;
}

public enum BoxShapeType
{
    Circle,
    Squere,
    RoundCorner,
}