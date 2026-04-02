# FreakySvgImageView

Renders inline SVG assets from embedded resources, Base64 strings, or URIs. Supports tint colour and a tap command.

> Requires `useSkiaSharp: true` in `InitializeFreakyControls`.

**Platforms:** iOS, macOS, Android, Windows

---

## XAML Usage

```xml
xmlns:freaky="clr-namespace:Maui.FreakyControls;assembly=Maui.FreakyControls"

<!-- From an embedded resource -->
<freaky:FreakySvgImageView
    ResourceId="MyApp.Resources.Icons.logo.svg"
    SvgAssembly="{x:Static local:App.AssemblyRef}"
    ImageColor="DodgerBlue"
    SvgMode="AspectFit"
    Command="{Binding LogoTappedCommand}" />

<!-- From a Base64 string -->
<freaky:FreakySvgImageView
    Base64String="{Binding SvgData}"
    ImageColor="SeaGreen" />
```

---

## Properties

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `ResourceId` | `string` | `null` | Fully-qualified embedded resource path |
| `Base64String` | `string` | `null` | Base64-encoded SVG string |
| `Uri` | `Uri` | `null` | Remote or local file URI |
| `SvgAssembly` | `Assembly` | `null` | Assembly that contains the embedded resource |
| `ImageColor` | `Color` | `Transparent` | Tint colour; `Transparent` = no tint |
| `SvgMode` | `Aspect` | `AspectFit` | Scale/fit mode |
| `Command` | `ICommand` | `null` | Executes when the image is tapped |
| `CommandParameter` | `object` | `null` | Parameter passed to `Command` |

---

## Events

| Event | Description |
| --- | --- |
| `Tapped` | Fires when the image is tapped |
