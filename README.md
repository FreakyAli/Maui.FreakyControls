## Freaky Controls are your usual Maui Controls but with a freaky twist to it :D

| Support       | OS            |
| ------------- |:-------------:|
| iOS             | iOS 11.0+ |
| Android    | API 23+ (Marshmallow)    | 

Add our [NuGet](https://www.nuget.org/packages/FreakyControls) package or 

Run the following command to add nuget to your .Net MAUI app:

      Install-Package FreakyControls -Version 0.1.0

Adding FreakyControlsHandlers to your MAUI app:

Add the following using statement and then Init the handlers in your MauiProgram: 

      using MAUI.FreakyControls.Extensions;
      
And then in your MauiProgram which would be something like below :       
      
      namespace Something;
      
      public static class MauiProgram
      {     
      var builder = MauiApp.CreateBuilder();
		  builder
			.UseMauiApp<App>()
      .ConfigureMauiHandlers(handlers =>
      {
          handlers.AddFreakyHandlers(); // To Init your freaky handlers for Entry and Editor
      });
                  // This line is needed for the follow issue: https://github.com/mono/SkiaSharp/issues/1979
		  builder.InitSkiaSharp(); // Use this if you want to use FreakySvgImageView 
		  return builder.Build();
      }   
      
Now you can use the controls in your app.

Like what you see? Want to support our repo?

[![](https://miro.medium.com/max/600/0*wrBJU05A3BULKcWA.gif)](https://www.buymeacoffee.com/FreakyAli)
 
For more details and API documentation check our [Wiki](https://github.com/FreakyAli/MAUI.FreakyControls/wiki)
 

 
 
