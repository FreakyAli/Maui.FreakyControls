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
        .InitializeFreakyControls()
        .ConfigureFonts(fonts =>
                        {
                            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                        });
             
             // Takes one argument if you would like to init Skiasharp through FreakyControls
             // or not. (Used for RadioButton, Checkbox & SVGImageView)
             // .InitializeFreakyControls();
             
             return builder.Build();
         }
      }
```

Now you can use the controls in your app.
