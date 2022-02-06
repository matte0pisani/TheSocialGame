using System;
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
            string userID;
            try
            {
               userID = await auth.SignUpWithEmailAndPassword(Mail.Text, passwordEntry.Text);
            }
            catch(Exception)
            {
                await DisplayAlert("ERRORE", "Questa email è già associata ad un account e non può essere utilizzata!", "OK");
                return;
            }

            if (errorLabel.Text.Length == 0 && passwordEntry.Text.Length != 0 && userID != null)
            {
                var signOut = auth.SignOut();
                if (signOut)
                {
                    Utente usr = new Utente
                    {
                        ID = userID,
                        Username = UsernameEntry.Text
                    };

                    if(!await DBmanager.InserisciUtente(usr))
                    {
                        await DisplayAlert("ERRORE", "Qualcosa è andato storto, per favore riprova", "OK");
                        return;
                    }

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