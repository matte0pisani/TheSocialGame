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

        public bool IsUsernameSet { get; set; }
        public bool IsPasswordSet { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            StarImage.IsVisible = false;
            IsUsernameSet = false;
            IsPasswordSet = false;
        }

        private bool IsUserValid()
        {
            return IsUsernameSet && IsPasswordSet;
        }

        private void RefreshStarVisibility()
        {
            StarImage.IsVisible = IsUsernameSet && IsPasswordSet;
        }

        private async void Star_Tapped(object sender, EventArgs e)
        {
            if (IsUserValid())
            {
                Utente usr = new Utente();
                usr.username = UsernameEntry.Text;
                await Navigation.PushAsync(new ProfilePage(usr));
            }
        }

        private void UsernameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsUsernameSet = UsernameEntry.Text.Length > 0;
            RefreshStarVisibility();
        }

        private void PasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsPasswordSet = PasswordEntry.Text.Length > 0;
            RefreshStarVisibility();
        }
        private async void SignUpLabel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void ForgottenPasswordLabel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgottenPasswordPage());
            Navigation.RemovePage(this);
        }

    }
}