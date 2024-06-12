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

## Breaking Changes in this version!!

Post v0.5.0-pre there will be some breaking changes made to the namespace structure of our controls,
anywhere the namespace had the Shared folder mentioned in it, shall be removed, together with the folder itself
So the new namespace would be as shown below:

```C#

using Maui.FreakyControls.Shared.Enums; //old namespace
using Maui.FreakyControls.Enums; // new namespace

```

The above applies to all the other sub-folders as well.

Deprecated API's from the previous version have also been removed!