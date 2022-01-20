using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using PCLAppConfig;
using Plugin.FirebasePushNotification;
using Firebase;

namespace TheSocialGame.Droid
{
    [Activity(Label = "TheSocialGame", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FirebaseApp.InitializeApp(Application.Context);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
          //  ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            LoadApplication(new App());


            FirebasePushNotificationManager.ProcessIntent(this, Intent);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}