# FreakyScratchView

A scratch-card control. The user scratches over a front surface (image or colour) to reveal `BackContent` underneath. Built on SkiaSharp.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<freaky:FreakyScratchView
    HeightRequest="300"
    WidthRequest="300"
    FrontImageSource="scratch_card.jpg"
    RevealThreshold="0.7"
    AutoRevealEnabled="true"
    ScratchCompleted="OnScratchCompleted">

    <freaky:FreakyScratchView.BackContent>
        <Grid BackgroundColor="Purple">
            <Label Text="🎉 You Won!" HorizontalOptions="Center" VerticalOptions="Center" TextColor="White" />
        </Grid>
    </freaky:FreakyScratchView.BackContent>

</freaky:FreakyScratchView>
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `BackContent` | `View` | `null` | The view revealed as the user scratches |
| `FrontImageSource` | `ImageSource` | `null` | Image drawn as the scratchable surface. Supports file, stream, and URI sources. Takes priority over `FrontColor` when set |
| `FrontColor` | `Color` | `Colors.LightGray` | Solid colour used as the scratchable surface when no `FrontImageSource` is set |
| `BrushSize` | `float` | `40` | Scratch brush diameter in canvas pixels |
| `RevealThreshold` | `float` | `0.7` | Fraction of the surface (0.0–1.0) that must be scratched before `ScratchCompleted` fires |
| `AutoRevealEnabled` | `bool` | `true` | Automatically clear the remaining surface and play the reveal animation once the threshold is reached |
| `IsTapToRevealEnabled` | `bool` | `false` | A single tap (without dragging) fully reveals the card |
| `RevealAnimationType` | `ScratchRevealAnimationType` | `FadeOut` | Animation played when the surface is fully revealed — `None`, `FadeOut`, or `Shimmer` |
| `ScratchCompletedCommand` | `ICommand` | `null` | Command executed when `RevealThreshold` is reached |

---

## Events

| Event | Args | Description |
| --- | --- | --- |
| `ScratchCompleted` | `EventArgs` | Fires once when the scratched area reaches `RevealThreshold` |

---

## Methods

| Method | Description |
| --- | --- |
| `Reset()` | Restores the scratch surface to its original state and resets all scratch progress. The cached front image is preserved so it does not reload |
