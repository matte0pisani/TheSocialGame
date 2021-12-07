using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TheSocialGame
{
    public partial class AddExperiencePage : ContentPage
    {
        Utente user;
        Page prossima; //prossima pagina da aprire
        Esperienza nuova;
        public AddExperiencePage(Utente us)
        {
            InitializeComponent();
            user = us;
            nuova = new Esperienza();

            nuova.ListaPartecipanti.Add(user);
            EsciSenzaSalvareFrame.IsVisible = false;
            CopertinaFrame.IsVisible = false;
            Rimuovi.IsVisible = false;
            LabelFromTime.IsVisible = false;
            FromTime.IsVisible = false;
            LabelToTime.IsVisible = false;
            ToTime.IsVisible = false;
            WarningPartecipanti.IsVisible = false;
            WarningTipologia.IsVisible = false;
            WarningTitolo.IsVisible = false;
            TipiEsp.ItemsSource = new List<string>(user.listaDistintivi.Keys);
        }

        /* metodi di apertura pagine connesse */
        void NotificationClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new NotificationPage();

        }

        void HomeClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new HomePage();
        }

        void ProfileClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new ProfilePage(user);
        }

         void SearchClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new SearchPage();
        }

        void RankingClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new RankingPage();
        }

         void BackClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new ProfilePage(user);
        }

        void AnnullaClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = false;
        }

        async void ConfermaClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = false;
            await Navigation.PopAsync();
        }

        void TitoloCopertina(Object sender, EventArgs e)
        {
            nuova.Titolo = Titolo.Text;
        }


        /* gestione foto profilo da fotocamera*/
        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void CameraClicked(Object sender, EventArgs e)
        {
          

            var foto = await MediaPicker.CapturePhotoAsync();

            if (foto != null)
            {
                
                var newFile = Path.Combine(FileSystem.CacheDirectory, foto.FileName);
                using (var stream = await foto.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);
                
                nuova.Copertina = newFile;
                CopertinaFrame.IsVisible = true;
                AvvisoCopertina.IsVisible = false;
                Rimuovi.IsVisible = true;
                Fotocamera.IsVisible = false;
                Galleria.IsVisible = false;
                Copertina.Source = newFile;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        nuova.copertinaLiveIOS = true;
                        Copertina.Rotation = 90;
                   break;
                }
            }
        }

        /* gestione foto profilo da galleria*/
        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void FromGalleryClicked(Object sender, EventArgs e)
        {
           
            var foto = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Scegli la tua immagine del profilo!"
            });

            if (foto != null)
            {                

                nuova.Copertina = foto.FullPath;
                CopertinaFrame.IsVisible = true;
                AvvisoCopertina.IsVisible = false;
                Rimuovi.IsVisible = true;
                Fotocamera.IsVisible = false;
                Galleria.IsVisible = false;
                Copertina.Source = foto.FullPath;
                
            }
        }

         void RimuoviClicked(Object sender, EventArgs e)
        {
            nuova.Copertina = null;
            CopertinaFrame.IsVisible = false;
            AvvisoCopertina.IsVisible = true;
            Rimuovi.IsVisible = false;
            Fotocamera.IsVisible = true;
            Galleria.IsVisible = true;
        }

        void DataInizioSelected(Object sender, EventArgs e)
        {
            nuova.DataInizio = DataInizio.Date;
            DataFine.MinimumDate = DataInizio.Date;
        }

        void DataFineSelected(Object sender, EventArgs e)
        {
            nuova.DataFine = DataFine.Date;
            DataInizio.MaximumDate = DataFine.Date;
        }

        //Capire come gestire esperienze in diretta
        void LiveChanged(Object sender, EventArgs e)
        {
            if (Live.IsToggled)
            {
                LabelFromTime.IsVisible = true;
                FromTime.IsVisible = true;
                LabelToTime.IsVisible = true;
                ToTime.IsVisible = true;
                nuova.live = true;
            } else
            {
                LabelFromTime.IsVisible = false;
                FromTime.IsVisible = false;
                LabelToTime.IsVisible = false;
                ToTime.IsVisible = false;
                nuova.live = false;
            }
        }

        void TipoEsperienza(Object sender, EventArgs e)
        {
            nuova.Tipologia = (string) TipiEsp.SelectedItem;
        }


        //da rifare cercando utenti in database
        void AggiungiPartecipanti(Object sender, EventArgs e)
        {
            Utente partecipante = new Utente();
            partecipante.username = Partecipanti.Text;
            if (!Partecipanti.Text.Equals(""))
            {
                nuova.ListaPartecipanti.Add(partecipante);
                ListaPartecipanti.Text = ListaPartecipanti.Text + "@" + partecipante.username + "  ";
            }
            Partecipanti.Text=null;
        }

        void PrivataChanged(Object sender, EventArgs e)
        {
            if (Privata.IsToggled)
                nuova.privata = true;
            else nuova.privata = false;
        }


        //DA CAPIRE come gestire amicizie
       async void Salva(Object sender, EventArgs e)
        {
            if (nuova.DataInizio.Equals(new DateTime())) nuova.DataInizio = DataInizio.Date;
            if (nuova.DataFine.Equals(new DateTime())) nuova.DataFine = DataFine.Date;

            if (nuova.Titolo == null ||  nuova.Tipologia == null || nuova.ListaPartecipanti.Count == 0)
            {
                SavingLabel.IsVisible = false;
                Warning.IsVisible = true;
                if (nuova.Titolo == null) WarningTitolo.IsVisible = true;
                if (nuova.Tipologia == null) WarningTipologia.IsVisible = true;
                if (nuova.ListaPartecipanti.Count == 1) WarningPartecipanti.IsVisible = true;
                
            }
            else
            {
                foreach (Utente u in nuova.ListaPartecipanti)
                {
                    u.esperienze.Add(nuova);
                    int x = u.listaDistintivi[nuova.Tipologia].Item1;
                    Dictionary<int, bool> diz = u.listaDistintivi[nuova.Tipologia].Item2;
                    x++;
                    u.listaDistintivi[nuova.Tipologia] = (x, diz);
                }
                await Navigation.PushAsync(new DiaryPage(user));
                Navigation.RemovePage(this);
            }
        }

    }
}
