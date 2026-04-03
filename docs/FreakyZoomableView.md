# FreakyZoomableView

A container that adds pinch-to-zoom and pan gesture support to any child view.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyZoomableView
    MinScale="1"
    MaxScale="4"
    DoubleTapToZoom="true"
    DoubleTapScaleFactor="4">
    <Image Source="map.png" />
</freaky:FreakyZoomableView>
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `MinScale` | `double` | `1` | Minimum zoom level |
| `MaxScale` | `double` | `4` | Maximum zoom level |
| `DoubleTapToZoom` | `bool` | `true` | Enable double-tap to zoom in |
| `DoubleTapScaleFactor` | `double` | `4` | Scale applied when double-tapping |
| `IsDoubleTapZoomAnimationEnabled` | `bool` | `true` | Animate the double-tap zoom |
| `Zoomable` | `bool` | `true` | Enable or disable pinch-to-zoom |
| `Translateable` | `bool` | `true` | Enable or disable panning |
