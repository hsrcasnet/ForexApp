using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ForexApp.Droid
{
    [Activity(Label = "ForexApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            var platformInitializer = new AndroidPlatformInitializer(); 

            global::Xamarin.Forms.Forms.Init(this, bundle);
            this.LoadApplication(new App(platformInitializer));
        }
    }
}

