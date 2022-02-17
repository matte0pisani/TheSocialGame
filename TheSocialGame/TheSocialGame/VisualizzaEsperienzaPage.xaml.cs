using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

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
            App.Current.Resources["BackgroundColor"] = user.Sfondo;
            App.Current.Resources["FirstColor"] = user.Primario;
            App.Current.Resources["SecondColor"] = user.Secondario;

            exp = e;
            Titolo.Text = exp.Titolo;
            Copertina.Source = ImageSource.FromStream(() =>
            {
                return new MemoryStream(exp.Copertina);
            }); ;
            if (exp.CopertinaLiveiOS) Copertina.Rotation = 90;
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
            importaCopertina.IsVisible = false;
            eliminaCompertina.IsVisible = false;

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
                im.Source = ImageSource.FromStream(() => { return new MemoryStream(exp.Galleria[primo]); });
                im.Aspect = Aspect.AspectFill;
                im.Scale = 1.5;
                fotoUno.IsClippedToBounds = true;
                fotoUno.Content = im;
                Image imdue = new Image();
                imdue.Source = ImageSource.FromStream(() => { return new MemoryStream(exp.Galleria[secondo]); });
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
                im.Source = ImageSource.FromStream(() => { return new MemoryStream(exp.Galleria[0]); });
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
            righeLuoghi.Text = exp.Luoghi.Count.ToString() + " elementi";
            righeGalleria.Text = exp.Galleria.Count.ToString() + " elementi";
            righeSlogan.Text = exp.Slogan.Count.ToString() + " elementi";
            righeFun.Text = exp.Funfacts.Count.ToString() + " elementi";
            righePlaylist.Text = exp.Playlist.Count.ToString() + " elementi";
            righeRecensioni.Text = exp.Recensioni.Count.ToString() + " elementi";
            righeAltro.Text = exp.Altro.Count.ToString() + " elementi";
            righeGalleriasec.Text = exp.Galleria.Count.ToString() + " elementi";
            righeAltrosec.Text = exp.Altro.Count.ToString() + " elementi";

        }


        void modificaClicked(Object sender, EventArgs e)
        {
            if (importaCopertina.IsVisible)
            {
                importaCopertina.TranslateTo(0, -40);
                eliminaCompertina.TranslateTo(0, -75);
                importaCopertina.IsVisible = false;
                eliminaCompertina.IsVisible = false;
            }
            else
            {
                importaCopertina.TranslationY = -40;
                importaCopertina.IsVisible = true;
                importaCopertina.TranslateTo(0, 0);
                if (exp.Copertina != null)
                {
                    eliminaCompertina.TranslationY = -75;
                    eliminaCompertina.IsVisible = true;
                    eliminaCompertina.TranslateTo(0, 0);
                }
            }

        }

        async void cambiaCopertina(Object sender, EventArgs e)
        {
            exp.Copertina = null;

            var foto = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Aggiungi foto"
            });


            if (foto != null)
            {
                using (var stream = await foto.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        exp.Copertina = ms.ToArray();
                    }
                }
                await Navigation.PushAsync(new VisualizzaEsperienzaPage(user, exp));
                Navigation.RemovePage(this);
            }

        }

        async void eliminaCopertina(Object sender, EventArgs e)
        {
            exp.Copertina = null;
            await Navigation.PushAsync(new VisualizzaEsperienzaPage(user, exp));
            Navigation.RemovePage(this);
        }

      
        async void RankingClicked(Object sender, EventArgs e)
        {
            DBmanager.AggiornaEsperienza(exp);
            await Navigation.PushAsync(new RankingPage(user));
            Navigation.RemovePage(this);
        }

        async void ProfileClicked(Object sender, EventArgs e)
        {
            DBmanager.AggiornaEsperienza(exp);
            await Navigation.PushAsync(new ProfilePage(user));
            Navigation.RemovePage(this);
        }

        async void BackClicked(Object sender, EventArgs e)
        {
            DBmanager.AggiornaEsperienza(exp);
            await Navigation.PushAsync(new DiaryPage(user));
            Navigation.RemovePage(this);

        }

        async void importa(Object sender, EventArgs e)
        {

            var foto = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Aggiungi foto"
            });

            if (foto != null)
            {
                using (Stream stream = await foto.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        exp.Galleria.Add(ms.ToArray());
                    }
                }
            }

            inizializzaGalleria();
            scroll.Children.Clear();
            inizializzaScorrimentoFoto();
        }


        void partecipantiClicked(Object sender, EventArgs e)
        {
            EmptyList.IsVisible = false;
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Partecipanti";
            apriPartecipanti();
        }

        void apriPartecipanti()
        {
            int x = 2;
            foreach (Utente u in exp.ListaPartecipanti)
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
                f.BackgroundColor = user.Primario;
                f.HasShadow = false;
                f.IsClippedToBounds = true;
                f.BorderColor = Color.Black;
                Label l = new Label();
                l.Text = "@" + u.Username;
                l.TextColor = Color.Black;
                l.HorizontalOptions = LayoutOptions.StartAndExpand;
                l.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                l.Margin = new Thickness(13);
                Button b = new Button();
                b.Text = "Rimuovi";
                b.TextColor = Color.Black;
                b.BackgroundColor = this.user.Secondario;
                b.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                b.Margin = new Thickness(100, 10);
                b.TranslationX = 30;
                b.Clicked += async (sender, args) =>
                {
                    if (exp.ListaPartecipanti.Count > 2)
                    {
                        layout.Children.Clear();
                        layout.Children.Add(nomeschermata);
                        layout.Children.Add(Esci);
                        layout.Children.Add(EmptyList);
                        layout.Children.Add(Aggiungi);
                        layout.Children.Add(AggiungiElemento);
                        Aggiungi.IsVisible = true;
                        exp.ListaPartecipanti.Remove(u);
                        u.Esperienze.Remove(exp);
                        apriPartecipanti();
                    }
                    else
                    {
                        await DisplayAlert("Attenzione", "Un'esperienza non può avere meno di due partecipanti!", "OK");

                    }
                };
                layout.Children.Add(f);
                layout.Children.Add(l);
                layout.Children.Add(b);
                Grid.SetColumn(f, 0);
                Grid.SetColumn(l, 1);
                Grid.SetRow(f, x);
                Grid.SetRow(l, x);
                Grid.SetRow(b, x);
                Grid.SetColumn(b, 1);
                RowDefinition riga = new RowDefinition();
                riga.Height = 60;
                layout.RowDefinitions.Add(riga);
                x++;


            }
            Grid.SetRow(AggiungiElemento, x);
            AggiungiElemento.IsVisible = false;
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
                layout.Children.Add(AggiungiElemento);
                Visualizza.IsVisible = false;
            }
            costruisciGriglia();

        }

        void AggiungiInLista(Object sender, EventArgs e)
        {
            AggiungiElemento.IsVisible = true;
            Aggiungi.IsVisible = false;
        }

        private async void MettiInLista(Object sender, EventArgs e)
        {
            Aggiungi.IsVisible = true;
            if (nomeschermata.Text.Equals("Luoghi"))
            {
                exp.Luoghi.Add(AggiungiElemento.Text);
                apriListaSemplice(exp.Luoghi);
            }
            else if (nomeschermata.Text.Equals("Slogan"))
            {
                exp.Slogan.Add(AggiungiElemento.Text);
                apriListaSemplice(exp.Slogan);
            }
            else if (nomeschermata.Text.Equals("Fun Facts"))
            {

                exp.Funfacts.Add(AggiungiElemento.Text);
                apriListaSemplice(exp.Funfacts);
            }
            else if (nomeschermata.Text.Equals("Playlist"))
            {
                exp.Playlist.Add(AggiungiElemento.Text);
                apriListaSemplice(exp.Playlist);
            }
            else if (nomeschermata.Text.Equals("Recensioni"))
            {

                exp.Recensioni.Add(AggiungiElemento.Text);
                apriListaSemplice(exp.Recensioni);
            }
            else if (nomeschermata.Text.Equals("Altro"))
            {

                exp.Altro.Add(AggiungiElemento.Text);
                apriListaSemplice(exp.Altro);
            }
            else if (nomeschermata.Text.Equals("Partecipanti"))
            {
                if (!AggiungiElemento.Text.Equals(""))
                {
                    foreach (Utente friend in user.Amici.Keys)
                    {
                        if (friend.Username.Equals(AggiungiElemento.Text))
                        {
                            exp.ListaPartecipanti.Add(friend);
                            var x = friend.ListaDistintivi[exp.Tipologia].Item1;
                            x++;
                            var y = friend.ListaDistintivi[exp.Tipologia].Item2;
                            friend.ListaDistintivi[exp.Tipologia] = (x, y);
                            friend.PuntiEsperienza++;
                            exp.ListaPartecipanti.Add(friend);
                            foreach (Utente mem in exp.ListaPartecipanti)
                            {
                                mem.PuntiSocial++;
                                mem.Livello = (mem.PuntiEsperienza + mem.PuntiSocial) / 10 + 1;
                            }
                            DBmanager.AggiornaUtentiInfoExp(exp.ListaPartecipanti, exp.Tipologia);
                            DBmanager.AggiornaAmicizie(exp.ListaPartecipanti, friend);
                            AggiungiElemento.Text = null;
                            apriPartecipanti();
                            return;
                        }
                    }

                    Utente DBfriend = await DBmanager.GetUtenteBasePerNome(AggiungiElemento.Text);
                    if (DBfriend != null)
                    {
                        var x = DBfriend.ListaDistintivi[exp.Tipologia].Item1;
                        x++;
                        var y = DBfriend.ListaDistintivi[exp.Tipologia].Item2;
                        DBfriend.ListaDistintivi[exp.Tipologia] = (x, y);
                        DBfriend.PuntiEsperienza++;
                        exp.ListaPartecipanti.Add(DBfriend);
                        foreach (Utente mem in exp.ListaPartecipanti)
                        {
                            mem.PuntiSocial++;
                            mem.Livello = (mem.PuntiEsperienza + mem.PuntiSocial) / 10 + 1;
                        }
                        DBmanager.AggiornaUtentiInfoExp(exp.ListaPartecipanti, exp.Tipologia);
                        DBmanager.AggiornaAmicizie(exp.ListaPartecipanti, DBfriend);
                        apriPartecipanti();
                    }
                    else { await DisplayAlert("Utente inesistente", "Non esiste alcun utente con il nome dato", "OK"); }
                }
            }
            AggiungiElemento.Text = null;

        }


        void apriListaSemplice(List<string> list)
        {
            layout.Children.Clear();
            layout.Children.Add(nomeschermata);
            layout.Children.Add(Esci);
            layout.Children.Add(EmptyList);
            layout.Children.Add(Aggiungi);
            layout.Children.Add(AggiungiElemento);
            if (list.Count == 0)
            {
                EmptyList.IsVisible = true;
                Grid.SetRow(AggiungiElemento, 2);
                AggiungiElemento.IsVisible = false;


            }
            else
            {
                EmptyList.IsVisible = false;
                int x = 2;
                foreach (string s in list)
                {
                    ScrollView scr = new ScrollView();
                    Label l = new Label();
                    l.Text = (x - 1).ToString() + ". " + s;
                    l.TextColor = Color.Black;
                    l.WidthRequest = Visualizza.Width;
                    l.Margin = new Thickness(13);
                    l.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));

                    scr.Content = l;
                    layout.Children.Add(scr);
                    Button b = new Button();
                    b.Text = "Rimuovi";
                    b.TextColor = Color.Black;
                    b.Margin = new Thickness(0, 10);
                    b.CornerRadius = 10;
                    b.BackgroundColor = this.user.Secondario;
                    b.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    b.Clicked += (sender, args) =>
                    {
                        layout.Children.Clear();
                        layout.Children.Add(nomeschermata);
                        layout.Children.Add(Esci);
                        layout.Children.Add(EmptyList);
                        layout.Children.Add(Aggiungi);
                        layout.Children.Add(AggiungiElemento);
                        Aggiungi.IsVisible = true;
                        if (nomeschermata.Text.Equals("Luoghi"))
                        {
                            exp.Luoghi.Remove(s);
                            apriListaSemplice(exp.Luoghi);
                        }
                        else if (nomeschermata.Text.Equals("Slogan"))
                        {
                            exp.Slogan.Remove(s);
                            apriListaSemplice(exp.Slogan);
                        }
                        else if (nomeschermata.Text.Equals("Fun Facts"))
                        {
                            exp.Funfacts.Remove(s);
                            apriListaSemplice(exp.Funfacts);
                        }
                        else if (nomeschermata.Text.Equals("Playlist"))
                        {
                            exp.Playlist.Remove(s);
                            apriListaSemplice(exp.Playlist);
                        }
                        else if (nomeschermata.Text.Equals("Recensioni"))
                        {
                            exp.Recensioni.Remove(s);
                            apriListaSemplice(exp.Recensioni);
                        }
                        else if (nomeschermata.Text.Equals("Altro"))
                        {
                            exp.Altro.Remove(s);
                            apriListaSemplice(exp.Altro);
                        }

                    };
                    layout.Children.Add(b);
                    Grid.SetColumn(scr, 0);
                    Grid.SetRow(scr, x);
                    Grid.SetColumnSpan(scr, 2);
                    Grid.SetRow(b, x);
                    Grid.SetColumn(b, 2);
                    RowDefinition riga = new RowDefinition();
                    if (Device.RuntimePlatform == Device.iOS)
                        riga.Height = 50;
                    else
                        riga.Height = 65;
                    layout.RowDefinitions.Add(riga);
                    x++;
                }
                Grid.SetRow(AggiungiElemento, x);
                AggiungiElemento.IsVisible = false;


            }
        }

        void LuoghiClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Luoghi";
            apriListaSemplice(exp.Luoghi);
        }

        void SloganClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Slogan";
            apriListaSemplice(exp.Slogan);
        }

        void FunClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Fun Facts";
            apriListaSemplice(exp.Funfacts);
        }

        void PlaylistClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Playlist";
            apriListaSemplice(exp.Playlist);
        }

        void RecensioniClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Recensioni";
            apriListaSemplice(exp.Recensioni);
        }

        void AltroClicked(Object sender, EventArgs e)
        {
            Visualizza.Scale = 0;
            Visualizza.IsVisible = true;
            Visualizza.ScaleTo(1);
            nomeschermata.Text = "Altro";
            apriListaSemplice(exp.Altro);
        }

        async void GalleriaClicked(Object sender, EventArgs e)
        {
            Galleria.Scale = 0;
            Galleria.IsVisible = true;
            await Galleria.ScaleTo(1);

        }


        void inizializzaScorrimentoFoto()
        {
            if (exp.Galleria.Count == 0) EliminaDaGalleria.IsVisible = false;
            else EliminaDaGalleria.IsVisible = true;

            foreach (byte[] ba in exp.Galleria)
            {
                Frame f = new Frame();
                if (Device.RuntimePlatform == Device.iOS)
                    f.WidthRequest = 340;
                else
                    f.WidthRequest = 370;
                f.BackgroundColor = Color.Black;
                Image im = new Image();
                im.Source = ImageSource.FromStream(() => { return new MemoryStream(ba); });
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
                foreach (byte[] ba in exp.Galleria)
                {

                    Button b = new Button();
                    b.BackgroundColor = Color.Transparent;
                    b.BorderWidth = 1.5;
                    b.BorderColor = Color.Black;
                    b.Clicked += (sender, args) =>
                    {
                        foto.IsVisible = true;
                        scrolling.ScrollToAsync(exp.Galleria.IndexOf(ba) * scroll.Children[0].Width, 0, false);
                    };
                    Frame f = new Frame();
                    f.BackgroundColor = Color.Black;
                    f.BorderColor = Color.Black;
                    Image im = new Image();
                    im.Source = ImageSource.FromStream(() => { return new MemoryStream(ba); });
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

        void EliminaFoto(Object sender, EventArgs e)
        {
            int indice = (int)(scrolling.ScrollX / scroll.Children[0].Width);
            exp.Galleria.RemoveAt(indice);
            scroll.Children.Clear();
            inizializzaScorrimentoFoto();
            layoutGalleria.Children.Clear();
            layoutGalleria.Children.Add(TitoloGalleria);
            layoutGalleria.Children.Add(EsciGalleria);
            layoutGalleria.Children.Add(Empty);
            layoutGalleria.Children.Add(AggiungiFoto);
            inizializzaGalleria();

        }

        async void eliminaEsperienza(Object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Attenzione", "Eliminando questa esperienza la cancellerai dal diario di tutti i partecipanti", "CONFERMA", "ANNULLA");
            if (answer)
            {
                this.exp.Elimina();
                DBmanager.EliminaEsperienza(exp.ID);
                await Navigation.PushAsync(new ProfilePage(user));
                Navigation.RemovePage(this);
            }
        }



    }
}
