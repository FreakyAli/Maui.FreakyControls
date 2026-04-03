# FreakyTextInputLayout

A Material Design-inspired text input with an animated floating label, configurable border types (none, underline, outline), and an optional side image.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyTextInputLayout
    Title="Email address"
    Text="{Binding Email, Mode=TwoWay}"
    BorderType="Outline"
    BorderStroke="Gray"
    BorderCornerRadius="8"
    TitleColor="Gray"
    TextColor="Black"
    ImageSource="email.png"
    AllowCopyPaste="true" />
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Text` | `string` | `""` | TwoWay — input text |
| `Title` | `string` | `""` | TwoWay — floating label text |
| `TitleColor` | `Color` | `Colors.Black` | |
| `TitleFontSize` | `double` | `0` | |
| `TextColor` | `Color` | `Colors.Black` | |
| `FontSize` | `double` | `0` | |
| `FontFamily` | `string` | `null` | |
| `FontAttributes` | `FontAttributes` | `None` | |
| `FontAutoScalingEnabled` | `bool` | `true` | |
| `BorderType` | `BorderType` | `None` | `None`, `Underline`, or `Outline` |
| `BorderStroke` | `Brush` | `Brush.Black` | Border brush |
| `BorderStrokeThickness` | `double` | `0` | |
| `BorderCornerRadius` | `CornerRadius` | default | Applies to `Outline` border type |
| `UnderlineColor` | `Color` | `Colors.Black` | Applies to `Underline` border type |
| `UnderlineThickness` | `double` | `0` | |
| `OutlineTitleBackgroundColor` | `Color` | `Colors.White` | Background colour behind the floating label cutout |
| `ControlBackgroundColor` | `Color` | `Transparent` | |
| `Keyboard` | `Keyboard` | `Default` | |
| `IsPassword` | `bool` | `false` | |
| `ReturnType` | `ReturnType` | `Default` | |
| `ReturnCommand` | `ICommand` | `null` | |
| `ReturnCommandParameter` | `object` | `null` | |
| `AllowCopyPaste` | `bool` | `true` | When `false`, hides the cut/copy/paste context menu |
| `CharacterSpacing` | `double` | `0` | |
| `ClearButtonVisibility` | `ClearButtonVisibility` | `Never` | |
| `CursorPosition` | `int` | `0` | |
| `SelectionLength` | `int` | `0` | |
| `HorizontalTextAlignment` | `TextAlignment` | default | |
| `VerticalTextAlignment` | `TextAlignment` | default | |
| `IsTextPredictionEnabled` | `bool` | `true` | |
| `IsSpellCheckEnabled` | `bool` | `false` | |
| `IsReadOnly` | `bool` | `false` | |
| `MaxLength` | `int` | `int.MaxValue` | |
| `TextTransform` | `TextTransform` | `Default` | |
| `ImageSource` | `ImageSource` | `null` | Optional side icon |
| `ImageHeight` | `int` | `25` | |
| `ImageWidth` | `int` | `25` | |
| `ImagePadding` | `int` | `5` | |
| `ImageCommand` | `ICommand` | `null` | |
| `ImageCommandParameter` | `object` | `null` | |

---

## Events

| Event | Description |
| --- | --- |
| `TextChanged` | Fires on text input |
| `Completed` | Fires when the return key is pressed |
