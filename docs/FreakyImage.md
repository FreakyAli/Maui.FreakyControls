# FreakyImage

An `Image` that fires an event once the image has finished loading.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyImage
    Source="hero.png"
    ImageLoaded="OnImageLoaded" />
```

---

## Events

| Event | Description |
| --- | --- |
| `ImageLoaded` | Fires once the image has finished rendering |

---

## Notes

- All standard `Image` properties are available.
- `FreakyCircularImage` inherits from `FreakyImage` and adds a circular clip. See [FreakyCircularImage](./FreakyCircularImage.md).
