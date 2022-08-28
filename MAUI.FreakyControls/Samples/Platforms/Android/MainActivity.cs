using Android.App;
using Android.Content.PM;
using Android.OS;
using MAUI.FreakyControls.Platforms.Android;
namespace Samples;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : FreakyMauiAppCompatActivity
{

}

