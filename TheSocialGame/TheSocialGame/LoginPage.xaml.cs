using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        IAuth auth;
        public bool IsEmailSet { get; set; }
        public bool IsPasswordSet { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
            StarImage.IsVisible = false;
            IsEmailSet = false;
            IsPasswordSet = false;
        }

        /**
        private bool IsUserValid()
        {
            return IsEmailSet && IsPasswordSet;
        }
        */

        private void RefreshStarVisibility()
        {
            StarImage.IsVisible = IsEmailSet && IsPasswordSet;
        }

        private async void Star_Tapped(object sender, EventArgs e)
        {

            string userID = await auth.LoginWithEmailAndPassword(MailEntry.Text, PasswordEntry.Text);
            if(userID != string.Empty)
            {
                if (auth.MailVerificata())
                {
                    Utente usr = new Utente();
                    await Navigation.PushAsync(new ProfilePage(usr));
                } else
                {
                    await DisplayAlert("AUTENTICAZIONE FALLITA", "Verifica la tua mail e riprova!", "OK");
                }
            } else
            {
                await DisplayAlert("AUTENTICAZIONE FALLITA", "L' email o la password non sono corrette, per favore riprova", "OK");
            }





          
        }

        private void UsernameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsEmailSet = MailEntry.Text.Length > 0;
            RefreshStarVisibility();
        }

        private void PasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsPasswordSet = PasswordEntry.Text.Length > 0;
            RefreshStarVisibility();
        }
        private async void SignUpLabel_Tapped(object sender, EventArgs e)
        {
            var signout = auth.SignOut();
            if(signout)
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void ForgottenPasswordLabel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgottenPasswordPage());
          
        }

    }
}