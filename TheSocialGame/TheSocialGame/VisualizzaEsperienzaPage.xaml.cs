using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TheSocialGame
{
    public partial class VisualizzaEsperienzaPage : ContentPage
    {
        public Utente user { get; set; }
        public Esperienza exp { get; set; }

        public VisualizzaEsperienzaPage(Utente u, Esperienza e)
        {
            InitializeComponent();
            user = u;
            exp = e;
            Titolo.Text = exp.Titolo;
            Copertina.Source = exp.Copertina;
            if (exp.copertinaLiveIOS) Copertina.Rotation = 90;
            if (exp.DataInizio.Equals(exp.DataFine)) Data.Text = exp.DataInizio.ToString("d MMM yyyy");
            else Data.Text = exp.DataInizio.ToString("d MMM yyyy") + "-" + exp.DataFine.ToString("d MMM yyyy");
            Tipologia.Text = "Tipologia: " + exp.Tipologia;
            costruisciGriglia();
            inizializzaGalleria();
            inizializzaScorrimentoFoto();
            Visualizza.IsVisible = false;
            Galleria.IsVisible = false;
            foto.IsVisible = false;
            EmptyList.IsVisible = false;

        }

        void costruisciGriglia()
        {
            if (exp.Galleria.Count >= 2)
            {
                int primo = new Random().Next(0, exp.Galleria.Count);
                int secondo = new Random().Next(0, exp.Galleria.Count);
                while (primo == secondo)
                    secondo = new Random().Next(0, exp.Galleria.Count);
                SecondariaUno.IsVisible = true;
                SecondariaUno.IsVisible = true;
                Image im = new Image();
                im.Source = ImageSource.FromFile(exp.Galleria[primo]);
                im.Aspect = Aspect.AspectFill;
                im.Scale = 1.5;
                fotoUno.IsClippedToBounds = true;
                fotoUno.Content = im;
                Image imdue = new Image();
                imdue.Source = ImageSource.FromFile(exp.Galleria[secondo]);
                imdue.Aspect = Aspect.AspectFill;
                imdue.Scale = 1.5;
                fotoDue.IsClippedToBounds = true;
                fotoDue.Content = imdue;
            }
            else if (exp.Galleria.Count == 1)
            {
                SecondariaUno.IsVisible = true;
                SecondariaDue.IsVisible = false;
                sostituibileUno.IsVisible = false;
                Image im = new Image();
                im.Source = ImageSource.FromFile(exp.Galleria[0]);
                im.Aspect = Aspect.AspectFill;
                im.Scale = 1.5;
                fotoUno.IsClippedToBounds = true;
                fotoUno.Content = im;
            }
            else
            {
                SecondariaUno.IsVisible = false;
                SecondariaDue.IsVisible = false;
            }

            righePartecipanti.Text = exp.ListaPartecipanti.Count.ToString() + " elementi";
            righeLuoghi.Text = exp.luoghi.Count.ToString() + " elementi";
            righeGalleria.Text = exp.Galleria.Count.ToString() + " elementi";
            righeSlogan.Text = exp.slogan.Count.ToString() + " elementi";
            righeFun.Text = exp.funfacts.Count.ToString() + " elementi";
            righePlaylist.Text = exp.playlist.Count.ToString() + " elementi";
            righeRecensioni.Text = exp.recensioni.Count.ToString() + " elementi";
            righeAltro.Text = exp.altro.Count.ToString() + " elementi";
            righeGalleriasec.Text = exp.Galleria.Count.ToString() + " elementi";
            righeAltrosec.Text = exp.altro.Count.ToString() + " elementi";

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
            await Navigation.PushAsync(new RankingPage());
            Navigation.RemovePage(this);
        }

        async void ProfileClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(user));
            Navigation.RemovePage(this);
        }

        async void BackClicked(Object sender, EventArgs e)
        {

            await Navigation.PopAsync();

        }

        async void importa(Object sender, EventArgs e)
        {

            var foto = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Aggiungi foto"
            });

            if (foto != null)
            {

                exp.Galleria.Add(foto.FullPath);


            }
            inizializzaGalleria();
            scroll.Children.Clear();
            inizializzaScorrimentoFoto();
        }

        void apriPartecipanti(Object sender, EventArgs e)
        {
            EmptyList.IsVisible = false;
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Partecipanti";
            int x = 2;
            foreach (Utente u in exp.ListaPartecipanti)
            {
                Frame f = new Frame();
                if (u.pathFotoProfilo != null)
                {
                    Image im = new Image();
                    im.Source = ImageSource.FromFile(u.pathFotoProfilo);
                    im.Aspect = Aspect.AspectFill;
                    im.Scale = 5;
                    f.Content = im;
                    if (u.fotoLiveiOS) im.Rotation = 90;
                }
                f.BackgroundColor = Color.Orange;
                f.HasShadow = false;
                f.IsClippedToBounds = true;
                f.BorderColor = Color.Black;
                Label l = new Label();
                l.Text = "@" + u.username;
                l.TextColor = Color.Black;
                l.HorizontalOptions = LayoutOptions.StartAndExpand;
                l.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                l.Margin = new Thickness(13);
                layout.Children.Add(f);
                layout.Children.Add(l);
                Grid.SetColumn(f, 0);
                Grid.SetColumn(l, 1);
                Grid.SetRow(f, x);
                Grid.SetRow(l, x);
                RowDefinition riga = new RowDefinition();
                riga.Height = 50;
                layout.RowDefinitions.Add(riga);
                x++;


            }
        }

        async void Chiudi(Object sender, EventArgs e)
        {
            if (Galleria.IsVisible)
            {
                await Galleria.ScaleTo(0);
                Galleria.IsVisible = false;
            }
            else
            {
                await Visualizza.ScaleTo(0);
                layout.Children.Clear();
                layout.Children.Add(nomeschermata);
                layout.Children.Add(Esci);
                layout.Children.Add(EmptyList);
                layout.Children.Add(Aggiungi);
                Visualizza.IsVisible = false;
            }
            costruisciGriglia();

        }


        void apriListaSemplice(List<string> list)
        {
            if (list.Count == 0)
            {
                EmptyList.IsVisible = true;
               
            }
            else
            {
                EmptyList.IsVisible = false;
                int x = 2;
                foreach (string s in list)
                {
                    Label l = new Label();
                    l.Text = s;
                    l.TextColor = Color.Black;
                    l.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    l.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    l.Margin = new Thickness(13);
                    if (x % 2 == 0) l.BackgroundColor = Color.WhiteSmoke;
                    layout.Children.Add(l);
                    Grid.SetColumn(l, 0);
                    Grid.SetRow(l, x);
                    Grid.SetColumnSpan(l, 300);
                    RowDefinition riga = new RowDefinition();
                    riga.Height = 50;
                    layout.RowDefinitions.Add(riga);
                    x++;
                }
               
            }
        }

        void LuoghiClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Luoghi";
            apriListaSemplice(exp.luoghi);
        }

        void SloganClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Slogan";
            apriListaSemplice(exp.luoghi);
        }

        void FunClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Fun Facts";
            apriListaSemplice(exp.luoghi);
        }

        void PlaylistClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Playlist";
            apriListaSemplice(exp.luoghi);
        }

        void RecensioniClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Recensioni";
            apriListaSemplice(exp.luoghi);
        }

        void AltroClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Altro";
            apriListaSemplice(exp.luoghi);
        }

        async void GalleriaClicked(Object sender, EventArgs e)
        {
            Galleria.Scale = 0;
            Galleria.IsVisible = true;
            await Galleria.ScaleTo(1);

        }


        void inizializzaScorrimentoFoto()
        {
           
            foreach (String s in exp.Galleria)
            {
                Frame f = new Frame();
                f.WidthRequest = 340;
                f.BackgroundColor = Color.Black;
                Image im = new Image();
                im.Source = ImageSource.FromFile(s);
                im.Aspect = Aspect.AspectFit;
                f.HasShadow = false;
                im.Scale = 1;
                f.IsClippedToBounds = true;
                f.Content = im;
                scroll.Children.Add(f);
            }
        }

        void ChiudiGalleria(Object sender, EventArgs e)
        {
            foto.IsVisible = false;
        }

        void inizializzaGalleria()
        {
            
            if (exp.Galleria.Count == 0)
            {
                Empty.IsVisible = true;
            }
            else
            {
                Empty.IsVisible = false;
                int col = 0;
                int rig = 2;
                foreach (String s in exp.Galleria)
                {

                    Button b = new Button();
                    b.BackgroundColor = Color.Transparent;
                    b.BorderWidth = 1.5;
                    b.BorderColor = Color.Black;
                    b.Clicked += async (sender, args) =>
                    {
                        foto.IsVisible = true;
                        scrolling.ScrollToAsync(exp.Galleria.IndexOf(s) * scroll.Children[0].Width, 0, false);
                  };
                    Frame f = new Frame();
                    f.BackgroundColor = Color.Black;
                    f.BorderColor = Color.Black;
                    Image im = new Image();
                    im.Source = ImageSource.FromFile(s);
                    im.Aspect = Aspect.AspectFill;
                    f.HasShadow = false;
                    im.Scale = 1.8;
                    f.IsClippedToBounds = true;
                    f.Content = im;
                    layoutGalleria.Children.Add(f);
                    layoutGalleria.Children.Add(b);
                    Grid.SetColumn(f, col);
                    Grid.SetRow(f, rig);
                    Grid.SetColumn(b, col);
                    Grid.SetRow(b, rig);
                    if (col == 2)
                    {

                        RowDefinition prima = new RowDefinition();
                        RowDefinition seconda = new RowDefinition();
                        prima.Height = 100;
                        layoutGalleria.RowDefinitions.Add(prima);
                        rig++;
                        col = 0;


                    }
                    else col++;
                }
            }
        }

        void ForzaFoto(Object sender, EventArgs e)
        {
            double resto = scrolling.ScrollX % scroll.Children[0].Width;
            if(resto != 0)
            {
                if (resto < (scroll.Children[0].Width / 2))
                {
                    scrolling.ScrollToAsync((scrolling.ScrollX / scroll.Children[0].Width) * scroll.Children[0].Width, 0, false);
                } else
                {
                    scrolling.ScrollToAsync(((scrolling.ScrollX / scroll.Children[0].Width) + 1) * scroll.Children[0].Width, 0, false);

                }
            } else return;
        }


        
    }
}
