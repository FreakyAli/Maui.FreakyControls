# FreakyCheckbox

A SkiaSharp-rendered checkbox with configurable shapes, animation styles, fill/outline colours, and design variants.

> Requires `useSkiaSharp: true` in `InitializeFreakyControls`.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyCheckbox
    IsChecked="{Binding IsAccepted, Mode=TwoWay}"
    OutlineColor="Black"
    FillColor="Blue"
    CheckColor="White"
    SizeRequest="24"
    CheckedChangedCommand="{Binding AcceptCommand}" />
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `IsChecked` | `bool` | `false` | TwoWay — checked state |
| `HasCheckAnimation` | `bool` | `true` | Animate the check mark on toggle |
| `OutlineColor` | `Color` | `Colors.Black` | Border colour |
| `FillColor` | `Color` | `Colors.White` | Background colour when checked |
| `CheckColor` | `Color` | `Colors.Black` | Tick mark colour |
| `OutlineWidth` | `float` | `6` | Border stroke width |
| `CheckWidth` | `float` | `6` | Tick stroke width |
| `Shape` | `Shape` | platform default | `Rectangle` or `Circle` |
| `CheckType` | `CheckType` | `Check` | `Check` (tick) or `Fill` (solid fill) |
| `Design` | `Design` | `Unified` | Visual design variant |
| `AnimationType` | `AnimationType` | `Default` | Animation style on toggle |
| `SizeRequest` | `double` | `24` | Width and height of the control |
| `CheckedChangedCommand` | `ICommand` | `null` | Executes when `IsChecked` changes |

---

## Events

| Event | Args | Description |
| --- | --- | --- |
| `CheckedChanged` | `CheckedChangedEventArgs` | Fires when `IsChecked` changes |
