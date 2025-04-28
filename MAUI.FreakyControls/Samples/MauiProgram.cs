using CommunityToolkit.Maui;
using Maui.FreakyControls.Extensions;
using MemoryToolkit.Maui;
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
        builder.UseMauiCommunityToolkit();
        builder.InitializeFreakyControls();
#if DEBUG    
        // Ensure UseLeakDetection is called after logging has been configured!
        builder.UseLeakDetection(collectionTarget =>
        {
            var MainPage = Application.Current.Windows.FirstOrDefault()?.Page;
            MainPage?.DisplayAlert("Leak Detected",
                $"❗🧟❗{collectionTarget.Name} is a zombie!", "OK");
        });
#endif
        return builder.Build();
    }
}