# FreakyPicker

A `Picker` extended with an optional side image and tap command.

**Platforms:** iOS, macOS, Android
> Image alignment is not yet implemented on Windows.

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyPicker
    Title="Select country"
    ItemsSource="{Binding Countries}"
    SelectedItem="{Binding SelectedCountry}"
    ImageSource="chevron.png"
    ImageAlignment="Right" />
```

---

## Properties

All standard `Picker` properties apply, plus:

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `ImageSource` | `ImageSource` | `null` | Side icon |
| `ImageHeight` | `int` | `25` | |
| `ImageWidth` | `int` | `25` | |
| `ImageAlignment` | `ImageAlignment` | `Right` | `Left` or `Right` |
| `ImagePadding` | `int` | `5` | |
| `ImageCommand` | `ICommand` | `null` | Command fired when the icon is tapped |
| `ImageCommandParameter` | `object` | `null` | |
