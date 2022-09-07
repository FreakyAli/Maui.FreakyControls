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

Still want the underline? You can add it using a BoxView (Never planning to add an API for this) :D

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
               
 ImageSource : Accepts a Maui ImageSource wherein you can assign the icon you want to display as a Right of Left ViewMode to your Entry.
 
 ImageAlignment : Align the Image to Right or Left Side of your Entry.
 
 ImageHeight/ImageWidth : Height or Width that you want that Image. 
 
 ImagePadding : The amount of padding you want your Image to have.
 
 ImageCommand/ImageCommandParameter : A command and command parameter for your image's tap event.
 
Note: Since the images are a part of the Entry Control, Giving unreasonable Height/Width or Padding to the Image will distort the Image displyed in your Entry control.
 
 
 # FreakySvgImageView: 
 
 A Freaky ImageView that uses SkiaSharp to display SVG images in your app.
 
 Build action of all SVG images that you want to use needs to be set to `EmbeddedResource`.
 
 
       <freakyControls:FreakySvgImageView
		Base64String="{Binding Base64Data}"
		Command="{Binding OnTapCommand}"
		CommandParameter="{Binding OnTapCommandParam}"
		ImageColor="AliceBlue"
		Tapped="FreakySvgImageView_Tapped"
		SvgAssembly="{x:Static samples:Constants.SvgAssembly}"
		SvgMode="AspectFit"
		ResourceId="{x:Static samples:Constants.DotnetBot}"/>
		
 
  SvgAssembly : Assembly of the Svg image that you want to display in this view. (An Example shown below)
  
  ResourceId : The String path to your Embedded SVG file that you want to display on your View.
  
  Base64String : Your SVG is not a file on your project but a Base64 string i got you homie ;) (Assembly is not needed here)
  
  ImageColor: Using our Control to show icons on your app but your svg image has a different color then you would want, No problemo just use this property to change the display color of the icon, NOTE: This will change the Color of the whole SVG image and not specific parts of it, Only useful in case you wan t to change color of icons based on conditions but dont want multiple icons in your app.
  
  SvgMode: Different modes in which you can display your SVG image wherein it uses Maui's Aspect enum with the same options that you find in `AspectMode` for a regular Maui Image control. 
  

 
 
 
 
 
