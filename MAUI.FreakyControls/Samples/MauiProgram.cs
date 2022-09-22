using Maui.FreakyControls.Extensions;

// this is to avoid the following https://docs.microsoft.com/en-us/dotnet/maui/xaml/xamlc
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
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
			})
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddFreakyHandlers(); 
            });
		builder.InitSkiaSharp();

		var mauiApp = builder.Build();
		var fontManager = mauiApp.Services.GetService<IFontManager>();
		return mauiApp;
	}
}

