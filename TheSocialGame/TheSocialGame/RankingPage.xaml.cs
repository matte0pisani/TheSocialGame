using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TheSocialGame
{
    public partial class RankingPage : ContentPage
    {
        Utente user;

        public RankingPage(Utente us)
        {
            InitializeComponent();
            user = us;
            App.Current.Resources["BackgroundColor"] = user.Sfondo;
            App.Current.Resources["FirstColor"] = user.Primario;
            App.Current.Resources["SecondColor"] = user.Secondario;

            generali.BackgroundColor = user.Sfondo;
            generali.TextColor = Color.Black;
            esperienze.BackgroundColor = user.Secondario;
            esperienze.Opacity = 0.4;
            esperienze.TextColor = Color.DarkGray;
            inizializzaGriglia();
          

        }

        void inizializzaGriglia()
        {
            Dictionary<Utente, int> classifica = user.ClassificaGenerale();
            int prog = 1;
            foreach (Utente u in classifica.Keys)
            {
                Label num = new Label();
                num.Text = prog.ToString();
                num.TextColor = Color.Black;
                num.FontAttributes = FontAttributes.Bold;
                num.FontSize = 30;
                num.Margin = 10;
                Label name = new Label();
                name.Text = "@" + u.Username;
                if (u == user) name.TextColor = user.Primario;
                else name.TextColor = user.Secondario;
                name.FontAttributes = FontAttributes.Bold;
                name.FontSize = 30;
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        name.Margin = 10;
                        break;
                    case Device.iOS:
                        name.Margin = -7;
                        break;
                }
                Label punti = new Label();
                punti.Text = classifica[u].ToString();
                punti.TextColor = Color.Black;
                punti.FontAttributes = FontAttributes.Bold;
                punti.FontSize = 30;
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        punti.Margin = 10;
                        break;
                    case Device.iOS:
                        punti.Margin = -7;
                        break;
                }
                    
                Griglia.Children.Add(num);
                Griglia.Children.Add(name);
                Griglia.Children.Add(punti);
                Grid.SetColumn(num, 0);
                Grid.SetColumn(name, 1);
                Grid.SetColumn(punti, 2);
                Grid.SetRow(num, (prog - 1));
                Grid.SetRow(name, (prog - 1));
                Grid.SetRow(punti, (prog - 1));
                prog++;
                RowDefinition riga = new RowDefinition();
                riga.Height = 70;
                Griglia.RowDefinitions.Add(riga);

            }
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

        async void AddClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddExperiencePage(user));

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

        async void SettingClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingPage(user));

        }

        async void BackClicked(Object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        void generaliClicked(Object sender, EventArgs e)
        {
            generali.BackgroundColor = user.Sfondo;
            generali.TextColor = Color.Black;
            esperienze.BackgroundColor = user.Secondario;
            esperienze.Opacity = 0.4;
            generali.Opacity = 100;
            esperienze.BackgroundColor = Color.LightGray;
            esperienze.TextColor = Color.DarkGray;
            Griglia.Children.Clear();
            Dictionary<Utente, int> classifica = user.ClassificaGenerale();
            int prog = 1;
            foreach (Utente u in classifica.Keys)
            {
                Label num = new Label();
                num.Text = prog.ToString();
                num.TextColor = Color.Black;
                num.FontAttributes = FontAttributes.Bold;
                num.FontSize = 30;
                num.Margin = 10;
                Label name = new Label();
                name.Text = "@" + u.Username;
                if (u==user) name.TextColor = user.Primario;
                else name.TextColor = user.Secondario;
                name.FontAttributes = FontAttributes.Bold;
                name.FontSize = 30;
                name.Margin = 10;
                Label punti = new Label();
                punti.Text = classifica[u].ToString();
                punti.TextColor = Color.Black;
                punti.FontAttributes = FontAttributes.Bold;
                punti.FontSize = 30;
                punti.Margin = 10;
                Griglia.Children.Add(num);
                Griglia.Children.Add(name);
                Griglia.Children.Add(punti);
                Grid.SetColumn(num, 0);
                Grid.SetColumn(name, 1);
                Grid.SetColumn(punti, 2);
                Grid.SetRow(num, (prog - 1));
                Grid.SetRow(name, (prog - 1));
                Grid.SetRow(punti, (prog - 1));
                prog++;

            }

        }

        void esperienzeClicked(Object sender, EventArgs e)
        {
           esperienze.BackgroundColor = user.Sfondo;
            esperienze.TextColor = Color.Black;
            generali.BackgroundColor = user.Secondario;
            generali.Opacity = 0.4;
            generali.BackgroundColor = Color.LightGray;
            generali.TextColor = Color.DarkGray;
            esperienze.Opacity = 100;
            Griglia.Children.Clear();
            Dictionary<Utente, int> classifica = user.ClassificaEsperienze();
            int prog = 1;
            foreach (Utente u in classifica.Keys)
            {
                Label num = new Label();
                num.Text = prog.ToString();
                num.TextColor = Color.Black;
                num.FontAttributes = FontAttributes.Bold;
                num.FontSize = 30;
                num.Margin = 10;
                Label name = new Label();
                name.Text = "@" + u.Username;
                if (u==user) name.TextColor = user.Primario;
                else name.TextColor = user.Secondario;
                name.FontAttributes = FontAttributes.Bold;
                name.FontSize = 30;
                name.Margin = 10;
                Label punti = new Label();
                punti.Text = classifica[u].ToString();
                punti.TextColor = Color.Black;
                punti.FontAttributes = FontAttributes.Bold;
                punti.FontSize = 30;
                punti.Margin = 10;
                Griglia.Children.Add(num);
                Griglia.Children.Add(name);
                Griglia.Children.Add(punti);
                Grid.SetColumn(num, 0);
                Grid.SetColumn(name, 1);
                Grid.SetColumn(punti, 2);
                Grid.SetRow(num, (prog - 1));
                Grid.SetRow(name, (prog - 1));
                Grid.SetRow(punti, (prog - 1));
                prog++;

            }
        }



    }
}
