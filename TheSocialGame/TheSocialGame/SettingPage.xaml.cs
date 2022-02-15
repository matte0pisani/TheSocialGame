using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using FirebaseAdmin;
using FirebaseAdmin.Auth;

namespace TheSocialGame
{
    public partial class SettingPage : ContentPage
    {
        public Utente User { get; set; }
        private IAuth auth;
        public bool Dirty { get; set; }

        public SettingPage(Utente u)
        {
            User = u;
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
            App.Current.Resources["BackgroundColor"] = User.Sfondo;
            App.Current.Resources["FirstColor"] = User.Primario;
            App.Current.Resources["SecondColor"] = User.Secondario;
            Username.Text = User.Username;
            Email.Text = auth.GetEmail();
          
            Privacy.IsToggled = User.Privato;
            Dirty = false;

        }

        private async void CambiaUsername(Object sender, EventArgs e)
        {
            User.Username = Username.Text;
            if (!await DBmanager.AggiornaUtenteInfoNonExp(User))
            {
                await DisplayAlert("ERRORE", "Questo username è già associato ad un account e non può essere utilizzato!", "OK");
            }
        }

        private void CambiaMail(Object sender, EventArgs e)
        {
            auth.ChangeEmail(Email.Text);
        }

        private void CambiaPrivacy(Object sender, EventArgs e)
        {
            if (Privacy.IsToggled)
                User.Privato = true;
            else
                User.Privato = false;
            Dirty = true;
        }

        private async void CambiaPassword(Object sender, EventArgs e)
        {
            string vecchia = await DisplayPromptAsync("CAMBIA PASSWORD", "Inserisci la tua vecchia password");
            if (vecchia == null) { return; }

            if (auth.ValidPassword(vecchia))
            {
                string nuova = await DisplayPromptAsync("CAMBIA PASSWORD", "Inserisci la nuova password");
                if (nuova == null) { return; }
                string conferma = await DisplayPromptAsync("CAMBIA PASSWORD", "Inserisci di nuovo la nuova password");
                if (conferma == null) { return; }

                if (nuova.Equals(conferma))
                {
                    if (auth.ChangePassword(nuova))
                    {
                        await DisplayAlert("SUCCESSO", "La tua password è stata modificata con successo!", "OK");
                    }
                    else
                        await DisplayAlert("ERRORE", "Qualcosa è andato storto", "OK");
                }
                else
                    await DisplayAlert("ERRORE", "Qualcosa è andato storto: le password non corrispondono", "OK");
            }
            else await DisplayAlert("ERRORE", "Qualcosa è andato storto: la password è errata", "OK");
        }

        private async void NotificationClicked(Object sender, EventArgs e)
        {
            if (Dirty) DBmanager.AggiornaUtenteInfoNonExp(User);
            await Navigation.PushAsync(new NotificationPage());
            Navigation.RemovePage(this);
        }

        private async void HomeClicked(Object sender, EventArgs e)
        {
            if (Dirty) DBmanager.AggiornaUtenteInfoNonExp(User);
            await Navigation.PushAsync(new HomePage());
            Navigation.RemovePage(this);
        }

        private async void SearchClicked(Object sender, EventArgs e)
        {
            if (Dirty) DBmanager.AggiornaUtenteInfoNonExp(User);
            await Navigation.PushAsync(new SearchPage());
            Navigation.RemovePage(this);
        }

        private async void RankingClicked(Object sender, EventArgs e)
        {
            if (Dirty) DBmanager.AggiornaUtenteInfoNonExp(User);
            await Navigation.PushAsync(new RankingPage(User));
            Navigation.RemovePage(this);
        }

        private async void AddClicked(Object sender, EventArgs e)
        {
            if (Dirty)  DBmanager.AggiornaUtenteInfoNonExp(User);
            await Navigation.PushAsync(new AddExperiencePage(User));
            Navigation.RemovePage(this);
        }

        private async void BackClicked(Object sender, EventArgs e)
        {
            if (Dirty)  DBmanager.AggiornaUtenteInfoNonExp(User);
            await Navigation.PopAsync();
        }

        private async void cambiaTema(Object sender, EventArgs e)
        {
            if (Default.IsChecked)
            {
               
                User.Sfondo = Color.BlanchedAlmond;
                User.Primario = Color.DarkOrange;
                User.Secondario = Color.Chocolate;
               
            }
            if (Caldo.IsChecked)
            {
                User.Sfondo = Color.Bisque;
                User.Primario = Color.OrangeRed;
                User.Secondario = Color.Coral;
               
            }
            if(Freddo.IsChecked)
            {
                User.Sfondo = Color.AliceBlue;
                User.Primario = Color.LightSkyBlue;
                User.Secondario = Color.DodgerBlue;

            }
            if(Tranquillo.IsChecked) {
                User.Sfondo = Color.Azure;
                User.Primario = Color.LightSeaGreen;
                User.Secondario = Color.MediumTurquoise;

            }
            if(Vivace.IsChecked)
            {
                User.Sfondo = Color.MistyRose;
                User.Primario = Color.Plum;
                User.Secondario = Color.MediumVioletRed;

            }
            if(Elegante.IsChecked)
            {
                User.Sfondo = Color.Lavender;
                User.Primario = Color.LightSteelBlue;
                User.Secondario = Color.MediumSlateBlue;

            }
            if(Divertente.IsChecked)
            {
                User.Sfondo = Color.LavenderBlush;
                User.Primario = Color.MediumOrchid;
                User.Secondario = Color.Magenta;

            }

            await Navigation.PushAsync(new SettingPage(User));
            Navigation.RemovePage(this);
        }

        private async void Elimina(Object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("ELIMINAZIONE", "Inserisci la password per confermare");

            string uid = User.ID;
            bool aut = auth.DeleteUser(result);
            if (aut)
            {
                DBmanager.EliminaUtente(uid);
                await DisplayAlert("SUCCESSO", "Il tuo account è stato eliminato con successo", "OK");
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                await DisplayAlert("ERRORE", "password errata", "OK");
            }




        }

        private async void Esci(Object sender, EventArgs e)
        {
            var signout = auth.SignOut();
            if (signout)
                await Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(this);
        }
    }
}
