# MAUI.FreakyControls

# Freaky Controls (iOS/Android) are your usual Maui Controls but a with a freaky twist to it :D

Adding FreakyControlsHandlers to your MAUI app:

Add the following using statement and then Init the handlers in your MauiProgram: 

      using MAUI.FreakyControls.Extensions;
      
      namespace Something;
      
      public static class MauiProgram
      {     
      var builder = MauiApp.CreateBuilder();
		  builder
			.UseMauiApp<App>()
      .ConfigureMauiHandlers(handlers =>
      {
          handlers.AddFreakyHandlers(); // To Init your freaky handlers
      });
		  builder.InitSkiaSharp(); // Use this if you want to use FreakySvgImageView 
		  return builder.Build();
      }
      
Now you can use the controls in your app.
 
# FreakyEntry: 

The default underline of your Android Entry won't be annoying you anymore ;) 

Still want the underline? You can add it using a BoxView :D

Main feature for Freaky Entry is adding Left and Right Drawable Images with Padding and Commands wired with it.

      xmlns:freakyControls="clr-namespace:MAUI.FreakyControls;assembly=MAUI.FreakyControls"
      
          <freakyControls:FreakyEntry
               Placeholder="This is a freaky entry with an image"
               ImagePadding="10" 
               ImageCommand="{Binding ImageWasTappedCommand}"
               ImageCommandParameter="{Binding CommandParam}"
               ImageHeight="{OnPlatform 25, iOS=25, Android=40}"
               ImageWidth="{OnPlatform 25, iOS=25, Android=40}"
               ImageAlignment="Right"
               ImageSource="calendar"
               Keyboard="Chat"/>
               
 ImageSource: Accepts a Maui ImageSource wherein you can assign the icon you want to display as a Right of Left ViewMode to your Entry.
 
 ImageAlignment: Align the Image to Right or Left Side of your Entry.
 
 ImageHeight/ImageWidth : Height or Width that you want that Image.
 
 ImagePadding : The amount of padding you want your Image to have.
 
 ImageCommand/ImageCommandParameter: A command and command parameter for your image's tap event.
 
 
 
 
 
