using Maui.FreakyControls.Extensions;
using System.Windows.Input;

namespace Maui.FreakyControls;

public class FreakyRadioGroup : StackLayout
{
    private List<FreakyRadioButton> radioButtons = new List<FreakyRadioButton>();

    /// <summary>
    /// Triggered when <see cref="FreakyRadioGroup.SelectedIndex"/> changes.
    /// </summary>
    public event EventHandler<FreakyRadioButtonEventArgs> SelectedRadioButtonChanged;

    public static readonly BindableProperty SelectedIndexProperty =
    BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(FreakyRadioGroup),
        -1,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((FreakyRadioGroup)bindable).UpdateCheckedStates();
            ((FreakyRadioGroup)bindable).SetDefaultCheckedRadioButton();
        });

    /// <summary>
    /// Triggered when <see cref="FreakyRadioGroup.SelectedIndex"/> changes.
    /// Has the <see cref="FreakyRadioButtonEventArgs"/> as a Command Parameter
    /// </summary>
    public ICommand SelectedRadioButtonChangedCommand
    {
        get { return (ICommand)GetValue(SelectedRadioButtonChangedCommandProperty); }
        set { SetValue(SelectedRadioButtonChangedCommandProperty, value); }
    }

    public static readonly BindableProperty SelectedRadioButtonChangedCommandProperty =
    BindableProperty.Create(
        nameof(SelectedRadioButtonChangedCommand),
        typeof(ICommand),
        typeof(FreakyRadioGroup));

    /// <summary>
    /// SelectedIndex of <see cref="FreakyRadioButton"/> in this Group
    /// </summary>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    private void UpdateCheckedStates()
    {
        int index = 0;
        LoopChildren(Children, index);
    }

    private void SetDefaultCheckedRadioButton()
    {
        if (SelectedIndex < 0 || SelectedIndex >= radioButtons.Count)
        {
            return;
        }

        radioButtons[SelectedIndex].IsChecked = true;
    }

    protected override void OnChildAdded(Element child)
    {
        base.OnChildAdded(child);

        if (child is FreakyRadioButton radioButton)
        {
            radioButtons.Add(radioButton);
            radioButton.CheckedChanged += RadioButton_CheckedChanged;
        }
        else if (child is Layout layout)
        {
            foreach (var grandChild in layout.Children)
            {
                if (grandChild is FreakyRadioButton grandChildRadioButton)
                {
                    radioButtons.Add(grandChildRadioButton);
                    grandChildRadioButton.CheckedChanged += RadioButton_CheckedChanged;
                }
            }
        }
    }

    private void RadioButton_CheckedChanged(object sender, EventArgs e)
    {
        if (!(sender is FreakyRadioButton selectedRadioButton) || !selectedRadioButton.IsChecked)
        {
            return;
        }

        foreach (var radioButton in radioButtons)
        {
            if (radioButton != selectedRadioButton)
            {
                radioButton.IsChecked = false;
            }
        }

        int index = FindIndex(selectedRadioButton);
        string name = selectedRadioButton.Name;
        var eventArgs = new FreakyRadioButtonEventArgs(name, index);
        this.SelectedRadioButtonChangedCommand?.ExecuteWhenAvailable(eventArgs);
        SelectedRadioButtonChanged?.Invoke(this, eventArgs);
    }

    private int FindIndex(FreakyRadioButton radioButton)
    {
        int index = 0;

        return FindIndexRecursive(Children, radioButton, index);
    }

    private int FindIndexRecursive(IList<IView> children, FreakyRadioButton radioButton, int index)
    {
        foreach (var child in children)
        {
            if (child is FreakyRadioButton)
            {
                if (child == radioButton)
                {
                    return index;
                }
                index++;
            }
            else if (child is Layout)
            {
                int childIndex = FindIndexRecursive(((Layout)child).Children, radioButton, index);
                if (childIndex != -1)
                {
                    return childIndex;
                }
            }
        }

        return -1;
    }

    private void LoopChildren(IList<IView> children, int index)
    {
        foreach (var child in children)
        {
            if (child is FreakyRadioButton)
            {
                FreakyRadioButton radioButton = (FreakyRadioButton)child;
                radioButton.IsChecked = index == SelectedIndex;
                index++;
            }
            else if (child is Layout)
            {
                LoopChildren(((Layout)child).Children, index);
            }
        }
    }
}