using System.Windows.Input;
using Maui.FreakyControls.Shared.Enums;
using Color = Microsoft.Maui.Graphics.Color;

namespace Maui.FreakyControls;

public class FreakyAutoCompleteView : View, IFreakyAutoCompleteView
{
    private bool suppressTextChangedEvent;

    private readonly WeakEventManager querySubmittedEventManager = new();
    public readonly WeakEventManager textChangedEventManager = new();
    private readonly WeakEventManager suggestionChosenEventManager = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="FreakyAutoCompleteView"/> class
    /// </summary>
    public FreakyAutoCompleteView()
    {
    }

    /// <summary>
    /// Gets or sets the Text property
    /// </summary>
    /// <seealso cref="TextColor"/>
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="Text"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(FreakyAutoCompleteView), "", BindingMode.OneWay, null, OnTextPropertyChanged);

    /// <summary>
    /// This command is invoked whenever the text on <see cref="FreakyAutoCompleteView"/> has changed.
    /// Note that this is fired after the tap or click is lifted.
    /// This is a bindable property.
    /// </summary>
    public ICommand? TextChangedCommand
    {
        get => (ICommand?)GetValue(TextChangedCommandProperty);
        set => SetValue(TextChangedCommandProperty, value);
    }

    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var box = (FreakyAutoCompleteView)bindable;
        if (!box.suppressTextChangedEvent) //Ensure this property changed didn't get call because we were updating it from the native text property
            box.textChangedEventManager.HandleEvent(box, new FreakyAutoCompleteViewTextChangedEventArgs("", TextChangeReason.ProgrammaticChange), nameof(TextChanged));
    }

    /// <summary>
    /// Gets or sets the foreground color of the control
    /// </summary>
    /// <seealso cref="Text"/>
    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="TextColor"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FreakyAutoCompleteView), Colors.Gray, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the PlaceholderText
    /// </summary>
    /// <seealso cref="PlaceholderTextColor"/>
    public string PlaceholderText
    {
        get { return (string)GetValue(PlaceholderTextProperty); }
        set { SetValue(PlaceholderTextProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="PlaceholderText"/> bindable property.
    /// </summary>
    public static readonly BindableProperty PlaceholderTextProperty =
        BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the foreground color of the control
    /// </summary>
    /// <seealso cref="PlaceholderText"/>
    public Color PlaceholderTextColor
    {
        get { return (Color)GetValue(PlaceholderTextColorProperty); }
        set { SetValue(PlaceholderTextColorProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="PlaceholderTextColor"/> bindable property.
    /// </summary>
    public static readonly BindableProperty PlaceholderTextColorProperty =
        BindableProperty.Create(nameof(PlaceholderTextColor), typeof(Color), typeof(FreakyAutoCompleteView), Colors.Gray, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the property path that is used to get the value for display in the
    /// text box portion of the FreakyAutoCompleteView control, when an item is selected.
    /// </summary>
    /// <value>
    /// The property path that is used to get the value for display in the text box portion
    /// of the FreakyAutoCompleteView control, when an item is selected.
    /// </value>
    public string TextMemberPath
    {
        get { return (string)GetValue(TextMemberPathProperty); }
        set { SetValue(TextMemberPathProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="TextMemberPath"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextMemberPathProperty =
        BindableProperty.Create(nameof(TextMemberPath), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the name or path of the property that is displayed for each data item.
    /// </summary>
    /// <value>
    /// The name or path of the property that is displayed for each the data item in
    /// the control. The default is an empty string ("").
    /// </value>
    public string DisplayMemberPath
    {
        get { return (string)GetValue(DisplayMemberPathProperty); }
        set { SetValue(DisplayMemberPathProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="DisplayMemberPath"/> bindable property.
    /// </summary>
    public static readonly BindableProperty DisplayMemberPathProperty =
        BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(FreakyAutoCompleteView), string.Empty, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets a Boolean value indicating whether the drop-down portion of the FreakyAutoCompleteView is open.
    /// </summary>
    /// <value>A Boolean value indicating whether the drop-down portion of the FreakyAutoCompleteView is open.</value>
    public bool IsSuggestionListOpen
    {
        get { return (bool)GetValue(IsSuggestionListOpenProperty); }
        set { SetValue(IsSuggestionListOpenProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="IsSuggestionListOpen"/> bindable property.
    /// </summary>
    public static readonly BindableProperty IsSuggestionListOpenProperty =
        BindableProperty.Create(nameof(IsSuggestionListOpen), typeof(bool), typeof(FreakyAutoCompleteView), false, BindingMode.OneWay, null, null);

    /// <summary>
    /// Used in conjunction with <see cref="TextMemberPath"/>, gets or sets a value indicating whether items in the view will trigger an update
    /// of the editable text part of the <see cref="FreakyAutoCompleteView"/> when clicked.
    /// </summary>
    /// <value>A value indicating whether items in the view will trigger an update of the editable text part of the <see cref="FreakyAutoCompleteView"/> when clicked.</value>
    public bool UpdateTextOnSelect
    {
        get { return (bool)GetValue(UpdateTextOnSelectProperty); }
        set { SetValue(UpdateTextOnSelectProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="UpdateTextOnSelect"/> bindable property.
    /// </summary>
    public static readonly BindableProperty UpdateTextOnSelectProperty =
        BindableProperty.Create(nameof(UpdateTextOnSelect), typeof(bool), typeof(FreakyAutoCompleteView), true, BindingMode.OneWay, null, null);

    /// <summary>
    /// Gets or sets the header object for the text box portion of this control.
    /// </summary>
    /// <value>The header object for the text box portion of this control.</value>
    public System.Collections.IList ItemsSource
    {
        get { return GetValue(ItemsSourceProperty) as System.Collections.IList; }
        set { SetValue(ItemsSourceProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="ItemsSource"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(System.Collections.IList), typeof(FreakyAutoCompleteView), null, BindingMode.OneWay, null, null);

    /// <summary>
    /// Backing BindableProperty for the <see cref="TextChangedCommand"/> property.
    /// </summary>
    public static readonly BindableProperty TextChangedCommandProperty = BindableProperty.Create(nameof(TextChangedCommand), typeof(ICommand), typeof(FreakyAutoCompleteView));

    public event EventHandler<FreakyAutoCompleteViewSuggestionChosenEventArgs> SuggestionChosen
    {
        add => suggestionChosenEventManager.AddEventHandler(value);
        remove => suggestionChosenEventManager.RemoveEventHandler(value);
    }

    public void RaiseSuggestionChosen(FreakyAutoCompleteViewSuggestionChosenEventArgs args)
    {
        suggestionChosenEventManager.HandleEvent(this, args, nameof(SuggestionChosen));
    }

    /// <summary>
    /// Raised before the text content of the editable control component is updated.
    /// </summary>
    public event EventHandler<FreakyAutoCompleteViewTextChangedEventArgs> TextChanged
    {
        add => textChangedEventManager.AddEventHandler(value);
        remove => textChangedEventManager.RemoveEventHandler(value);
    }

    // Called by the native control when users enter text
    public void NativeControlTextChanged(FreakyAutoCompleteViewTextChangedEventArgs args)
    {
        suppressTextChangedEvent = true; //prevent loop of events raising, as setting this property will make it back into the native control
        Text = args.Text;
        suppressTextChangedEvent = false;
        textChangedEventManager.HandleEvent(this, args, nameof(TextChanged));

        //if (TextChangedCommand?.CanExecute(args.Text) ?? false)
        //{
        //    TextChangedCommand.Execute(args.Text);
        //}
    }

    /// <summary>
    /// Raised after the text content of the editable control component is updated.
    /// </summary>
    public void RaiseQuerySubmitted(FreakyAutoCompleteViewQuerySubmittedEventArgs args)
    {
        querySubmittedEventManager.HandleEvent(this, args, nameof(QuerySubmitted));
    }

    /// <summary>
    /// Executes QuerySubmitted event
    /// Occurs when the user submits a search query.
    /// </summary>
    public event EventHandler<FreakyAutoCompleteViewQuerySubmittedEventArgs> QuerySubmitted
    {
        add => querySubmittedEventManager.AddEventHandler(value);
        remove => querySubmittedEventManager.RemoveEventHandler(value);
    }
}