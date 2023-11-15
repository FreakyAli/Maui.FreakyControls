## An Introduction to Maui.FreakyControls

<div>
   <a href="https://www.nuget.org/packages/FreakyControls"><img src="https://img.shields.io/nuget/v/FreakyControls?color=blue&logo=nuget"></a>
   <a href="https://www.nuget.org/packages/FreakyControls"><img src="https://img.shields.io/nuget/dt/FreakyControls.svg"></a>
   <a href="./LICENSE"><img src="https://img.shields.io/github/license/freakyali/maui.freakycontrols"></a>
   <a href="https://www.codefactor.io/repository/github/freakyali/maui.freakycontrols"><img src="https://www.codefactor.io/repository/github/freakyali/maui.freakycontrols/badge"></a>
   <a href="https://app.fossa.com/projects/git%2Bgithub.com%2FFreakyAli%2FMaui.FreakyControls?ref=badge_shield" alt="FOSSA Status"><img src="https://app.fossa.com/api/projects/git%2Bgithub.com%2FFreakyAli%2FMaui.FreakyControls.svg?type=shield"/></a>
   </div>


## Platforms

| Support       | OS            |
| ------------- |:-------------:|
| iOS             | iOS 15.0 + |
| Android    | API 23+ (Marshmallow) | 


## Documentation
 
For more details and API documentation check our [Wiki](https://github.com/FreakyAli/MAUI.FreakyControls/wiki)

### Like what you saw? Want to keep this repo alive?
[![](https://miro.medium.com/max/600/0*wrBJU05A3BULKcWA.gif)](https://www.buymeacoffee.com/FreakyAli)

## Previews:

| iOS | Android |
| --- | --- |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/ad3e28df-6b94-4eb1-92c7-f4731c28a438" width="250" height="550"/>| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/8e3513e8-d1d3-4c31-b81b-8585042f1605" width="250" height="550"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/45a38c0a-03d1-47f7-bd83-6dfda36abf33" width="250" height="550"/>| <img width="250" height="550" src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/825de592-e70c-48eb-a1d0-a18a00668fab" /> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/bce1ec58-090b-4528-b51d-45a59da5c518" width="250" height="550"/>| <img width="250" height="550" src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/3a16dec7-2569-4fe6-bd86-4b0dd7fdebb3" /> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/d6a2812f-9f55-41fa-9dad-b2be23924c6b" width="250" height="550"/>| <img width="250" height="550" src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/199bf72d-fc6b-46cf-8c1b-d1a87d0a9210" /> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/b16c3d8a-d0e2-4e1e-badd-429f523bc63e" width="250" height="550"/>| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/30baab9e-cdb7-41b6-a33b-d9b324571db4" width="250" height="550"/> |
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/1868d7bf-8f94-47f8-9f15-22c821d41a2c" width="250" height="550"/>| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/ea99a389-e302-4857-bdf2-cda52f4821b2" width="250" height="550"/>| 
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/2cfdbbdb-196c-4721-ba89-8446a8da66e3" width="250" height="550"/>| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/48bc373e-ed13-4ba4-a6bf-f933af9ef150" width="250" height="550"/>| 
| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/f1d1f4db-06fc-4732-a66c-bedf6b6a9393" width="250" height="550"/>| <img src="https://github.com/FreakyAli/Maui.FreakyControls/assets/31090457/319b0342-02c0-4a88-8ee0-fd2c74d519d6" width="250" height="550"/>| 


## License

The license for this project can be found [here](https://github.com/FreakyAli/Maui.FreakyControls/blob/master/LICENSE)



## Installation

Add our [NuGet](https://www.nuget.org/packages/FreakyControls) package or 

Run the following command to add our Nuget to your .Net MAUI app:

      Install-Package FreakyControls -Version xx.xx.xx

Add the following using statement and Initialization in your MauiProgram: 

```c#
using MAUI.FreakyControls.Extensions;
namespace Samples;

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

             //Takes one argument if you would like to init Skiasharp 
             // through FreakyControls or not. (Used for RadioButton, Checkbox & SVGImageView)
             builder.InitializeFreakyControls();
             
             return builder.Build();
         }
      }
```      
      
Now you can use the controls in your app.

## Activity 

Fossa: 

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FFreakyAli%2FMaui.FreakyControls.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FFreakyAli%2FMaui.FreakyControls?ref=badge_large)

Sparkline: 

[![Sparkline](https://stars.medv.io/FreakyAli/Maui.FreakyControls.svg)](https://stars.medv.io/FreakyAli/Maui.FreakyControls)

RepoBeats:

![RepoBeats](https://repobeats.axiom.co/api/embed/37b730ec7020123a37b048636c0babfac3b4a014.svg "Repobeats analytics image")
