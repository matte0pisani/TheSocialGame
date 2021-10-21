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
       
        public LoginPage()
        {
            InitializeComponent();
            //this.IsCredentialsInserted = false;
            BindingContext = this;
        }

        private void SignUpLabel_Tapped(object sender, EventArgs e)
        {

        }

        private void ForgottenPasswordLabel_Tapped(object sender, EventArgs e)
        {

        }

        private bool IsUserValid()
        {
            if (this.PasswordEntry.Text == null || this.UsernameEntry.Text == null) return false;
            return PasswordEntry.Text.Length > 0 && this.UsernameEntry.Text.Length > 0;
        }

        async private void Star_Tapped(object sender, EventArgs e)
        {
            if(IsUserValid())
                await Navigation.PushAsync(new ProfilePage( null )); // provvisoriamente manda null come parametro, poi dovrà mandare il riferimento all'utente che sta accedendo al suo profilo
        }
    }
}