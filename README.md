<div align="center">

# Maui.FreakyControls

**A free, open-source UI control library for .NET MAUI**

<a href="https://www.nuget.org/packages/FreakyControls"><img src="https://img.shields.io/nuget/v/FreakyControls?color=blue&logo=nuget&style=for-the-badge"></a>
<a href="https://www.nuget.org/packages/FreakyControls"><img src="https://img.shields.io/nuget/dt/FreakyControls?style=for-the-badge"></a>
<a href="./LICENSE"><img src="https://img.shields.io/github/license/freakyali/maui.freakycontrols?style=for-the-badge"></a>
<a href="https://www.codefactor.io/repository/github/freakyali/maui.freakycontrols"><img src="https://img.shields.io/codefactor/grade/github/freakyali/maui.freakycontrols?style=for-the-badge"></a>

<br/>

| iOS | macOS | Android | Windows |
| :---: | :---: | :---: | :---: |
| 14.0+ | 14.0+ | API 23+ | 10.0.17763+ |

<br/>

</div>

---

## Previews

> GIFs do not represent actual performance — clone the repo and run the Samples app for a real feel.

<div align="center">

| iOS | Android |
| :---: | :---: |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/ad3e28df-6b94-4eb1-92c7-f4731c28a438" width="220"/> | <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/8e3513e8-d1d3-4c31-b81b-8585042f1605" width="220"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/45a38c0a-03d1-47f7-bd83-6dfda36abf33" width="220"/> | <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/825de592-e70c-48eb-a1d0-a18a00668fab" width="220"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/bce1ec58-090b-4528-b51d-45a59da5c518" width="220"/> | <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/3a16dec7-2569-4fe6-bd86-4b0dd7fdebb3" width="220"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/d6a2812f-9f55-41fa-9dad-b2be23924c6b" width="220"/> | <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/199bf72d-fc6b-46cf-8c1b-d1a87d0a9210" width="220"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/b16c3d8a-d0e2-4e1e-badd-429f523bc63e" width="220"/> | <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/30baab9e-cdb7-41b6-a33b-d9b324571db4" width="220"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/1868d7bf-8f94-47f8-9f15-22c821d41a2c" width="220"/> | <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/ea99a389-e302-4857-bdf2-cda52f4821b2" width="220"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/2cfdbbdb-196c-4721-ba89-8446a8da66e3" width="220"/> | <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/48bc373e-ed13-4ba4-a6bf-f933af9ef150" width="220"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/f1d1f4db-06fc-4732-a66c-bedf6b6a9393" width="220"/> | <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/319b0342-02c0-4a88-8ee0-fd2c74d519d6" width="220"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/732eb058-7619-4288-a778-d3a670f64c2c" width="220"/> | <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/cf41f49b-30a0-4508-bffc-64ab35ebbc44" width="220"/> |

</div>

---

## Installation

```
dotnet add package FreakyControls
```

Or via Package Manager Console:

```
Install-Package FreakyControls -Version xx.xx.xx
```

### Initialization

In your `MauiProgram.cs`:

```csharp
using Maui.FreakyControls.Extensions;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // useSkiaSharp  → FreakyCheckbox, FreakyRadioButton, FreakySvgImageView
        // useFreakyEffects → touch/ripple effects
        builder.InitializeFreakyControls(useSkiaSharp: true, useFreakyEffects: true);

        return builder.Build();
    }
}
```

---

## Documentation

Full API docs for every control live in the [`docs/`](./docs/) folder.

---

### Like what you saw? Want to keep this repo alive?

<div align="center">

[![Buy Me A Coffee](https://miro.medium.com/max/600/0*wrBJU05A3BULKcWA.gif)](https://www.buymeacoffee.com/FreakyAli)

</div>

---

## License

[MIT](https://github.com/FreakyAli/Maui.FreakyControls/blob/master/LICENSE)

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FFreakyAli%2FMaui.FreakyControls.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FFreakyAli%2FMaui.FreakyControls?ref=badge_large)

---

## Activity

<div align="center">

[![Star History Chart](https://api.star-history.com/svg?repos=FreakyAli/Maui.FreakyControls&type=Date)](https://star-history.com/#FreakyAli/Maui.FreakyControls&type=Date)

![RepoBeats](https://repobeats.axiom.co/api/embed/37b730ec7020123a37b048636c0babfac3b4a014.svg "Repobeats analytics image")

</div>
