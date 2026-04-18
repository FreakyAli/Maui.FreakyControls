# FreakyChip / FreakyChipGroup

A toggleable chip control. Use `FreakyChipGroup` to wrap multiple chips for single-selection (radio-style) behaviour.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<!-- Standalone chip -->
<freaky:FreakyChip
    Text="Featured"
    IsSelected="{Binding IsFeatured, Mode=TwoWay}"
    SelectedBackgroundColor="DodgerBlue"
    SelectedTextColor="White" />

<!-- Group — single selection -->
<freaky:FreakyChipGroup SelectedIndex="{Binding SelectedTab, Mode=TwoWay}">
    <freaky:FreakyChip Text="All" Name="all" />
    <freaky:FreakyChip Text="Active" Name="active" />
    <freaky:FreakyChip Text="Done" Name="done" />
</freaky:FreakyChipGroup>
```

---

## FreakyChip Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Text` | `string` | `null` | Chip label |
| `Name` | `string` | `"Chip"` | Identifier used by FreakyChipGroup |
| `IsSelected` | `bool` | `false` | TwoWay — selected state |
| `SelectedBackgroundColor` | `Color` | `Colors.LightGray` | |
| `UnselectedBackgroundColor` | `Color` | `Transparent` | |
| `SelectedTextColor` | `Color` | `Colors.Black` | |
| `UnselectedTextColor` | `Color` | `Colors.Black` | |
| `CornerRadius` | `CornerRadius` | default | |
| `Stroke` | `Brush` | `null` | Border brush |
| `StrokeThickness` | `double` | `0` | |
| `Padding` | `Thickness` | `10` | |
| `AnimationColor` | `Color` | default ripple | Ripple/press animation colour |
| `LeadingResourceId` | `string` | `null` | Embedded SVG resource ID for leading icon |
| `TrailingResourceId` | `string` | `null` | Embedded SVG resource ID for trailing icon |
| `LeadingBase64String` | `string` | `null` | Base64 SVG for leading icon |
| `TrailingBase64String` | `string` | `null` | Base64 SVG for trailing icon |
| `SvgAssembly` | `Assembly` | `null` | Assembly containing the SVG resources |
| `ImageColor` | `Color` | `Transparent` | Tint colour for SVG icons |
| `FontFamily` | `string` | `null` | |
| `FontSize` | `double` | default | |
| `FontAttributes` | `FontAttributes` | `None` | |
| `FontAutoScalingEnabled` | `bool` | `true` | Scale font with system accessibility settings |
| `SizeRequest` | `double` | — | Uniform size applied to both leading and trailing icons |
| `TextDecorations` | `TextDecorations` | `None` | |
| `TextTransform` | `TextTransform` | `None` | |
| `TextType` | `TextType` | `Text` | |
| `HorizontalTextAlignment` | `TextAlignment` | `Center` | |
| `VerticalTextAlignment` | `TextAlignment` | `Center` | |
| `SelectedChangedCommand` | `ICommand` | `null` | |

### FreakyChip Events

| Event | Description |
| --- | --- |
| `SelectedChanged` | Fires when `IsSelected` changes |

---

## FreakyChipGroup Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `SelectedIndex` | `int` | `-1` | TwoWay — zero-based index of the selected chip |
| `SelectedFreakyChipChangedCommand` | `ICommand` | `null` | |

### FreakyChipGroup Events

| Event | Args | Description |
| --- | --- | --- |
| `SelectedFreakyChipChanged` | `FreakyRadioButtonEventArgs` | Fires when the selected chip changes |
