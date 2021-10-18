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
    public partial class LoginPage : ContentPage
    {
        public bool IsCredentialsInserted
        {
            get
            {
                if (Password == null || Username == null) return false;
                return Password.Length > 0 && Username.Length > 0;
            }

        }

        public string Username { get; set; }

        public string Password { get; set; }

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
            if (this.Password == null || this.Username == null) return false;
            return Password.Length > 0 && this.Username.Length > 0;
        }

        async private void Star_Tapped(object sender, EventArgs e)
        {
            if(IsUserValid())
                await Navigation.PushAsync(new Dummy());
        }
    }
}