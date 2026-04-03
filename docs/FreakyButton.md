# FreakyButton

A fully customisable button with leading/trailing icon slots, a busy/loading state, tap animations, and corner radius support.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyButton
    Text="Submit"
    IsBusy="{Binding IsLoading}"
    Command="{Binding SubmitCommand}"
    CornerRadius="10"
    BackgroundColor="Black"
    TextColor="White">
    <freaky:FreakyButton.LeadingIcon>
        <Image Source="icon.png" />
    </freaky:FreakyButton.LeadingIcon>
</freaky:FreakyButton>
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `Text` | `string` | `null` | Button label |
| `TextColor` | `Color` | `Colors.White` | |
| `BackgroundColor` | `Color` | `Colors.Black` | |
| `BorderColor` | `Color` | `Colors.White` | |
| `BorderWidth` | `double` | inherited | |
| `CornerRadius` | `CornerRadius` | `10` | |
| `Padding` | `Thickness` | `12,0` | |
| `FontFamily` | `string` | `null` | |
| `FontSize` | `double` | default | |
| `FontAttributes` | `FontAttributes` | `None` | |
| `FontAutoScalingEnabled` | `bool` | `true` | |
| `CharacterSpacing` | `double` | inherited | |
| `TextDecorations` | `TextDecorations` | `None` | |
| `TextTransform` | `TextTransform` | `None` | |
| `TextType` | `TextType` | `Text` | Set to `Html` for HTML text rendering |
| `LineBreakMode` | `LineBreakMode` | `NoWrap` | |
| `HorizontalTextAlignment` | `TextAlignment` | `Center` | |
| `VerticalTextAlignment` | `TextAlignment` | `Center` | |
| `LeadingIcon` | `View` | `null` | View placed before the label |
| `TrailingIcon` | `View` | `null` | View placed after the label |
| `IconSize` | `double` | `24` | |
| `Spacing` | `int` | `12` | Space between icon and text |
| `AreIconsDistant` | `bool` | `true` | Push icons to opposite edges |
| `IsBusy` | `bool` | `false` | Shows an activity indicator and disables the button |
| `BusyColor` | `Color` | `Colors.White` | Activity indicator colour |
| `ActivityIndicatorSize` | `double` | `30` | |
| `Animation` | `ButtonAnimations` | `FadeAndScale` | Tap animation style |
| `NativeAnimationColor` | `Color` | `Transparent` | Ripple/native press animation colour |
| `Command` | `ICommand` | `null` | |
| `CommandParameter` | `object` | `null` | |
| `IsEnabled` | `bool` | `true` | |

---

## Events

| Event | Description |
| --- | --- |
| `Clicked` | Fires on button tap |
