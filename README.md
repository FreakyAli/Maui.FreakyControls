# Freaky Controls are your usual Maui Controls but with a freaky twist to it :D

Colons can be used to align columns.

| Support       | OS            |
| ------------- |:-------------:|
| iOS             | iOS 11.0+ |
| Android    | 23+ (Marshmallow)    | 



Add the [NuGet](https://www.nuget.org/packages/FreakyControls) package or 

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
 
# FreakyEntry: 

The default underline of your Android Entry/Editor won't be annoying you anymore ;) 

Still want the underline? You can add it using a BoxView (Never planning to add an API for this) :D

FreakyEditor for now just removes the pesky default underline and gives you an option to disable copy paste funcationality , everything else is just the same as your regular MAUI Editor. (More features will be implemented on demand)

Main feature for Freaky Entry is adding Left and Right Drawable Images with Padding and Commands wired with it.
      
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
               
 ***ImageSource*** : Accepts a Maui ImageSource wherein you can assign the icon you want to display as a Right of Left ViewMode to your Entry.
 
 ***ImageAlignment*** : Align the Image to Right or Left Side of your Entry.
 
 ***ImageHeight/ImageWidth*** : Height or Width that you want that Image. 
 
 ***ImagePadding*** : The amount of padding you want your Image to have.
 
 ***ImageCommand/ImageCommandParameter*** : A command and command parameter for your image's tap event.
 
 ***AllowCopyPaste*** : Disable the copy paste long click funcationality in your freaky entry 
 
 **Please know, Our control's changes will get overriden if you use "ClearButtonVisibility" property's WhileEditing option.**
 
**Note: Since the images are a part of the Entry Control, Giving unreasonable Height/Width or Padding to the Image will distort the Image displayed in your Entry control. Keep these values relative to your entry size.**


 # FreakyTextInputLayout:
 
 A Freaky TextInputLayout that also has all the freaky features from our Entry control and more.
 
 ***BorderType** : An enum which let's you choose whether you would like A Full, Underlined or No borders. NOTE: This is to be used before adding Border/Underline details.
 
 **If you select Full:**
 
 ***BorderStroke*** : BorderStroke, of type Brush, indicates the brush used to paint the border.

 ***BorderStrokeThickness***: BorderStrokeThickness, of type double, indicates the width of the border.

 ***BorderCornerRadius***: BorderCornerRadius property is set to a CornerRadius Type.
 
       <freakyControls:FreakyTextInputLayout
                x:Name="yolo"
                FontSize="Large"
                ImageSource="calendar"
                ImageHeight="{OnPlatform 25, iOS=25, Android=40}"
                ImageWidth="{OnPlatform 25, iOS=25, Android=40}"
                ImagePadding="10"
                ImageCommand="{Binding ImageWasTappedCommand}"
                UnderlineColor="Black"
                BorderType="Underline"
                UnderlineThickness="1.5"
                Title="Underlined TextInputLayout"/>
 
 
 **If you select Underline:**
 
 ***UnderlineColor***: Color of the underline drawn for the TIL
 
 ***UnderlineThickness***: Underline thickness double, that decides the thickness for your underline 
 
 
      <freakyControls:FreakyTextInputLayout
                TextChanged="FreakyTextInputLayout_TextChanged"
                FontSize="Large"
                BorderStroke="Black"
                BorderType="Full"
                BorderCornerRadius="10"
                BorderStrokeThickness="2"
                ImageSource="calendar"
                ImageHeight="{OnPlatform 25, iOS=25, Android=40}"
                ImageWidth="{OnPlatform 25, iOS=25, Android=40}"
                ImagePadding="10"
                ImageCommand="{Binding ImageWasTappedCommand}"
                Title="Bordered TextInputLayout"/>
		
		
 
 ***Title***: Title/Placeholder for your TIL.
 
 ***TitleColor***: Text color for your Title.
 
 
 
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
		
 
  ***SvgAssembly*** : Assembly of the Svg image that you want to display in this view. (An Example shown below)
  
  ***ResourceId*** : The String path to your Embedded SVG file that you want to display on your View.
  
  ***Base64String*** : Your SVG is not a file on your project but a Base64 string i got you homie ;) (Assembly is not needed here)
  
  ***ImageColor***: Using our Control to show icons on your app but your svg image has a different color then you would want, No problemo just use this property to change the display color of the icon, NOTE: This will change the Color of the whole SVG image and not specific parts of it, Only useful in case you wan t to change color of icons based on conditions but dont want multiple icons in your app.
  
  ***SvgMode***: Different modes in which you can display your SVG image wherein it uses Maui's Aspect enum with the same options that you find in `AspectMode` for a regular Maui Image control. 
  
  ***Command/CommandParameter***: Your usual combo of MVVM love.
  
  ***Tapped***: A tap event in case MVVM is too boring for you :D
  
  
  Example of a constant class that provides Assembly and ResourceId to the ImageView:
  
       public static class Constants
	{
	   public static readonly Assembly SvgAssembly = typeof(Constants).Assembly;
	   public static readonly string ResourcePath = "Samples.Resources.Images.";
	   public static readonly string DotnetBot = ResourcePath+ "dotnet_bot.svg";
	}
	
Still confused? Don't Worry just check the Sample project for how to configure this :) 
  
  
 
 
 
 
 
