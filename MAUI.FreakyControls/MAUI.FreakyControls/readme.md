## Installation

Add our [NuGet](https://www.nuget.org/packages/FreakyControls) package or

Run the following command to add our Nuget to your .Net MAUI app:

Install-Package FreakyControls -Version xx.xx.xx

Add the following using statement and Initialization in your MauiProgram:

```C#
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
             // Some of our controls use SkiaSharp and FreakyEffects!
             builder.InitializeFreakyControls(useSkiaSharp: true, useFreakyEffects: true);
             return builder.Build();
         }
      }
```

Now you can use the controls in your app.