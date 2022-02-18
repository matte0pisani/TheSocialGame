using PCLAppConfig;

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
                MainPage = new NavigationPage(new WelcomePage());
            }



          

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
