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
        IAuth auth;
        public SignUpPage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var user = auth.SignUpWithEmailAndPassword(Mail.Text, passwordEntry.Text);
            




            if (errorLabel.Text.Length == 0 && passwordEntry.Text.Length != 0 && user != null)
            {
                var signOut = auth.SignOut();
                if (signOut)
                {
                    Utente usr = new Utente();
                    usr.username = UsernameEntry.Text;
                    await DisplayAlert("SUCCESSO", "Il tuo account è stato creato, verifica la tua mail per completare la registrazione!", "OK");
                    await Navigation.PushAsync(new LoginPage());
                } 

                
            }
            else
            {
                await DisplayAlert("ERRORE", "Qualcosa è andato storto, per favore riprova", "OK");
            }
        }

        async void BackClicked(Object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}