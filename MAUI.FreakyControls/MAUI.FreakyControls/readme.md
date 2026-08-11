# FreakyControls

A free, open-source UI kit for .NET MAUI. Provides a comprehensive set of controls and utilities for building modern cross-platform apps. Trim-safe.

**Full documentation:** [github.com/FreakyAli/MAUI.FreakyControls](https://github.com/FreakyAli/MAUI.FreakyControls#readme)

---

## Platform Support

| Platform | Minimum Version         |
| -------- | :---------------------: |
| iOS      | 14.0+                   |
| macOS    | 14.0+ (Mac Catalyst)    |
| Android  | API 23+ (Marshmallow)   |
| Windows  | 10.0.17763+             |

---

## Installation

```
dotnet add package FreakyControls
```

### Initialization

```csharp
using Maui.FreakyControls.Extensions;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // useSkiaSharp: required for FreakyCheckbox, FreakyRadioButton, FreakySvgImageView
        // useFreakyEffects: required for touch/ripple effects
        builder.InitializeFreakyControls(useSkiaSharp: true, useFreakyEffects: true);

        return builder.Build();
    }
}
```

Add the namespace to your XAML pages:

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"
```

---

## Controls

| Control | Description |
| --- | --- |
| `FreakyAutoCompleteView` | Text field with live suggestion dropdown |
| `FreakyButton` | Button with leading/trailing icons and loading state |
| `FreakyCheckbox` | SkiaSharp checkbox with animations |
| `FreakyChip` / `FreakyChipGroup` | Toggleable chip with group selection |
| `FreakyCircularImage` | Image clipped to a circle |
| `FreakyCodeView` | Inline OTP / PIN entry |
| `FreakyPinCodeControl` | OTP entry with built-in custom keyboard |
| `FreakyDatePicker` | DatePicker with optional side image |
| `FreakyEditor` | Editor with copy/paste control |
| `FreakyEntry` | Entry with optional side image and copy/paste control |
| `FreakyImage` | Image with load completion event |
| `FreakyJumpList` | Alphabetical jump bar |
| `FreakyPicker` | Picker with optional side image |
| `FreakyRadioButton` / `FreakyRadioGroup` | SkiaSharp radio button with group |
| `FreakySignatureCanvasView` | Freehand signature pad |
| `FreakySvgImageView` | SVG renderer with tint and tap command |
| `FreakySwipeButton` | Swipe-to-confirm control |
| `FreakySwitch` | Animated toggle switch |
| `FreakyTextInputLayout` | Material-style floating label text input |
| `FreakyTimePicker` | TimePicker with optional side image |
| `FreakyZoomableView` | Pinch-to-zoom and pan container |

---

## Breaking Changes — v0.5.0+

The `Shared` folder was removed from all namespaces:

```csharp
using Maui.FreakyControls.Shared.Enums; // old
using Maui.FreakyControls.Enums;        // new
```

Deprecated APIs from previous versions have been removed.
