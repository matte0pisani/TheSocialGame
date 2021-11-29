using PCLAppConfig;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
<<<<<<< HEAD

            MainPage = new NavigationPage(new WelcomePage());
=======
            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            MainPage = new NavigationPage(new DBtestPage());
>>>>>>> 3b146daf77e207a73bcd5753ddd0743a32c78280
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
