using Maui.FreakyControls.Extensions;
using System.Windows.Input;

namespace Maui.FreakyControls;

public class FreakyChipGroup : StackLayout
{
    private readonly List<FreakyChip> chips = new List<FreakyChip>();

    /// <summary>
    /// Triggered when <see cref="FreakyChipGroup.SelectedIndex"/> changes.
    /// </summary>
    public event EventHandler<FreakyRadioButtonEventArgs> SelectedFreakyChipChanged;

    public static readonly BindableProperty SelectedIndexProperty =
    BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(FreakyChipGroup),
        -1,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((FreakyChipGroup)bindable).UpdateCheckedStates();
            ((FreakyChipGroup)bindable).SetDefaultCheckedFreakyChip();
        });

    /// <summary>
    /// Triggered when <see cref="FreakyChipGroup.SelectedIndex"/> changes.
    /// Has the <see cref="FreakyRadioButtonEventArgs"/> as a Command Parameter
    /// </summary>
    public ICommand SelectedFreakyChipChangedCommand
    {
        get { return (ICommand)GetValue(SelectedFreakyChipChangedCommandProperty); }
        set { SetValue(SelectedFreakyChipChangedCommandProperty, value); }
    }

    public static readonly BindableProperty SelectedFreakyChipChangedCommandProperty =
    BindableProperty.Create(
        nameof(SelectedFreakyChipChangedCommand),
        typeof(ICommand),
        typeof(FreakyChipGroup));

    /// <summary>
    /// SelectedIndex of <see cref="FreakyChip"/> in this Group
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

    private void SetDefaultCheckedFreakyChip()
    {
        if (SelectedIndex < 0 || SelectedIndex >= chips.Count)
        {
            return;
        }

        chips[SelectedIndex].IsSelected = true;
    }

    protected override void OnChildAdded(Element child)
    {
        base.OnChildAdded(child);

        if (child is FreakyChip radioButton)
        {
            chips.Add(radioButton);
            radioButton.SelectedChanged += FreakyChip_CheckedChanged;
        }
        else if (child is Layout layout)
        {
            foreach (var grandChild in layout.Children)
            {
                if (grandChild is FreakyChip grandChildFreakyChip)
                {
                    chips.Add(grandChildFreakyChip);
                    grandChildFreakyChip.SelectedChanged += FreakyChip_CheckedChanged;
                }
            }
        }
    }

    private void FreakyChip_CheckedChanged(object sender, EventArgs e)
    {
        if (sender is not FreakyChip selectedFreakyChip || !selectedFreakyChip.IsSelected)
        {
            return;
        }

        foreach (var radioButton in chips)
        {
            if (radioButton != selectedFreakyChip)
            {
                radioButton.IsSelected = false;
            }
        }

        int index = FindIndex(selectedFreakyChip);
        string name = selectedFreakyChip.Name;
        var eventArgs = new FreakyRadioButtonEventArgs(name, index);
        this.SelectedFreakyChipChangedCommand?.ExecuteCommandIfAvailable(eventArgs);
        SelectedFreakyChipChanged?.Invoke(this, eventArgs);
    }

    private int FindIndex(FreakyChip radioButton)
    {
        int index = 0;

        return FindIndexRecursive(Children, radioButton, index);
    }

    private int FindIndexRecursive(IList<IView> children, FreakyChip radioButton, int index)
    {
        foreach (var child in children)
        {
            if (child is FreakyChip)
            {
                if (child == radioButton)
                {
                    return index;
                }
                index++;
            }
            else if (child is Layout layout)
            {
                int childIndex = FindIndexRecursive(layout.Children, radioButton, index);
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
            if (child is FreakyChip radioButton)
            {
                radioButton.IsSelected = index == SelectedIndex;
                index++;
            }
            else if (child is Layout layout)
            {
                LoopChildren(layout.Children, index);
            }
        }
    }
}