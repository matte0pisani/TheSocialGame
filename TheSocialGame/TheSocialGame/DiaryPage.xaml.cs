using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace TheSocialGame
{
    public partial class DiaryPage : ContentPage
    {
        Utente user { get; set; }

       
        public DiaryPage(Utente us)
        {
            InitializeComponent();
            user = us;
            App.Current.Resources["BackgroundColor"] = user.Sfondo;
            App.Current.Resources["FirstColor"] = user.Primario;
            App.Current.Resources["SecondColor"] = user.Secondario;
            List<Esperienza> l = new List<Esperienza>(user.Esperienze);
            l.Reverse();
            visualizzaEsperienze(l);
           
        }




    

    void visualizzaEsperienze(List<Esperienza> list)
    {
            int col = 0;
            int rig = 1;
            if (list.Count == 0)
            {
                NoExp.IsVisible = true;
            }
            else
            {
                NoExp.IsVisible = false;
                foreach (Esperienza e in list)
                {

                    Button b = new Button();
                    b.BackgroundColor = Color.Transparent;
                    b.BorderWidth = 2;
                    b.BorderColor = Color.Black;
                    b.Clicked += async (sender, args) =>
                    {
                        await Navigation.PushAsync(new VisualizzaEsperienzaPage(user, e));
                              Navigation.RemovePage(this);
                       
                    };
                    Frame f = new Frame();
                    f.BackgroundColor = Color.Black;
                    f.BorderColor = Color.Black;
                    Image im = new Image();
                    im.Source = ImageSource.FromStream(() =>
                    {
                        return new MemoryStream(e.Copertina);
                    }); ;
                    im.Aspect = Aspect.AspectFill;
                    f.HasShadow = false;
                    im.Scale = 1.8;
                    f.IsClippedToBounds = true;
                    f.Content = im;
                    if (e.CopertinaLiveiOS) im.Rotation = 90;
                    if (e.Live)
                    {
                        b.BorderWidth = 3;
                        b.BorderColor = Color.LimeGreen;
                    }
                    else
                    {
                        b.BorderWidth = 1;
                    }

                    Label l = new Label();
                    l.Text = e.Titolo;
                    l.TextColor = Color.Black;
                    l.HorizontalOptions = LayoutOptions.Center;
                    l.FontSize = 16;
                    l.FontAttributes = FontAttributes.Bold;
                    Contenuti.Children.Add(f);
                    Contenuti.Children.Add(b);
                    Grid.SetColumn(f, col);
                    Grid.SetRow(f, rig);
                    Grid.SetColumn(b, col);
                    Grid.SetRow(b, rig);
                    Contenuti.Children.Add(l);
                    Grid.SetColumn(l, col);
                    Grid.SetRow(l, rig + 1);

                    if (col == 2)
                    {

                        RowDefinition prima = new RowDefinition();
                        RowDefinition seconda = new RowDefinition();
                        prima.Height = 100;
                        seconda.Height = 30;
                        Contenuti.RowDefinitions.Add(prima);
                        Contenuti.RowDefinitions.Add(seconda);
                        rig += 2;
                        col = 0;


                    }
                    else col++;
                }
            }

        }

        /* Bottoni di Navigazione */
        
      

      

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
            await Navigation.PushAsync(new ProfilePage(user));
            Navigation.RemovePage(this);
        }
    }
}
