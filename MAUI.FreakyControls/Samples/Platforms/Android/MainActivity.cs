using Android.App;
using Android.Content.PM;
using Android.OS;
using Maui.FreakyControls.Platforms.Android;
using System.Diagnostics;
using Color = Android.Graphics.Color;

namespace Samples;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : FreakyMauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        this.Window.SetNavigationBarColor(Color.Black);
    }
}