# FreakyDatePicker

A `DatePicker` extended with an optional side image and tap command.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyDatePicker
    Date="{Binding SelectedDate}"
    ImageSource="calendar.png"
    ImageAlignment="Right"
    ImageCommand="{Binding OpenCalendarCommand}" />
```

---

## Properties

All standard `DatePicker` properties apply, plus:

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `ImageSource` | `ImageSource` | `null` | Side icon |
| `ImageHeight` | `int` | `25` | |
| `ImageWidth` | `int` | `25` | |
| `ImageAlignment` | `ImageAlignment` | `Right` | `Left` or `Right` |
| `ImagePadding` | `int` | `5` | |
| `ImageCommand` | `ICommand` | `null` | Command fired when the icon is tapped |
| `ImageCommandParameter` | `object` | `null` | |
