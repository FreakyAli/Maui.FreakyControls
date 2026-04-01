# FreakyEntry

An `Entry` extended with an optional side image, tap command, and copy/paste control.

**Platforms:** iOS, macOS, Android
> Image alignment is not yet implemented on Windows.

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyEntry
    Text="{Binding Username}"
    Placeholder="Enter username"
    ImageSource="user.png"
    ImageAlignment="Left"
    ImageCommand="{Binding ClearCommand}"
    AllowCopyPaste="true" />
```

---

## Properties

All standard `Entry` properties apply, plus:

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `ImageSource` | `ImageSource` | `null` | Side icon |
| `ImageHeight` | `int` | `25` | |
| `ImageWidth` | `int` | `25` | |
| `ImageAlignment` | `ImageAlignment` | `Right` | `Left` or `Right` |
| `ImagePadding` | `int` | `5` | |
| `ImageCommand` | `ICommand` | `null` | Command fired when the icon is tapped |
| `ImageCommandParameter` | `object` | `null` | |
| `AllowCopyPaste` | `bool` | `true` | When `false`, the context menu (cut/copy/paste) is hidden on all platforms |
