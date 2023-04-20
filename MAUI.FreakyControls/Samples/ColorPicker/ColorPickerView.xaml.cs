namespace Samples.ColorPicker;

public partial class ColorPickerView : ContentPage
{
    public ColorPickerView()
    {
        InitializeComponent();
        BindingContext = new ColorPickerViewModel();
    }

    void ColorPicker_PickedColorChanged(System.Object sender, Microsoft.Maui.Graphics.Color e)
    {
    }
}
