using System.Runtime.CompilerServices;
using System.Windows.Input;
using Maui.FreakyControls.Shared.Enums;
using Maui.FreakyControls.Shared.TouchPress;

namespace Maui.FreakyControls;

public class CustomImageButton : ContentView, ITouchPressEffect
{
    private Image image;
    private View customImage;

    #region Constructors

    public CustomImageButton()
    {
        Padding = 6;
        VerticalOptions = LayoutOptions.Center;

        Effects.Add(new TouchAndPressRoutingEffect());
    }

    #endregion Constructors

    #region Properties

    public static readonly BindableProperty ImageHeightRequestProperty =
        BindableProperty.Create(nameof(ImageHeightRequest), typeof(double), typeof(CustomImageButton), 24.0);

    public double ImageHeightRequest
    {
        get => (double)GetValue(ImageHeightRequestProperty);
        set => SetValue(ImageHeightRequestProperty, value);
    }

    public static readonly BindableProperty ImageWidthRequestProperty =
        BindableProperty.Create(nameof(ImageWidthRequest), typeof(double), typeof(CustomImageButton), 24.0);

    public double ImageWidthRequest
    {
        get => (double)GetValue(ImageWidthRequestProperty);
        set => SetValue(ImageWidthRequestProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomImageButton), null);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CustomImageButton), null);

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty AnimationProperty =
        BindableProperty.Create(nameof(Animation), typeof(ButtonAnimations), typeof(CustomImageButton),
            ButtonAnimations.None);

    public ButtonAnimations Animation
    {
        get => (ButtonAnimations)GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    public static readonly BindableProperty AnimationParameterProperty =
        BindableProperty.Create(nameof(AnimationParameter), typeof(double?), typeof(CustomImageButton), null);

    public double? AnimationParameter
    {
        get => (double?)GetValue(AnimationParameterProperty);
        set => SetValue(AnimationParameterProperty, value);
    }

    #endregion Properties

    #region Methods

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        switch (propertyName)
        {
            case nameof(ImageWidthRequest):
                if (image != null)
                    image.WidthRequest = ImageWidthRequest;
                else if (customImage != null)
                    customImage.WidthRequest = ImageWidthRequest;
                break;

            case nameof(ImageHeightRequest):
                if (image != null)
                    image.HeightRequest = ImageHeightRequest;
                else if (customImage != null)
                    customImage.HeightRequest = ImageHeightRequest;
                break;
        }
    }

    public void SetImage(string imageSource)
    {
        customImage = null;

        image = new Image
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = ImageWidthRequest,
            HeightRequest = ImageHeightRequest,
            Source = imageSource
        };

        Content = image;
    }

    public void SetCustomImage(View view)
    {
        image = null;
        view.VerticalOptions = LayoutOptions.Center;
        view.HorizontalOptions = LayoutOptions.Center;
        view.WidthRequest = ImageWidthRequest;
        view.HeightRequest = ImageHeightRequest;
        Content = view;
    }

    public void ConsumeEvent(EventType gestureType)
    {
        TouchAndPressAnimation.Animate(this, gestureType);
    }

    public void ExecuteAction()
    {
        if (IsEnabled && Command != null && Command.CanExecute(CommandParameter))
            Command.Execute(CommandParameter);
    }

    #endregion Methods
}