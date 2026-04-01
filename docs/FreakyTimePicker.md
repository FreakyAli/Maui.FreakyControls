# FreakyTimePicker

A `TimePicker` extended with an optional side image and tap command.

**Platforms:** iOS, macOS, Android
> Image alignment is not yet implemented on Windows.

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyTimePicker
    Time="{Binding SelectedTime}"
    ImageSource="clock.png"
    ImageAlignment="Right"
    ImageCommand="{Binding OpenPickerCommand}" />
```

---

## Properties

All standard `TimePicker` properties apply, plus:

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `ImageSource` | `ImageSource` | `null` | Side icon |
| `ImageHeight` | `int` | `25` | |
| `ImageWidth` | `int` | `25` | |
| `ImageAlignment` | `ImageAlignment` | `Right` | `Left` or `Right` |
| `ImagePadding` | `int` | `5` | |
| `ImageCommand` | `ICommand` | `null` | Command fired when the icon is tapped |
| `ImageCommandParameter` | `object` | `null` | |
