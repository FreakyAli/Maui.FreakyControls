## Freaky Controls are your usual Maui Controls but with a freaky twist to it :D

[![FreakyControls NuGet](https://img.shields.io/nuget/vpre/FreakyControls.svg?label=FreakyControls%20NuGet)](https://www.nuget.org/packages/FreakyControls)

| Support       | OS            |
| ------------- |:-------------:|
| iOS             | iOS 11.0 + |
| Android    | API 23+ (Marshmallow)    | 

## Documentation
 
For more details and API documentation check our [Wiki](https://github.com/FreakyAli/MAUI.FreakyControls/wiki)

## License 

The license for this project can be found [here](https://github.com/FreakyAli/Maui.FreakyControls/blob/master/LICENSE)

## Installation

Add our [NuGet](https://www.nuget.org/packages/FreakyControls) package or 

Run the following command to add nuget to your .Net MAUI app:

      Install-Package FreakyControls -Version xx.xx.xx

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

## Previews:

| iOS | Android |
| --- | --- |
| <img src="https://user-images.githubusercontent.com/31090457/195843558-dbc2c1d2-939e-49fd-829f-1ac999ef732f.gif" width="200" height="450"/>| <img src="https://user-images.githubusercontent.com/31090457/195855656-60d8d113-8748-44d5-a13b-5b972ffa304e.gif" width="200" height="450"/> |
| <img src="https://user-images.githubusercontent.com/31090457/195860576-1d5566ac-c4dc-41d0-9c1f-38338d9192c7.gif" width="200" height="450"/>| <img width="200" height="450" src="https://user-images.githubusercontent.com/31090457/195860338-d7286795-2c4a-4e7f-be91-0ff7f01a7747.gif" /> |
| <img src="https://user-images.githubusercontent.com/31090457/195864114-5a32df8c-32aa-4c42-850d-398a54babca3.gif" width="200" height="450"/>| <img width="200" height="450" src="https://user-images.githubusercontent.com/31090457/195864040-2f37c110-92b3-47af-af2a-b50d895a77b5.gif" /> |
| <img src="https://user-images.githubusercontent.com/31090457/195867103-37d65de1-6e39-42d9-9c98-705e49f4bc88.gif" width="200" height="450"/>| <img width="200" height="450" src="https://user-images.githubusercontent.com/31090457/195866605-20bf6373-53a3-44d9-9fde-c442ee1aec70.gif" /> |

