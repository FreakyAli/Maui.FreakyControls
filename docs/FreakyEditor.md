# FreakyEditor

An `Editor` extended with copy/paste control.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyEditor
    Text="{Binding Notes}"
    AllowCopyPaste="false" />
```

---

## Properties

All standard `Editor` properties apply, plus:

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `AllowCopyPaste` | `bool` | `true` | When `false`, the context menu (cut/copy/paste) is hidden on all platforms |
