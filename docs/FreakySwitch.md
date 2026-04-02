# FreakySwitch

A custom animated toggle switch with configurable track colours, thumb colours, and an optional check mark drawn on the thumb.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakySwitch
    IsToggled="{Binding IsEnabled, Mode=TwoWay}"
    OnColor="SeaGreen"
    OffColor="LightGray"
    ThumbOnColor="White"
    ShowCheckMark="true"
    ToggledCommand="{Binding ToggleCommand}" />
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `IsToggled` | `bool` | `false` | TwoWay — toggle state |
| `OnColor` | `Color` | `Colors.SeaGreen` | Track colour when on |
| `OffColor` | `Color` | `Colors.LightGray` | Track colour when off |
| `ThumbOnColor` | `Color` | `Colors.White` | Thumb colour when on |
| `ThumbOffColor` | `Color` | `Colors.White` | Thumb colour when off |
| `OutlineColor` | `Color` | platform default | Track border colour |
| `ThumbOffSizeFactor` | `float` | platform default | Scale factor of the thumb when off |
| `ShowCheckMark` | `bool` | `false` | Draw a tick on the thumb |
| `CheckMarkColor` | `Color` | `Colors.White` | Check mark colour |
| `CheckMarkWidth` | `float` | `3` | Check mark stroke width |
| `AnimationDuration` | `int` | `250` | Toggle animation duration in milliseconds |
| `ToggledCommand` | `ICommand` | `null` | Executes when the toggle state changes |

---

## Events

| Event | Args | Description |
| --- | --- | --- |
| `Toggled` | `ToggledEventArgs` | Fires when `IsToggled` changes |
