using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (errorLabel.Text.Length == 0 && passwordEntry.Text.Length != 0)
            {
                Utente usr = new Utente();
                usr.username = UsernameEntry.Text;
                usr.mail = Mail.Text;
                usr.password = passwordEntry.Text;
                await Navigation.PushAsync(new ProfilePage(usr));
            }
        }

        async void BackClicked(Object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}