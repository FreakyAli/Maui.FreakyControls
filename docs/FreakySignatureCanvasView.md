# FreakySignatureCanvasView

A freehand signature pad. Supports retrieving the signature as an image stream, raw points, or grouped stroke collections.

**Platforms:** iOS, macOS, Android, Windows

> **Windows:** Rendering uses Polyline-based strokes. `StrokeColor`, `StrokeWidth`, `Points`, `Strokes`, `Clear`, and image export via `GetImageStreamAsync` are all fully supported.

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakySignatureCanvasView
    x:Name="SignaturePad"
    StrokeColor="Black"
    StrokeWidth="3"
    StrokeCompletedCommand="{Binding SaveCommand}"
    ClearedCommand="{Binding ClearCommand}" />
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `StrokeColor` | `Color` | `Colors.Black` | Ink colour |
| `StrokeWidth` | `float` | `2` | Stroke thickness in points |
| `IsBlank` | `bool` | `true` | Read-only — `true` when the canvas has no strokes |
| `Points` | `IEnumerable<Point>` | — | Get or set all strokes as a flat point sequence |
| `Strokes` | `IEnumerable<IEnumerable<Point>>` | — | Get or set strokes as grouped sequences |
| `StrokeCompletedCommand` | `ICommand` | `null` | |
| `ClearedCommand` | `ICommand` | `null` | |

---

## Events

| Event | Description |
| --- | --- |
| `StrokeCompleted` | Fires when the user lifts their finger/stylus |
| `Cleared` | Fires when the canvas is cleared |
| `ImageStreamRequested` | Request the current signature as an image stream |
| `IsBlankRequested` | Request whether the canvas is currently blank |
| `PointsRequested` / `PointsSpecified` | Get or set the flat point data |
| `StrokesRequested` / `StrokesSpecified` | Get or set the grouped stroke data |
| `ClearRequested` | Request a programmatic clear |

---

## Getting the Signature Image

Call `GetImageStreamAsync` on the control to retrieve the signature as an encoded image stream:

```csharp
Stream? stream = await SignaturePad.GetImageStreamAsync(SignatureImageFormat.Png);
if (stream is null) return; // handle the case where the image could not be generated
```

Overloads accept an optional `size`, `scale`, `strokeColor`, and `fillColor`. `ImageStreamRequested` is internal handler plumbing and is not part of the consumer API.