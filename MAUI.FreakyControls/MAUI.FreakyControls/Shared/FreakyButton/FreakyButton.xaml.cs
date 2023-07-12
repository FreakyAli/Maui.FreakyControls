using Maui.FreakyControls.Extensions;
using Maui.FreakyControls.TouchPress;
using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.FreakyControls;

public enum MaterialButtonType
{
    Elevated, Filled, Tonal, Outlined, Text
}

public partial class MaterialButton : Grid, ITouchAndPressEffectConsumer
{
    #region Attributes and Properties

    private bool _initialized = false;

    private StackLayout _stcLayout;

    private ContentView _leadingIconContentView;

    private ContentView _trailingIconContentView;

    private Label _textLabel;

    private ActivityIndicator _activityIndicator;

    private ContentView _cntActivityIndicator;

    private Frame _frameLayout;

    #endregion Attributes and Properties

    #region Bindable properties

    public static readonly BindableProperty ButtonTypeProperty =
        BindableProperty.Create(nameof(ButtonType), typeof(MaterialButtonType), typeof(MaterialButton), defaultValue: MaterialButtonType.Filled);

    public MaterialButtonType ButtonType
    {
        get { return (MaterialButtonType)GetValue(ButtonTypeProperty); }
        set { SetValue(ButtonTypeProperty, value); }
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MaterialButton), defaultValue: null);

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(MaterialButton), defaultValue: null);

    public object CommandParameter
    {
        get { return (object)GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    public static readonly new BindableProperty IsEnabledProperty =
        BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(MaterialButton), defaultValue: true);

    public new bool IsEnabled
    {
        get { return (bool)GetValue(IsEnabledProperty); }
        set { SetValue(IsEnabledProperty, value); }
    }

    public static readonly BindableProperty AnimationProperty =
        BindableProperty.Create(nameof(Animation), typeof(AnimationTypes), typeof(MaterialButton), defaultValue: AnimationTypes.Fade);

    public AnimationTypes Animation
    {
        get { return (AnimationTypes)GetValue(AnimationProperty); }
        set { SetValue(AnimationProperty, value); }
    }

    public static readonly BindableProperty AnimationParameterProperty =
        BindableProperty.Create(nameof(AnimationParameter), typeof(double?), typeof(MaterialButton), defaultValue: 0.7);

    public double? AnimationParameter
    {
        get { return (double?)GetValue(AnimationParameterProperty); }
        set { SetValue(AnimationParameterProperty, value); }
    }

    public static readonly BindableProperty CustomAnimationProperty =
        BindableProperty.Create(nameof(CustomAnimation), typeof(ICustomAnimation), typeof(MaterialButton), defaultValue: null);

    public ICustomAnimation CustomAnimation
    {
        get { return (ICustomAnimation)GetValue(CustomAnimationProperty); }
        set { SetValue(CustomAnimationProperty, value); }
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(MaterialButton), defaultValue: null);

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(MaterialButton), defaultValue: 20.0);

    public double CornerRadius
    {
        get { return (double)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly BindableProperty IsBusyProperty =
        BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(MaterialButton), defaultValue: false);

    public bool IsBusy
    {
        get { return (bool)GetValue(IsBusyProperty); }
        set { SetValue(IsBusyProperty, value); }
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(MaterialButton), defaultValue: Colors.Black);

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public static readonly new BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(MaterialButton), defaultValue: Colors.White);

    public new Color BackgroundColor
    {
        get { return (Color)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }

    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(MaterialButton), defaultValue: Colors.Black);

    public Color BorderColor
    {
        get { return (Color)GetValue(BorderColorProperty); }
        set { SetValue(BorderColorProperty, value); }
    }

    public static readonly BindableProperty BusyColorProperty =
        BindableProperty.Create(nameof(BusyColor), typeof(Color), typeof(MaterialButton), defaultValue: Colors.Gray);

    public Color BusyColor
    {
        get { return (Color)GetValue(BusyColorProperty); }
        set { SetValue(BusyColorProperty, value); }
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(MaterialButton), defaultValue: Microsoft.Maui.Font.Default.Size);

    public double FontSize
    {
        get { return (double)GetValue(FontSizeProperty); }
        set { SetValue(FontSizeProperty, value); }
    }

    public static readonly BindableProperty FontFamilyProperty =
        BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(MaterialButton), defaultValue: null);

    public string FontFamily
    {
        get { return (string)GetValue(FontFamilyProperty); }
        set { SetValue(FontFamilyProperty, value); }
    }

    public static readonly BindableProperty LeadingIconProperty =
        BindableProperty.Create(nameof(LeadingIcon), typeof(View), typeof(MaterialButton), defaultValue: null);

    public View LeadingIcon
    {
        get { return (View)GetValue(LeadingIconProperty); }
        set { SetValue(LeadingIconProperty, value); }
    }

    public static readonly BindableProperty TrailingIconProperty =
        BindableProperty.Create(nameof(TrailingIcon), typeof(View), typeof(MaterialButton), defaultValue: null);

    public View TrailingIcon
    {
        get { return (View)GetValue(TrailingIconProperty); }
        set { SetValue(TrailingIconProperty, value); }
    }

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(nameof(IconSize), typeof(double), typeof(MaterialButton), defaultValue: 24.0);

    public double IconSize
    {
        get { return (double)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
    }

    public static readonly BindableProperty ToUpperProperty =
        BindableProperty.Create(nameof(ToUpper), typeof(bool), typeof(MaterialButton), defaultValue: false);

    public bool ToUpper
    {
        get { return (bool)GetValue(ToUpperProperty); }
        set { SetValue(ToUpperProperty, value); }
    }

    public static readonly BindableProperty CustomActivityIndicatorProperty =
        BindableProperty.Create(nameof(CustomActivityIndicator), typeof(View), typeof(MaterialButton), defaultValue: null);

    public View CustomActivityIndicator
    {
        get { return (View)GetValue(CustomActivityIndicatorProperty); }
        set { SetValue(CustomActivityIndicatorProperty, value); }
    }

    public static readonly BindableProperty ActivityIndicatorSizeProperty =
        BindableProperty.Create(nameof(ActivityIndicatorSize), typeof(double), typeof(MaterialButton), defaultValue: 24.0);

    public double ActivityIndicatorSize
    {
        get { return (double)GetValue(ActivityIndicatorSizeProperty); }
        set { SetValue(ActivityIndicatorSizeProperty, value); }
    }

    public static readonly new BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(MaterialButton), defaultValue: new Thickness(12, 0));

    public new Thickness Padding
    {
        get { return (Thickness)GetValue(PaddingProperty); }
        set { SetValue(PaddingProperty, value); }
    }

    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(int), typeof(MaterialButton), defaultValue: 12);

    public int Spacing
    {
        get { return (int)GetValue(SpacingProperty); }
        set { SetValue(SpacingProperty, value); }
    }

    public static readonly BindableProperty ContentIsExpandedProperty =
        BindableProperty.Create(nameof(ContentIsExpanded), typeof(bool), typeof(MaterialButton), defaultValue: false);

    public bool ContentIsExpanded
    {
        get { return (bool)GetValue(ContentIsExpandedProperty); }
        set { SetValue(ContentIsExpandedProperty, value); }
    }

    public event EventHandler Clicked;

    #endregion Bindable properties

    #region Constructors

    public MaterialButton()
    {
        if (Children == null)
            return;

        if (!_initialized)
            Initialize();
    }

    #endregion Constructors

    #region Methods

    private void Initialize()
    {
        _initialized = true;

        MinimumHeightRequest = 40;
        HeightRequest = 40;

        _frameLayout = new Frame
        {
            HasShadow = false,
            CornerRadius = Convert.ToInt32(CornerRadius),
            Padding = Padding
        };
        // Todo Fix this shit up
        Children.Add(_frameLayout);

        _stcLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Spacing = Spacing,
            HorizontalOptions = ContentIsExpanded ? LayoutOptions.FillAndExpand : LayoutOptions.Center,
        };
        _frameLayout.Content = _stcLayout;

        _leadingIconContentView = new ContentView
        {
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = IconSize,
            HeightRequest = IconSize,
            IsVisible = false
        };
        _stcLayout.Children.Add(_leadingIconContentView);

        _textLabel = new Label
        {
            LineBreakMode = LineBreakMode.NoWrap,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = ContentIsExpanded ? LayoutOptions.CenterAndExpand : LayoutOptions.Center,
            Text = ToUpper ? Text?.ToUpper() : Text,
            FontSize = FontSize,
            FontFamily = FontFamily,

        };
        _stcLayout.Children.Add(_textLabel);

        _trailingIconContentView = new ContentView
        {
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = IconSize,
            HeightRequest = IconSize,
            IsVisible = false
        };
        _stcLayout.Children.Add(_trailingIconContentView);

        _cntActivityIndicator = new ContentView
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = ActivityIndicatorSize,
            HeightRequest = ActivityIndicatorSize,
            IsVisible = false
        };
        //TODO Fix this shit up
        this.Children.Add(_cntActivityIndicator);

        Effects.Add(new TouchAndPressEffect());

        SetButtonType();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        if (Children == null)
            return;

        if (!_initialized)
            Initialize();

        switch (propertyName)
        {
            case nameof(ButtonType):
            case nameof(TextColor):
            case nameof(BackgroundColor):
            case nameof(BorderColor):
                SetButtonType();
                break;
            case nameof(base.Opacity):
            case nameof(base.Scale):
            case nameof(base.IsVisible):
                base.OnPropertyChanged(propertyName);
                break;
            case nameof(Text):
            case nameof(ToUpper):
                _textLabel.Text = ToUpper ? Text?.ToUpper() : Text;
                break;
            case nameof(FontSize):
                _textLabel.FontSize = FontSize;
                break;
            case nameof(FontFamily):
                _textLabel.FontFamily = FontFamily;
                break;
            case nameof(CornerRadius):
                _frameLayout.CornerRadius = Convert.ToInt32(CornerRadius);
                break;
            case nameof(LeadingIcon):
                if (LeadingIcon != null)
                {
                    _leadingIconContentView.Content = LeadingIcon;
                    _leadingIconContentView.IsVisible = true;
                }
                break;
            case nameof(TrailingIcon):
                if (TrailingIcon != null)
                {
                    _trailingIconContentView.Content = TrailingIcon;
                    _trailingIconContentView.IsVisible = true;
                }
                break;
            case nameof(IconSize):
                _leadingIconContentView.HeightRequest = IconSize;
                _leadingIconContentView.WidthRequest = IconSize;
                _trailingIconContentView.HeightRequest = IconSize;
                _trailingIconContentView.WidthRequest = IconSize;
                break;
            case nameof(IsEnabled):
                VisualStateManager.GoToState(this, IsEnabled ? "Normal" : "Disabled");
                break;
            case nameof(ActivityIndicatorSize):
                _cntActivityIndicator.HeightRequest = ActivityIndicatorSize;
                _cntActivityIndicator.WidthRequest = ActivityIndicatorSize;
                break;
            case nameof(CustomActivityIndicator):
                if (CustomActivityIndicator != null)
                {
                    _cntActivityIndicator.Content = CustomActivityIndicator;
                }
                break;
            case nameof(BusyColor):
                if (CustomActivityIndicator == null)
                {
                    if (_activityIndicator == null)
                    {
                        _activityIndicator = new ActivityIndicator();
                        _cntActivityIndicator.Content = _activityIndicator;
                    }

                    _activityIndicator.Color = BusyColor;
                }
                break;
            case nameof(IsBusy):
                if (IsBusy)
                {
                    if (CustomActivityIndicator == null)
                    {
                        if (_activityIndicator == null)
                        {
                            _activityIndicator = new ActivityIndicator { Color = BusyColor };
                            _cntActivityIndicator.Content = _activityIndicator;
                        }

                        _activityIndicator.IsVisible = true;
                        _activityIndicator.IsRunning = true;
                    }

                    _cntActivityIndicator.IsVisible = true;

                    _frameLayout.IsVisible = false;
                }
                else
                {
                    _frameLayout.IsVisible = true;

                    if (CustomActivityIndicator == null)
                    {
                        if (_activityIndicator == null)
                        {
                            _activityIndicator = new ActivityIndicator();
                            _cntActivityIndicator.Content = _activityIndicator;
                        }

                        _activityIndicator.IsVisible = false;
                        _activityIndicator.IsRunning = false;
                    }

                    _cntActivityIndicator.IsVisible = false;
                }
                break;

            case nameof(Padding):
                _frameLayout.Padding = Padding;
                break;
            case nameof(Spacing):
                _stcLayout.Spacing = Spacing;
                break;
            case nameof(ContentIsExpanded):
                _stcLayout.HorizontalOptions = ContentIsExpanded ? LayoutOptions.FillAndExpand : LayoutOptions.Center;
                _textLabel.HorizontalOptions = ContentIsExpanded ? LayoutOptions.CenterAndExpand : LayoutOptions.Center;
                break;
        }
    }

    public void SetButtonType()
    {
        switch (ButtonType)
        {
            case MaterialButtonType.Elevated:
                _textLabel.TextColor = TextColor;
                _frameLayout.BackgroundColor = BackgroundColor;
                _frameLayout.BorderColor = BackgroundColor;
                _frameLayout.HasShadow = true;
                break;
            case MaterialButtonType.Filled:
                _textLabel.TextColor = TextColor;
                _frameLayout.BackgroundColor = BackgroundColor;
                _frameLayout.BorderColor = Colors.Transparent;
                _frameLayout.HasShadow = false;
                break;
            case MaterialButtonType.Tonal:
                _textLabel.TextColor = TextColor;
                _frameLayout.BackgroundColor = BackgroundColor;
                _frameLayout.BorderColor = Colors.Transparent;
                _frameLayout.HasShadow = false;
                break;
            case MaterialButtonType.Outlined:
                _textLabel.TextColor=TextColor;
                _frameLayout.BackgroundColor = BackgroundColor;
                _frameLayout.BorderColor = BorderColor;
                _frameLayout.HasShadow = false;
                break;
            case MaterialButtonType.Text:
                _textLabel.TextColor = TextColor;
                _frameLayout.BackgroundColor = Colors.Transparent;
                _frameLayout.BorderColor = Colors.Transparent;
                _frameLayout.HasShadow = false;
                break;
        }
    }

    public void ConsumeEvent(EventType gestureType)
    {
        TouchAndPressAnimation.Animate(this, gestureType);
    }

    public void ExecuteAction()
    {
        if (IsEnabled && Command != null && Command.CanExecute(CommandParameter))
            Command.Execute(CommandParameter);

        if (IsEnabled && Clicked != null)
            Clicked.Invoke(this, null);
    }

    #endregion Methods
}