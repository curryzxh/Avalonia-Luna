using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace Luna.Mobile.Sample;

[Activity(
    Label = "Luna Mobile Sample",
    Theme = "@style/MyTheme.NoActionBar",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity
{
}
