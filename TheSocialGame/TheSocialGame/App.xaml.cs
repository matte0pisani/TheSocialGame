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
            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            MainPage = new NavigationPage(new WelcomePage());
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
