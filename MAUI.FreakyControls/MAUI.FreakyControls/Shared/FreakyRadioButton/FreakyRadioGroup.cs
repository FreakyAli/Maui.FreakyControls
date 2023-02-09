using System;

namespace Maui.FreakyControls;


public class FreakyRadioGroup : StackLayout
{
    private List<FreakyRadioButton> _radioButtons = new List<FreakyRadioButton>();

    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(FreakyRadioGroup), -1,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((FreakyRadioGroup)bindable).UpdateCheckedStates();
            });

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

    public FreakyRadioGroup()
    {
        Orientation = StackOrientation.Horizontal;
    }

    protected override void OnChildAdded(Element child)
    {
        base.OnChildAdded(child);

        if (child is FreakyRadioButton radioButton)
        {
            _radioButtons.Add(radioButton);
            radioButton.CheckedChanged += RadioButton_CheckedChanged;
        }
        else if (child is Layout layout)
        {
            foreach (var grandChild in layout.Children)
            {
                if (grandChild is FreakyRadioButton grandChildRadioButton)
                {
                    _radioButtons.Add(grandChildRadioButton);
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

        foreach (var radioButton in _radioButtons)
        {
            if (radioButton != selectedRadioButton)
            {
                radioButton.IsChecked = false;
            }
        }
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
                radioButton.IsChecked = SomeCondition(radioButton, index);
                index++;
            }
            else if (child is Layout)
            {
                LoopChildren(((Layout)child).Children, index);
            }
        }
    }

    private bool SomeCondition(FreakyRadioButton radioButton, int index)
    {
        // Only one radiobutton can be checked at a time
        return index == SelectedIndex;
    }
}