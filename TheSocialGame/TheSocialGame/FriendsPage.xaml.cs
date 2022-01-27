using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace TheSocialGame
{
    public partial class FriendsPage : ContentPage
    {
        public Utente user { get; set; }

        public FriendsPage(Utente us)
        {
            InitializeComponent();
            user = us;
            App.Current.Resources["BackgroundColor"] = user.Sfondo;
            App.Current.Resources["FirstColor"] = user.Primario;
            App.Current.Resources["SecondColor"] = user.Secondario;
            inserisci();

        }


        async void BackClicked(Object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public void inserisci()
        {
            int x = 0;
            foreach (Utente u in user.Amici.Keys)
            {
                Frame f = new Frame();
                if (u.FotoBytes != null)
                {
                    Image im = new Image();
                    im.Source = ImageSource.FromStream(() =>
                    {
                        return new MemoryStream(u.FotoBytes);
                    });
                    im.Aspect = Aspect.AspectFill;
                    im.Scale = 5;
                    f.Content = im;
                    if (u.FotoLiveiOS) im.Rotation = 90;
                }
                f.BackgroundColor = user.Secondario;
                f.HasShadow = false;
                f.IsClippedToBounds = true;
                f.BorderColor = Color.Black;
                Label l = new Label();
                l.Text = "@" + u.Username;
                l.TextColor = Color.Black;
                l.Margin = new Thickness(20, 0);
                l.FontSize = 25;
                Label s = new Label();
                s.Text = user.Amici[u].ToString() + " esperienze insieme";
                s.TextColor = Color.SlateGray;
                s.FontSize = 15;
                s.Margin = new Thickness(20, 30);
                Griglia.Children.Add(f);
                Griglia.Children.Add(l);
                Griglia.Children.Add(s);
                Grid.SetColumn(f, 0);
                Grid.SetColumn(l, 1);
                Grid.SetColumn(s, 1);
                Grid.SetRow(f, x);
                Grid.SetRow(l, x);
                Grid.SetRow(s, x);
                x++;
                RowDefinition riga = new RowDefinition();
                riga.Height = 90;
                Griglia.RowDefinitions.Add(riga);
            }


            }
        }
    }

