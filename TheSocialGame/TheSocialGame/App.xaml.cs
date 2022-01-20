using PCLAppConfig;
using Plugin.FirebasePushNotification;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame
{
    public partial class App : Application
    {
        IAuth auth;

        public App()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
            if (auth.SignIn())
            {
                MainPage = new ProfilePage(new Utente()); // capire come risalirea ad utente associato
            }
            else
            {
                MainPage = new NavigationPage(new WelcomePage());
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
