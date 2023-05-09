using System.Windows.Input;
using Maui.FreakyControls.GestureRecognizers;
using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls;

public class FreakyStateLayout : Border
{
    #region Bindable Properties

    /// <summary>
    /// Backing BindableProperty for the <see cref="Status"/> property.
    /// </summary>
    public static readonly BindableProperty StateProperty =
                        BindableProperty.Create(
                            nameof(Status),
                            typeof(StateLayoutStatus),
                            typeof(FreakyStateLayout),
                            StateLayoutStatus.NotPressed,
                            BindingMode.OneWayToSource);

    /// <summary>
    /// Property that gets updated depending on the button state. This is a bindable property.
    /// </summary>
    public StateLayoutStatus Status
    {
        get => (StateLayoutStatus)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    #endregion Bindable Properties

    #region Commands

    /// <summary>
    /// Backing BindableProperty for the <see cref="ClickedCommand"/> property.
    /// </summary>
    public static readonly BindableProperty ClickedCommandProperty =
                        BindableProperty.Create(
                            nameof(ClickedCommand),
                            typeof(ICommand),
                            typeof(FreakyStateLayout));

    /// <summary>
    /// Command that is triggered when the button is clicked. This is a bindable property.
    /// </summary>
    public ICommand ClickedCommand
    {
        get => (ICommand)GetValue(ClickedCommandProperty);
        set => SetValue(ClickedCommandProperty, value);
    }

    /// <summary>
    /// Backing BindableProperty for the <see cref="ClickedCommandParameter"/> property.
    /// </summary>
    public static readonly BindableProperty ClickedCommandParameterProperty =
                        BindableProperty.Create(
                            nameof(ClickedCommandParameter),
                            typeof(object),
                            typeof(FreakyStateLayout));

    /// <summary>
    /// Property that gets returned when  <see cref="ClickedCommand" /> is executed. This is a bindable property.
    /// </summary>
    public object ClickedCommandParameter
    {
        get => GetValue(ClickedCommandParameterProperty);
        set => SetValue(ClickedCommandParameterProperty, value);
    }

    /// <summary>
    /// Backing BindableProperty for the <see cref="PressedCommand"/> property.
    /// </summary>
    public static readonly BindableProperty PressedCommandProperty =
                        BindableProperty.Create(
                            nameof(PressedCommand),
                            typeof(ICommand),
                            typeof(FreakyStateLayout));

    /// <summary>
    /// Command that is triggered when the button is pressed. This is a bindable property.
    /// </summary>
    public ICommand PressedCommand
    {
        get => (ICommand)GetValue(PressedCommandProperty);
        set => SetValue(PressedCommandProperty, value);
    }

    /// <summary>
    /// Backing BindableProperty for the <see cref="PressedCommandParameter"/> property.
    /// </summary>
    public static readonly BindableProperty PressedCommandParameterProperty =
                        BindableProperty.Create(
                            nameof(PressedCommandParameter),
                            typeof(object),
                            typeof(FreakyStateLayout));

    /// <summary>
    /// Property that gets returned when  <see cref="PressedCommand" /> is executed. This is a bindable property.
    /// </summary>
    public object PressedCommandParameter
    {
        get => GetValue(PressedCommandParameterProperty);
        set => SetValue(PressedCommandParameterProperty, value);
    }

    /// <summary>
    /// Backing BindableProperty for the <see cref="ReleasedCommand"/> property.
    /// </summary>
    public static readonly BindableProperty ReleasedCommandProperty =
                        BindableProperty.Create(
                            nameof(ReleasedCommand),
                            typeof(ICommand),
                            typeof(FreakyStateLayout));

    /// <summary>
    /// Command that is triggered when the button is released. This is a bindable property.
    /// </summary>
    public ICommand ReleasedCommand
    {
        get => (ICommand)GetValue(ReleasedCommandProperty);
        set => SetValue(ReleasedCommandProperty, value);
    }

    /// <summary>
    /// Backing BindableProperty for the <see cref="ReleasedCommandParameter"/> property.
    /// </summary>
    public static readonly BindableProperty ReleasedCommandParameterProperty =
                        BindableProperty.Create(
                            nameof(ReleasedCommandParameter),
                            typeof(object),
                            typeof(FreakyStateLayout));

    /// <summary>
    /// Property that gets returned when  <see cref="ReleasedCommand" /> is executed. This is a bindable property.
    /// </summary>
    public object ReleasedCommandParameter
    {
        get => GetValue(ReleasedCommandParameterProperty);
        set => SetValue(ReleasedCommandParameterProperty, value);
    }

    #endregion Commands

    #region Events

    /// <summary>
    /// Event that is triggered when button is pressed. This is a bindable property.
    /// </summary>
    public event EventHandler<EventArgs> Pressed;

    /// <summary>
    /// Event that is triggered when button is released. This is a bindable property.
    /// </summary>
    public event EventHandler<EventArgs> Released;

    /// <summary>
    /// Event that is triggered when button is clicked. This is a bindable property.
    /// </summary>
    public event EventHandler<EventArgs> Clicked;

    #endregion Events

    public FreakyStateLayout()
    {
        var touchGestureRecognizor = new FreakyTouchGestureRecognizer();
        this.GestureRecognizers.Add(touchGestureRecognizor);
        Margin = 0;
        Padding = 10;
        BackgroundColor = Colors.Transparent;
        StrokeThickness = 0;
        Stroke = Colors.Transparent;
        touchGestureRecognizor.Pressed += PressedGesture;
        touchGestureRecognizor.Released += ReleasedGesture;
        touchGestureRecognizor.Clicked += ClickedGesture;
        VisualStateManager.GoToState(this, nameof(StateLayoutStatus.NotPressed));
    }

    private void ClickedGesture(object sender, FreakyEventArgs e)
    {
        InvokeClicked();
    }

    private void ReleasedGesture(object sender, FreakyEventArgs e)
    {
        InvokeReleased();
    }

    private void PressedGesture(object sender, FreakyEventArgs e)
    {
        InvokePressed();
    }

    internal void InvokeClicked()
    {
        if (!IsEnabled) return;

        Clicked?.Invoke(this, EventArgs.Empty);
        ClickedCommand?.Execute(ClickedCommandParameter);
    }

    internal void InvokePressed()
    {
        if (!IsEnabled) return;

        if (Status.Equals(StateLayoutStatus.NotPressed)) return;

        Released?.Invoke(this, EventArgs.Empty);
        ReleasedCommand?.Execute(ReleasedCommandParameter);

        VisualStateManager.GoToState(this, nameof(StateLayoutStatus.NotPressed));
        Status = StateLayoutStatus.NotPressed;
    }

    internal void InvokeReleased()
    {
        if (!IsEnabled) return;

        Pressed?.Invoke(this, EventArgs.Empty);
        PressedCommand?.Execute(PressedCommandParameter);

        VisualStateManager.GoToState(this, nameof(StateLayoutStatus.Pressed));
        Status = StateLayoutStatus.Pressed;
    }
}