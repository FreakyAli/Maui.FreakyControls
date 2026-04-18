# FreakyRadioButton / FreakyRadioGroup

A SkiaSharp-rendered radio button. Wrap multiple buttons in `FreakyRadioGroup` for mutually exclusive selection.

> Requires `useSkiaSharp: true` in `InitializeFreakyControls`.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<!-- Standalone -->
<freaky:FreakyRadioButton
    IsChecked="{Binding OptionA, Mode=TwoWay}"
    OutlineColor="Black"
    CheckColor="DodgerBlue"
    SizeRequest="24" />

<!-- Group — single selection -->
<freaky:FreakyRadioGroup SelectedIndex="{Binding SelectedOption, Mode=TwoWay}">
    <freaky:FreakyRadioButton Name="option1" />
    <freaky:FreakyRadioButton Name="option2" />
    <freaky:FreakyRadioButton Name="option3" />
</freaky:FreakyRadioGroup>
```

---

## FreakyRadioButton Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `IsChecked` | `bool` | `false` | TwoWay — checked state |
| `Name` | `string` | `"RadioButton"` | Identifier used by FreakyRadioGroup |
| `HasCheckAnimation` | `bool` | `true` | Animate on toggle |
| `OutlineColor` | `Color` | `Colors.Black` | |
| `FillColor` | `Color` | `Colors.White` | Background fill |
| `CheckColor` | `Color` | `Colors.Black` | Inner dot colour |
| `OutlineWidth` | `float` | `6` | Border stroke width |
| `SizeRequest` | `double` | `24` | Width and height |
| `CheckedChangedCommand` | `ICommand` | `null` | |

### FreakyRadioButton Events

| Event | Args | Description |
| --- | --- | --- |
| `CheckedChanged` | `CheckedChangedEventArgs` | Fires when `IsChecked` changes |

---

## FreakyRadioGroup Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `SelectedIndex` | `int` | `-1` | TwoWay — zero-based index of the selected button |
| `SelectedRadioButtonChangedCommand` | `ICommand` | `null` | |

### FreakyRadioGroup Events

| Event | Args | Description |
| --- | --- | --- |
| `SelectedRadioButtonChanged` | `FreakyRadioButtonEventArgs` | Fires when the selection changes |
