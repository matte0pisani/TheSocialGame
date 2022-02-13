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
        Utente user { get; set; }
        IAuth auth;

        public SettingPage(Utente u)
        {
            user = u;
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
            App.Current.Resources["BackgroundColor"] = user.Sfondo;
            App.Current.Resources["FirstColor"] = user.Primario;
            App.Current.Resources["SecondColor"] = user.Secondario;
            Username.Text = user.Username;
            Email.Text = auth.GetEmail();
            colori();
            back.Title = user.Sfondo.ToHex();
            prim.Title = user.Primario.ToHex();
            sec.Title = user.Secondario.ToHex();

        }

        void cambiaUsername(Object sender, EventArgs e)
        {
            user.Username = Username.Text;

        }

        void cambiaMail(Object sender, EventArgs e)
        {
            auth.ChangeEmail(Email.Text);

        }

        void cambiaPrivacy(Object sender, EventArgs e)
        {
            if (Privacy.IsToggled)
                user.Privato = true;
            else
                user.Privato = false;
        }

        async void cambiaPassword(Object sender, EventArgs e)
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

        async void NotificationClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());
            Navigation.RemovePage(this);
        }

        async void HomeClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
            Navigation.RemovePage(this);
        }

        async void SearchClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
            Navigation.RemovePage(this);
        }

        async void RankingClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RankingPage(user));
            Navigation.RemovePage(this);
        }

        async void AddClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddExperiencePage(user));
            Navigation.RemovePage(this);
        }

        async void BackClicked(Object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        void cambiaSfondo(Object sender, EventArgs e)
        {
            var button = (Button)sender;
            user.Sfondo = button.BackgroundColor;
            Navigation.PushAsync(new SettingPage(user));
            Navigation.RemovePage(this);

        }

        void cambiaPrimario(Object sender, EventArgs e)
        {
            var button = (Button)sender;
            user.Primario = button.BackgroundColor;
            Navigation.PushAsync(new SettingPage(user));
            Navigation.RemovePage(this);
        }

        void cambiaSecondario(Object sender, EventArgs e)
        {
            var button = (Button)sender;
            user.Secondario = button.BackgroundColor;
            Navigation.PushAsync(new SettingPage(user));
            Navigation.RemovePage(this);
        }


        void colori()
        {
            List<String> col = new List<String>();

            col.Add(Color.AliceBlue.ToHex());
            col.Add(Color.AntiqueWhite.ToHex());
            col.Add(Color.Aqua.ToHex());
            col.Add(Color.Aquamarine.ToHex());
            col.Add(Color.Azure.ToHex());
            col.Add(Color.Beige.ToHex());
            col.Add(Color.Bisque.ToHex());
            col.Add(Color.Black.ToHex());
            col.Add(Color.BlanchedAlmond.ToHex());
            col.Add(Color.Blue.ToHex());
            col.Add(Color.BlueViolet.ToHex());
            col.Add(Color.Brown.ToHex());
            col.Add(Color.BurlyWood.ToHex());
            col.Add(Color.CadetBlue.ToHex());
            col.Add(Color.Chartreuse.ToHex());
            col.Add(Color.Chocolate.ToHex());
            col.Add(Color.Coral.ToHex());
            col.Add(Color.CornflowerBlue.ToHex());
            col.Add(Color.Cornsilk.ToHex());
            col.Add(Color.Crimson.ToHex());
            col.Add(Color.Cyan.ToHex());
            col.Add(Color.DarkBlue.ToHex());
            col.Add(Color.DarkCyan.ToHex());
            col.Add(Color.DarkGoldenrod.ToHex());
            col.Add(Color.DarkGray.ToHex());
            col.Add(Color.DarkGreen.ToHex());
            col.Add(Color.DarkKhaki.ToHex());
            col.Add(Color.DarkMagenta.ToHex());
            col.Add(Color.DarkOliveGreen.ToHex());
            col.Add(Color.DarkOrange.ToHex());
            col.Add(Color.DarkOrchid.ToHex());
            col.Add(Color.DarkRed.ToHex());
            col.Add(Color.DarkSalmon.ToHex());
            col.Add(Color.DarkSeaGreen.ToHex());
            col.Add(Color.DarkSlateBlue.ToHex());
            col.Add(Color.DarkSlateGray.ToHex());
            col.Add(Color.DarkTurquoise.ToHex());
            col.Add(Color.DarkViolet.ToHex());
            col.Add(Color.DeepPink.ToHex());
            col.Add(Color.DeepSkyBlue.ToHex());
            col.Add(Color.DimGray.ToHex());
            col.Add(Color.DodgerBlue.ToHex());
            col.Add(Color.Firebrick.ToHex());
            col.Add(Color.FloralWhite.ToHex());
            col.Add(Color.ForestGreen.ToHex());
            col.Add(Color.Fuchsia.ToHex());
            col.Add(Color.Gainsboro.ToHex());
            col.Add(Color.GhostWhite.ToHex());
            col.Add(Color.Gold.ToHex());
            col.Add(Color.Goldenrod.ToHex());
            col.Add(Color.Gray.ToHex());
            col.Add(Color.Green.ToHex());
            col.Add(Color.GreenYellow.ToHex());
            col.Add(Color.Honeydew.ToHex());
            col.Add(Color.HotPink.ToHex());
            col.Add(Color.IndianRed.ToHex());
            col.Add(Color.Indigo.ToHex());
            col.Add(Color.Ivory.ToHex());
            col.Add(Color.Khaki.ToHex());
            col.Add(Color.Lavender.ToHex());
            col.Add(Color.LavenderBlush.ToHex());
            col.Add(Color.LawnGreen.ToHex());
            col.Add(Color.LemonChiffon.ToHex());
            col.Add(Color.LightBlue.ToHex());
            col.Add(Color.LightCoral.ToHex());
            col.Add(Color.LightCyan.ToHex());
            col.Add(Color.LightGoldenrodYellow.ToHex());
            col.Add(Color.LightGray.ToHex());
            col.Add(Color.LightGreen.ToHex());
            col.Add(Color.LightPink.ToHex());
            col.Add(Color.LightSalmon.ToHex());
            col.Add(Color.LightSeaGreen.ToHex());
            col.Add(Color.LightSkyBlue.ToHex());
            col.Add(Color.LightSlateGray.ToHex());
            col.Add(Color.LightSteelBlue.ToHex());
            col.Add(Color.LightYellow.ToHex());
            col.Add(Color.Lime.ToHex());
            col.Add(Color.LimeGreen.ToHex());
            col.Add(Color.Linen.ToHex());
            col.Add(Color.Magenta.ToHex());
            col.Add(Color.Maroon.ToHex());
            col.Add(Color.MediumAquamarine.ToHex());
            col.Add(Color.MediumBlue.ToHex());
            col.Add(Color.MediumOrchid.ToHex());
            col.Add(Color.MediumPurple.ToHex());
            col.Add(Color.MediumSeaGreen.ToHex());
            col.Add(Color.MediumSlateBlue.ToHex());
            col.Add(Color.MediumSpringGreen.ToHex());
            col.Add(Color.MediumTurquoise.ToHex());
            col.Add(Color.MediumVioletRed.ToHex());
            col.Add(Color.MidnightBlue.ToHex());
            col.Add(Color.MintCream.ToHex());
            col.Add(Color.MistyRose.ToHex());
            col.Add(Color.Moccasin.ToHex());
            col.Add(Color.NavajoWhite.ToHex());
            col.Add(Color.Navy.ToHex());
            col.Add(Color.OldLace.ToHex());
            col.Add(Color.Olive.ToHex());
            col.Add(Color.OliveDrab.ToHex());
            col.Add(Color.Orange.ToHex());
            col.Add(Color.OrangeRed.ToHex());
            col.Add(Color.Orchid.ToHex());
            col.Add(Color.PaleGoldenrod.ToHex());
            col.Add(Color.PaleGreen.ToHex());
            col.Add(Color.PaleTurquoise.ToHex());
            col.Add(Color.PaleVioletRed.ToHex());
            col.Add(Color.PeachPuff.ToHex());
            col.Add(Color.PapayaWhip.ToHex());
            col.Add(Color.Peru.ToHex());
            col.Add(Color.Pink.ToHex());
            col.Add(Color.Plum.ToHex());
            col.Add(Color.PowderBlue.ToHex());
            col.Add(Color.Purple.ToHex());
            col.Add(Color.Red.ToHex());
            col.Add(Color.RosyBrown.ToHex());
            col.Add(Color.RoyalBlue.ToHex());
            col.Add(Color.SaddleBrown.ToHex());
            col.Add(Color.Salmon.ToHex());
            col.Add(Color.SandyBrown.ToHex());
            col.Add(Color.SeaGreen.ToHex());
            col.Add(Color.SeaShell.ToHex());
            col.Add(Color.Sienna.ToHex());
            col.Add(Color.Silver.ToHex());
            col.Add(Color.SkyBlue.ToHex());
            col.Add(Color.SlateBlue.ToHex());
            col.Add(Color.SlateGray.ToHex());
            col.Add(Color.Snow.ToHex());
            col.Add(Color.SpringGreen.ToHex());
            col.Add(Color.SteelBlue.ToHex());
            col.Add(Color.Tan.ToHex());
            col.Add(Color.Teal.ToHex());
            col.Add(Color.Thistle.ToHex());
            col.Add(Color.Tomato.ToHex());
            col.Add(Color.Transparent.ToHex());
            col.Add(Color.Turquoise.ToHex());
            col.Add(Color.Violet.ToHex());
            col.Add(Color.Wheat.ToHex());
            col.Add(Color.White.ToHex());
            col.Add(Color.WhiteSmoke.ToHex());
            col.Add(Color.Yellow.ToHex());
            col.Add(Color.YellowGreen.ToHex());

            back.ItemsSource = col;
            prim.ItemsSource = col;
            sec.ItemsSource = col;





        }

        void sfondo(Object sender, EventArgs e)
        {


            user.Sfondo = Color.FromHex((string)back.SelectedItem);
            Navigation.PushAsync(new SettingPage(user));
            Navigation.RemovePage(this);
        }

        void primario(Object sender, EventArgs e)
        {

            user.Primario = Color.FromHex((string)prim.SelectedItem);
            Navigation.PushAsync(new SettingPage(user));
            Navigation.RemovePage(this);
        }

        void secondario(Object sender, EventArgs e)
        {

            user.Secondario = Color.FromHex((string)sec.SelectedItem);
            Navigation.PushAsync(new SettingPage(user));
            Navigation.RemovePage(this);
        }

        async void elimina(Object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("ELIMINAZIONE", "Inserisci la password per confermare");

            string uid = user.ID;
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

        async void esci(Object sender, EventArgs e)
        {
            var signout = auth.SignOut();
            if (signout)
                await Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(this);
        }
    }
}
