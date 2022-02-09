using PCLAppConfig;
using Plugin.FirebasePushNotification;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame
{
    public partial class App : Application
    {
        private IAuth auth;

        public App()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
            if (auth.SignIn())
            {
                string userID = auth.GetCurrentUserId();
                MainPage = new NavigationPage(new ProfilePage(userID));
            }
            else
            {
                MainPage = new NavigationPage(new DBstuff.DBmanagerTestPage());
            }



            CrossFirebasePushNotification.Current.Subscribe("all");
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;



        }


        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Token: {e.Token}");
        }






        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
