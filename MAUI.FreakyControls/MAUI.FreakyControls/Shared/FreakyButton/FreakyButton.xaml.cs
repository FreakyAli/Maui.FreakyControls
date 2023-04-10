using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Maui.FreakyControls.Shared.Enums;
using Microsoft.Maui.Layouts;

namespace Maui.FreakyControls;

public partial class FreakyButton : ContentView
{
    ButtonMode mode;
    bool userChangedPadding;
    bool userChangedIconPadding;

    #region Bindable Properties

    // Foreground and Background Properties

    public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(FreakyButton), Colors.Transparent);
    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public static readonly BindableProperty HighlightBackgroundColorProperty = BindableProperty.Create(nameof(HighlightBackgroundColor), typeof(Color), typeof(FreakyButton), Colors.Transparent);
    public Color HighlightBackgroundColor
    {
        get => (Color)GetValue(HighlightBackgroundColorProperty);
        set => SetValue(HighlightBackgroundColorProperty, value);
    }

    public static readonly BindableProperty ForegroundColorProperty = BindableProperty.Create(nameof(ForegroundColor), typeof(Color), typeof(FreakyButton));
    public Color ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    public static readonly BindableProperty HighlightForegroundColorProperty = BindableProperty.Create(nameof(HighlightForegroundColor), typeof(Color), typeof(FreakyButton), Colors.White);
    public Color HighlightForegroundColor
    {
        get => (Color)GetValue(HighlightForegroundColorProperty);
        set => SetValue(HighlightForegroundColorProperty, value);
    }

    // Border Properties

    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(FreakyButton), Colors.Transparent);
    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public static readonly BindableProperty HighlightBorderColorProperty = BindableProperty.Create(nameof(HighlightBorderColor), typeof(Color), typeof(FreakyButton), Colors.Transparent);
    public Color HighlightBorderColor
    {
        get => (Color)GetValue(HighlightBorderColorProperty);
        set => SetValue(HighlightBorderColorProperty, value);
    }

    public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(Thickness), typeof(FreakyButton), new Thickness(0));
    public Thickness BorderThickness
    {
        get => (Thickness)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(FreakyButton), 0f);
    public float CornerRadius
    {
        get => (float)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }
    public float InnerCornerRadius { get; private set; }

    // Icon Properties

    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(FreakyButton), null);
    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly BindableProperty IconOrientationProperty = BindableProperty.Create(nameof(IconOrientation), typeof(IconOrientation), typeof(FreakyButton), IconOrientation.Left);
    public IconOrientation IconOrientation
    {
        get => (IconOrientation)GetValue(IconOrientationProperty);
        set => SetValue(IconOrientationProperty, value);
    }

    public static readonly BindableProperty IconTintEnabledProperty = BindableProperty.Create(nameof(IconTintEnabled), typeof(bool), typeof(FreakyButton), true);
    public bool IconTintEnabled
    {
        get => (bool)GetValue(IconTintEnabledProperty);
        set => SetValue(IconTintEnabledProperty, value);
    }

    // Text Properties

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FreakyButton), string.Empty);
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(FreakyButton));
    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(FreakyButton), (FontAttributes)Label.FontAttributesProperty.DefaultValue);
    public FontAttributes FontAttributes
    {
        get => (FontAttributes)GetValue(FontAttributesProperty);
        set => SetValue(FontAttributesProperty, value);
    }

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(FreakyButton), (string)Label.FontFamilyProperty.DefaultValue);
    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public static readonly BindableProperty MaxLinesProperty = BindableProperty.Create(nameof(MaxLines), typeof(int), typeof(FreakyButton), 1);
    public int MaxLines
    {
        get => (int)GetValue(MaxLinesProperty);
        set => SetValue(MaxLinesProperty, value);
    }

    // Toggle Properties

    public static readonly BindableProperty ToggleModeProperty = BindableProperty.Create(nameof(ToggleMode), typeof(bool), typeof(FreakyButton), false);
    public bool ToggleMode
    {
        get => (bool)GetValue(ToggleModeProperty);
        set => SetValue(ToggleModeProperty, value);
    }

    public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(FreakyButton), false, BindingMode.TwoWay);
    public bool IsToggled
    {
        get => (bool)GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
    }

    // Other Properties

    public static readonly new BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(FreakyButton), new Thickness(-1));
    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    public static readonly BindableProperty IconPaddingProperty = BindableProperty.Create(nameof(IconPadding), typeof(Thickness), typeof(FreakyButton), new Thickness(-1));
    public Thickness IconPadding
    {
        get => (Thickness)GetValue(IconPaddingProperty);
        set => SetValue(IconPaddingProperty, value);
    }

    public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(FreakyButton), false);
    public bool HasShadow
    {
        get => (bool)GetValue(HasShadowProperty);
        set => SetValue(HasShadowProperty, value);
    }

    public static readonly BindableProperty AccessibleNameProperty = BindableProperty.Create(nameof(AccessibleName), typeof(string), typeof(FreakyButton), null);
    public string AccessibleName
    {
        get => (string)GetValue(AccessibleNameProperty);
        set => SetValue(AccessibleNameProperty, value);
    }


    #endregion

    #region Commands

    public static readonly BindableProperty ClickedCommandProperty = BindableProperty.Create(nameof(ClickedCommand), typeof(ICommand), typeof(FreakyButton), null, propertyChanged: (bindable, oldValue, newValue) => ((FreakyButton)bindable).OnClickOrTouchedDownCommandPropertyChanged());
    public ICommand ClickedCommand
    {
        get => (ICommand)GetValue(ClickedCommandProperty);
        set => SetValue(ClickedCommandProperty, value);
    }

    public static readonly BindableProperty ClickedCommandParameterProperty = BindableProperty.Create(nameof(ClickedCommandParameter), typeof(object), typeof(FreakyButton), null, propertyChanged: (bindable, oldValue, newValie) => ((FreakyButton)bindable).CommandCanExecuteChanged(bindable, EventArgs.Empty));
    public object ClickedCommandParameter
    {
        get => GetValue(ClickedCommandParameterProperty);
        set => SetValue(ClickedCommandParameterProperty, value);
    }

    public static BindableProperty TouchedDownCommandProperty = BindableProperty.Create(nameof(TouchedDownCommand), typeof(ICommand), typeof(FreakyButton), null);
    public ICommand TouchedDownCommand
    {
        get { return (ICommand)GetValue(TouchedDownCommandProperty); }
        set { SetValue(TouchedDownCommandProperty, value); }
    }

    public static readonly BindableProperty TouchedDownCommandParameterProperty = BindableProperty.Create(nameof(TouchedDownCommandParameter), typeof(object), typeof(FreakyButton), null, propertyChanged: (bindable, oldValue, newValie) => ((FreakyButton)bindable).CommandCanExecuteChanged(bindable, EventArgs.Empty));
    public object TouchedDownCommandParameter
    {
        get => GetValue(TouchedDownCommandParameterProperty);
        set => SetValue(TouchedDownCommandParameterProperty, value);
    }

    public static BindableProperty TouchedUpCommandProperty = BindableProperty.Create(nameof(TouchedUpCommand), typeof(ICommand), typeof(FreakyButton), null);
    public ICommand TouchedUpCommand
    {
        get => (ICommand)GetValue(TouchedUpCommandProperty);
        set => SetValue(TouchedUpCommandProperty, value);
    }

    public static readonly BindableProperty TouchedUpCommandParameterProperty = BindableProperty.Create(nameof(TouchedUpCommandParameter), typeof(object), typeof(FreakyButton), null, propertyChanged: (bindable, oldValue, newValie) => ((FreakyButton)bindable).CommandCanExecuteChanged(bindable, EventArgs.Empty));
    public object TouchedUpCommandParameter
    {
        get => GetValue(TouchedUpCommandParameterProperty);
        set => SetValue(TouchedUpCommandParameterProperty, value);
    }

    public static BindableProperty TouchCanceledCommandProperty = BindableProperty.Create(nameof(TouchCanceledCommand), typeof(ICommand), typeof(FreakyButton), null);
    public ICommand TouchCanceledCommand
    {
        get => (ICommand)GetValue(TouchCanceledCommandProperty);
        set => SetValue(TouchCanceledCommandProperty, value);
    }

    public static readonly BindableProperty TouchCanceledCommandParameterProperty = BindableProperty.Create(nameof(TouchCanceledCommandParameter), typeof(object), typeof(FreakyButton), null, propertyChanged: (bindable, oldValue, newValie) => ((FreakyButton)bindable).CommandCanExecuteChanged(bindable, EventArgs.Empty));
    public object TouchCanceledCommandParameter
    {
        get => GetValue(TouchedUpCommandParameterProperty);
        set => SetValue(TouchedUpCommandParameterProperty, value);
    }

    #endregion

    #region Events

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        if (propertyName == PaddingProperty.PropertyName)
        {
            userChangedPadding = true;
            SetButtonMode();
        }
        else if (propertyName == IconPaddingProperty.PropertyName)
        {
            userChangedIconPadding = true;
            SetButtonMode();
        }
        else if (propertyName == IsEnabledProperty.PropertyName)
        {
            // Set Opacity based on IsEnabled
            Opacity = IsEnabled ? 1 : 0.5;
        }
        else if (propertyName == IconTintEnabledProperty.PropertyName)
        {
            Highlight(ToggleMode && IsToggled);
        }
        else if (propertyName == IconProperty.PropertyName || propertyName == ForegroundColorProperty.PropertyName)
        {
            SetButtonMode();

            // Make sure, that Icon Source is set manually, as Binding is too slow sometimes
            if (ButtonIcon != null)
            {
                ButtonIcon.Source = Icon;
                ColorIcon(ForegroundColor);

            }
        }
        if (propertyName == IsToggledProperty.PropertyName)
        {
            if (ToggleMode)
            {
                Highlight(IsToggled);
            }
        }
        else if (
            propertyName == TextProperty.PropertyName ||
            propertyName == IconOrientationProperty.PropertyName ||
            propertyName == BorderThicknessProperty.PropertyName ||
            propertyName == FontSizeProperty.PropertyName ||
            propertyName == HasShadowProperty.PropertyName ||
            propertyName == BackgroundColorProperty.PropertyName ||
            propertyName == ToggleModeProperty.PropertyName)
        {
            SetButtonMode();
        }

        base.OnPropertyChanged(propertyName);
    }

    void OnClickOrTouchedDownCommandPropertyChanged()
    {
        if (ClickedCommand != null)
            ClickedCommand.CanExecuteChanged += CommandCanExecuteChanged;

        if (TouchedDownCommand != null)
            TouchedDownCommand.CanExecuteChanged += CommandCanExecuteChanged;

        CommandCanExecuteChanged(this, EventArgs.Empty);
    }

    void CommandCanExecuteChanged(object sender, EventArgs e)
    {
        // Define IsEnabled state
        var canExecuteClick = ClickedCommand?.CanExecute(ClickedCommandParameter);
        var canExecuteTouchedDown = TouchedDownCommand?.CanExecute(TouchedDownCommandParameter);

        if (canExecuteClick != null && canExecuteTouchedDown != null)
            IsEnabled = canExecuteClick == true && canExecuteTouchedDown == true;
        else
            IsEnabled = canExecuteClick == true || canExecuteTouchedDown == true;
    }

    protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
    {
        // Unsubscribe from command events when Command changes
        if (propertyName == ClickedCommandProperty.PropertyName && ClickedCommand != null)
            ClickedCommand.CanExecuteChanged -= CommandCanExecuteChanged;
        if (propertyName == TouchedDownCommandProperty.PropertyName && TouchedDownCommand != null)
            TouchedDownCommand.CanExecuteChanged -= CommandCanExecuteChanged;
        if (propertyName == TouchedUpCommandProperty.PropertyName && TouchedUpCommand != null)
            TouchedUpCommand.CanExecuteChanged -= CommandCanExecuteChanged;
        if (propertyName == TouchCanceledCommandProperty.PropertyName && TouchCanceledCommand != null)
            TouchCanceledCommand.CanExecuteChanged -= CommandCanExecuteChanged;

        base.OnPropertyChanging(propertyName);
    }

    #endregion

    void SetButtonMode()
    {
        if (Icon != null && Text.Length > 0)
        {
            mode = ButtonMode.IconWithText;
        }
        else if (Icon != null && Text.Length == 0)
        {
            mode = ButtonMode.IconOnly;
        }
        else if (Icon == null && Text.Length > 0)
        {
            mode = ButtonMode.TextOnly;
        }

        // Set Accessibility Name
        AutomationProperties.SetName(Border, AccessibleName ?? Text);

        if (ButtonIcon == null || ButtonText == null)
            return;

        switch (mode)
        {
            case ButtonMode.IconOnly:

                // Configure Container
                //ContainerContent.HorizontalOptions = LayoutOptions.Fill;
                Grid.SetColumnSpan(ButtonIcon, 2);
                Grid.SetColumn(ButtonText, 1);

                // Set Visibilities
                ButtonText.IsVisible = false;
                ButtonIcon.IsVisible = true;

                // Adjust Default Padding
                if (!userChangedPadding)
                {
                    Padding = new Thickness(HeightRequest * .3, HeightRequest * .3);
                    userChangedPadding = false; // Set this back to false, as the above line triggers OnPropertyChanged 
                }

                break;

            case ButtonMode.IconWithText:

                // Configure Container
                switch (IconOrientation)
                {
                    case IconOrientation.Top:
                        ContainerContent.Direction = FlexDirection.Column;
                        break;

                    case IconOrientation.Left:
                        ContainerContent.Direction = FlexDirection.Row;
                        break;

                    case IconOrientation.Right:
                        ContainerContent.Direction = FlexDirection.RowReverse;
                        break;
                }

                // Set Visibilities
                ButtonText.IsVisible = true;
                ButtonIcon.IsVisible = true;

                // Adjust Default Padding
                if (!userChangedPadding)
                {
                    Padding = new Thickness(HeightRequest * .1, HeightRequest * .3);
                    userChangedPadding = false; // Set this back to false, as the above line triggers OnPropertyChanged 
                }
                if (!userChangedIconPadding)
                {
                    switch (IconOrientation)
                    {
                        case IconOrientation.Top: IconPadding = new Thickness(0, 0, 0, 6); break;
                        case IconOrientation.Left: IconPadding = new Thickness(0, 0, 6, 0); break;
                        case IconOrientation.Right: IconPadding = new Thickness(6, 0, 0, 0); break;
                    }
                    userChangedIconPadding = false; // Set this back to false, as the above line triggers OnPropertyChanged 
                }

                break;

            case ButtonMode.TextOnly:

                // Configure Container
                //ContainerContent.HorizontalOptions = LayoutOptions.FillAndExpand;
                Grid.SetColumnSpan(ButtonIcon, 1);
                Grid.SetColumnSpan(ButtonText, 2);
                Grid.SetColumn(ButtonText, 0);

                // Set Visibilities
                ButtonText.IsVisible = true;
                ButtonIcon.IsVisible = false;

                // Adjust Default Padding
                if (!userChangedPadding)
                {
                    Padding = new Thickness(20, 0);
                    userChangedPadding = false; // Set this back to false, as the above line triggers OnPropertyChanged 
                }

                break;
        }

        // HACK: Horrible Hack, that makes the Xamarin.Forms Previewer work, who seems to give up some Binding support
        // since Xamarin.Forms 2.5.1
        try
        {
            Border.BackgroundColor = BorderColor;
            Border.CornerRadius = CornerRadius;
            Border.Padding = BorderThickness;
            Border.HasShadow = HasShadow;
            Container.BackgroundColor = BackgroundColor;
            Container.CornerRadius = InnerCornerRadius;
            ContainerContent.Margin = Padding;
            ButtonText.Text = Text;
            ButtonText.FontSize = FontSize;
            ButtonText.FontAttributes = FontAttributes;
            ButtonText.FontFamily = FontFamily;
            ButtonText.TextColor = ForegroundColor;
            ButtonText.MaxLines = MaxLines;
            ButtonIcon.Margin = IconPadding;
        }
        catch (NullReferenceException)
        {

        }

        if (ToggleMode)
        {
            Highlight(IsToggled);
        }

        // Calculate inner corner radius
        // Use the outer radius minus the max thickness of a single direction
        InnerCornerRadius = Math.Max(0, CornerRadius - (int)Math.Max(Math.Max(BorderThickness.Left, BorderThickness.Top), Math.Max(BorderThickness.Right, BorderThickness.Bottom)));
        Container.CornerRadius = InnerCornerRadius;

        //ColorIcon(ForegroundColor);
    }

    public event EventHandler<EventArgs> TouchedDown;
    public event EventHandler<EventArgs> TouchedUp;
    public event EventHandler<EventArgs> TouchCanceled;
    public event EventHandler<EventArgs> Clicked;
    public event EventHandler<ToggledEventArgs> Toggled;

    public FreakyButton()
    {
        InitializeComponent();

        TouchRecognizer.TouchDown += OnTouchDown;
        TouchRecognizer.TouchUp += OnTouchUp;
        TouchRecognizer.TouchCanceled += OnTouchCanceled;
        SizeChanged += FreakyButton_SizeChanged;
    }

    void FreakyButton_SizeChanged(object sender, EventArgs e)
    {
        // HACK: Needs to be called to not make the Designer do stupid things
        SetButtonMode();
        ColorIcon(ForegroundColor);
    }

    void OnTouchDown()
    {
        if (IsEnabled)
        {
            TouchedDown?.Invoke(this, EventArgs.Empty);
            TouchedDownCommand?.Execute(TouchedDownCommandParameter);

            Highlight(true);
        }
    }

    void OnTouchUp()
    {
        if (IsEnabled)
        {
            TouchedUp?.Invoke(this, EventArgs.Empty);
            TouchedUpCommand?.Execute(TouchedUpCommandParameter);
            Clicked?.Invoke(this, EventArgs.Empty);
            ClickedCommand?.Execute(ClickedCommandParameter);

            if (ToggleMode)
            {
                IsToggled = !IsToggled;
                Toggled?.Invoke(this, new ToggledEventArgs(IsToggled));

                Highlight(IsToggled);
            }
            else
            {
                Highlight(false);
            }
        }
    }

    void OnTouchCanceled()
    {
        if (IsEnabled)
        {
            TouchCanceled?.Invoke(this, EventArgs.Empty);
            TouchCanceledCommand?.Execute(TouchCanceledCommandParameter);

            if (ToggleMode)
                Highlight(IsToggled);
            else
                Highlight(false);
        }
    }

    void ColorIcon(Color color)
    {
        // Attach Color Overlay Effect
        ButtonIcon.Effects.Clear();

        if (IconTintEnabled)
        {
            ButtonIcon.Effects.Add(new ColorOverlayEffect { Color = color });
        }
    }

    void Highlight(bool isHighlighted)
    {
        if (isHighlighted)
        {
            Border.BackgroundColor = HighlightBorderColor;
            Container.BackgroundColor = HighlightBackgroundColor;
            ButtonText.TextColor = HighlightForegroundColor;
            ColorIcon(HighlightForegroundColor);
        }
        else
        {
            Border.BackgroundColor = BorderColor;
            Container.BackgroundColor = BackgroundColor;
            ButtonText.TextColor = ForegroundColor;
            ColorIcon(ForegroundColor);
        }
    }
}

public class ColorOverlayEffect : RoutingEffect
{
    public Color Color { get; set; }
}
