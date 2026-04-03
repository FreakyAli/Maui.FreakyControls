# FreakyJumpList

An alphabetical jump bar for quick-scrolling through a list. Renders a vertical strip of letters; tapping or dragging fires `SelectedCharacterChanged` so you can scroll your list to the matching section.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyJumpList
    CharacterColor="Gray"
    SelectedCharacterColor="DodgerBlue"
    HasHapticFeedback="true"
    SelectedCharacterChanged="OnJumpCharacterChanged" />
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `SelectedCharacter` | `string` | `""` | Currently selected letter |
| `CharacterColor` | `Color` | `Colors.Black` | Default letter colour |
| `SelectedCharacterColor` | `Color` | `Colors.Black` | Highlighted letter colour |
| `CharacterSize` | `float` | `40` | Font size of letters in the bar |
| `HasHapticFeedback` | `bool` | `false` | Trigger haptic feedback on selection |
| `AlphabetProvider` | `IAlphabetProvider` | `null` | Supply a custom alphabet; defaults to A–Z |

---

## Events

| Event | Args | Description |
| --- | --- | --- |
| `SelectedCharacterChanged` | `FreakyCharacterChangedEventArgs` | Fires when a letter is tapped or dragged to |
