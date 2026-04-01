# FreakyCircularImage

An image clipped to a circle. Inherits from `FreakyImage`. Set equal `HeightRequest` and `WidthRequest` for a perfect circle.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyCircularImage
    Source="avatar.png"
    HeightRequest="80"
    WidthRequest="80" />
```

---

## Notes

- All standard `Image` properties are available.
- The clipping is applied natively on each platform (EllipseGeometry clip on Windows, CALayer mask on Apple, circular outline on Android).
- For a perfect circle, `HeightRequest` and `WidthRequest` must be equal.
- See [FreakyImage](./FreakyImage.md) for the `ImageLoaded` event.
