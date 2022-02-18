using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TheSocialGame
{
    public partial class ProfilePage : ContentPage
    {
        private string usrID;
        public Utente user;

        public ProfilePage(string id)
        {
            InitializeComponent();
            usrID = id;
            user = null;

            Scrolling.HeightRequest = 650;
            ConfermaEliminazioneFrame.IsVisible = false;
          
            AddPhotoFrame.IsVisible = false;
            BindingContext = this;
        }

        public ProfilePage(Utente usr)
        {
            InitializeComponent();
            user = usr;
            usrID = usr.ID;

            Scrolling.HeightRequest = 650;
            ConfermaEliminazioneFrame.IsVisible = false;
           
            AddPhotoFrame.IsVisible = false;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            sfondoCaricamento.IsVisible = true;
            caricamento.IsRunning = true;
            base.OnAppearing();
            if (user == null) { user = await DBmanager.GetUtente(usrID); }

            App.Current.Resources["BackgroundColor"] = user.Sfondo;
            App.Current.Resources["FirstColor"] = user.Primario;
            App.Current.Resources["SecondColor"] = user.Secondario;

            //       creaUtenteFake();

            if (user.FotoBytes == null)
            {
                ProfilePicFrame.IsVisible = false;
                ChangeProfilePicButton.IsVisible = false;

            }
            else
            {
                if (user.FotoLiveiOS) ProfilePic.Rotation = 90;
                ProfilePic.Source = ImageSource.FromStream(() =>  new MemoryStream(user.FotoBytes));    // modificato; ma probabilmente uguale a prima
                ChangeProfilePicButton.IsVisible = true;
            }

            UsernameLabel.Text = user.Username;
            AbilitaDistintivi();
            MostraAmici();
            RiempiProgressBar();
            if (user.Livello < 10)
                LabelLevelSingle.Text = Convert.ToString(user.Livello);
            else LabelLevelDouble.Text = Convert.ToString(user.Livello);
            sfondoCaricamento.IsVisible = false;
            caricamento.IsRunning = false;
        }


        /* Mostra i tre "migliori amici", ordinati per numero di esperienze condivise, inserendo il loro username come testo degli appositi label*/
        void MostraAmici()
        {
            Dictionary<Utente, int> best = user.GetBestFriends();

            int i = 1;
            foreach (KeyValuePair<Utente, int> coppia in best)
            {
                if (i == 1)
                {
                    BF1.Text = "@" + coppia.Key.Username;
                    EspBF1.Text = coppia.Value.ToString() + " esperienze insieme";
                    i++;
                }
                else if (i == 2)
                {
                    BF2.Text = "@" + coppia.Key.Username;
                    EspBF2.Text = coppia.Value.ToString() + " esperienze insieme";
                    i++;
                }
                else if (i == 3)
                {
                    BF3.Text = "@" + coppia.Key.Username;
                    EspBF3.Text = coppia.Value.ToString() + " esperienze insieme";
                    i++;
                }
            }

        }

        /*riempimento barre fino a soglia indicata */
        async void RiempiProgressBar()
        {
            await SocialPointBar.ProgressTo((double)(user.PuntiSocial % 10) / 10, 3000, Easing.Linear);
        }

       

        /* abilita i distintivi ottenuti */
        void AbilitaDistintivi()
        {
            int guadagnati = 0;


            ViaggioMare1.IsVisible = !user.ListaDistintivi["ViaggioMare"].Item2[1];
            Ristorante1.IsVisible = !user.ListaDistintivi["Ristorante"].Item2[1];
            Sport1.IsVisible = !user.ListaDistintivi["Sport"].Item2[1];
            Discoteca1.IsVisible = !user.ListaDistintivi["Discoteca"].Item2[1];
            Compleanno1.IsVisible = !user.ListaDistintivi["Compleanno"].Item2[1];
            Maschera1.IsVisible = !user.ListaDistintivi["Maschera"].Item2[1];
            ViaggioMontagna1.IsVisible = !user.ListaDistintivi["ViaggioMontagna"].Item2[1];
            ViaggioCitta1.IsVisible = !user.ListaDistintivi["ViaggioCitta"].Item2[1];
            Cultura1.IsVisible = !user.ListaDistintivi["Cultura"].Item2[1];
            Cocktail1.IsVisible = !user.ListaDistintivi["Cocktail"].Item2[1];
            Casa1.IsVisible = !user.ListaDistintivi["Casa"].Item2[1];
            ViaggioMare2.IsVisible = !user.ListaDistintivi["ViaggioMare"].Item2[2];
            Ristorante2.IsVisible = !user.ListaDistintivi["Ristorante"].Item2[2];
            Sport2.IsVisible = !user.ListaDistintivi["Sport"].Item2[2];
            Disco2.IsVisible = !user.ListaDistintivi["Discoteca"].Item2[2];
            Compleanno2.IsVisible = !user.ListaDistintivi["Compleanno"].Item2[2];
            Maschera2.IsVisible = !user.ListaDistintivi["Maschera"].Item2[2];
            Montagna2.IsVisible = !user.ListaDistintivi["ViaggioMontagna"].Item2[2];
            Citta2.IsVisible = !user.ListaDistintivi["ViaggioCitta"].Item2[2];
            Cultura2.IsVisible = !user.ListaDistintivi["Cultura"].Item2[2];
            Cocktail2.IsVisible = !user.ListaDistintivi["Cocktail"].Item2[2];
            Casa2.IsVisible = !user.ListaDistintivi["Casa"].Item2[2];


            Dictionary<string, (int, Dictionary<int, bool>)>.ValueCollection valori = user.ListaDistintivi.Values;
            foreach ((int, Dictionary<int, bool>) elem in valori)
            {
                Dictionary<int, bool> val = elem.Item2;
                foreach (KeyValuePair<int, bool> coppia in val)
                {
                    if (coppia.Value)
                        guadagnati++;
                }
            }


            DistintiviGuadagnati.Text = Convert.ToString(guadagnati) + "/\n22";
            DescriptionFrame.IsVisible = false;


        }

        /* gestione foto profilo da fotocamera*/
        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void CameraClicked(Object sender, EventArgs e)
        {
            var foto = await MediaPicker.CapturePhotoAsync();

            if (foto != null)
            {
                using (var stream = await foto.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        user.FotoBytes = ms.ToArray();
                    }
                }

                ProfilePicFrame.IsVisible = true;
                ChangeProfilePicButton.IsVisible = true;
                ProfilePic.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(user.FotoBytes);
                });

                        user.FotoLiveiOS = true;
                        ProfilePic.Rotation = 90;
                        
                
            }

            DBmanager.AggiornaUtenteInfoNonExp(user);
        }

        /* gestione foto profilo da galleria*/
        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void FromGalleryClicked(Object sender, EventArgs e)
        {
            ProfilePicFrame.Rotation = 0;
            var foto = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Scegli la tua immagine del profilo!"
            });

            if (foto != null)
            {
                ProfilePic.Rotation = 0;
                user.FotoLiveiOS = false;
                using (var stream = await foto.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        user.FotoBytes = ms.ToArray();
                    }
                }
                DBmanager.AggiornaUtenteInfoNonExp(user);
                ProfilePicFrame.IsVisible = true;
                ChangeProfilePicButton.IsVisible = true;
                ProfilePic.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(user.FotoBytes);
                });
            }
        }

        /* eliminazione foto profilo corrente */
        void ConfermaElimina(Object sender, EventArgs e)
        {
            ConfermaEliminazioneFrame.IsVisible = true;
        }

        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        void EliminaFoto(Object sender, EventArgs e)
        {
            // non so se l'immagine rimane "pendente" nella memoria del processo, in tal caso andrebbe liberata la memoria
            user.FotoBytes = null;
            DBmanager.AggiornaUtenteInfoNonExp(user);
            ProfilePicFrame.IsVisible = false;
            ChangeProfilePicButton.IsVisible = false;
            ConfermaEliminazioneFrame.IsVisible = false;
        }

        void AnnullaElimina(Object sender, EventArgs e)
        {
            ConfermaEliminazioneFrame.IsVisible = false;
        }

      

        /* gestione foto profilo*/
        void AddPhoto(Object sender, EventArgs e)
        {
            AddPhotoFrame.IsVisible = true;
            if (user.FotoBytes != null)
                EliminaButton.IsVisible = true;
            else
                EliminaButton.IsVisible = false;
        }


        void ExitFromAddPhoto(Object sender, EventArgs e)
        {

            AddPhotoFrame.IsVisible = false;

        }

        /* metodi di apertura pagine connesse */
      
        async void AddClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddExperiencePage(user));
            Navigation.RemovePage(this);

        }

       

        async void RankingClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RankingPage(user));

        }

        async void SettingClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingPage(user));

        }

        async void DiaryClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DiaryPage(user));

        }

        async void FriendsClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FriendsPage(user));

        }


        async void Mare1Clicked(System.Object sender, System.EventArgs e)
        {
            TranslationX = 0;
            TitoloDistintivo.Text = "Tipo da Spiaggia";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella\ncategoria Viaggio al Mare";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Ristorante1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 60;
            TitoloDistintivo.Text = "Assaggiatore";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Ristorante";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Sport1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 120;
            TitoloDistintivo.Text = "Sportivo occasionale";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Sport";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Disco1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 180;
            TitoloDistintivo.Text = "Passo Base";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Discoteca";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Compleanno1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 240;
            TitoloDistintivo.Text = "Mangiatore di Torte";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Compleanno";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Montagna1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 300;
            TitoloDistintivo.Text = "Amanate dell'Aria Pulita";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Viaggio in Montagna";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Citta1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 360;
            TitoloDistintivo.Text = "Esploratore";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Viaggio in Città";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Cultura1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 420;
            TitoloDistintivo.Text = "Studioso";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Visita Culturale";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Cocktail1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 480;
            TitoloDistintivo.Text = "Sommelier";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Drink";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Maschera1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 540;
            TitoloDistintivo.Text = "Incognito";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Travestimenti";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Casa1Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 600;
            TitoloDistintivo.Text = "Re del Divano";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Casa";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Mare2Clicked(System.Object sender, System.EventArgs e)
        {
            TranslationX = 660;
            TitoloDistintivo.Text = "Baywatch";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella\ncategoria Viaggio al Mare";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Ristorante2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 720;
            TitoloDistintivo.Text = "Palato Raffinato";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Ristorante";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Sport2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 780;
            TitoloDistintivo.Text = "Gioco di Squadra";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Sport";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Disco2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 840;
            TitoloDistintivo.Text = "Re della pista";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Discoteca";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Compleanno2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 900;
            TitoloDistintivo.Text = "Lanciatore di Coriandoli";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Compleanno";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Montagna2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 960;
            TitoloDistintivo.Text = "Avventuriero";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Viaggio in Montagna";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Citta2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 690;
            TitoloDistintivo.Text = "Viaggiatore";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Viaggio in Città";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Cultura2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 750;
            TitoloDistintivo.Text = "Professore";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Visita Culturale";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Cocktail2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 810;
            TitoloDistintivo.Text = "Compagno di Bevute";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Drink";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Maschera2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 870;
            TitoloDistintivo.Text = "L'Irriconoscibile";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Travestimenti";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Casa2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 930;
            TitoloDistintivo.Text = "Pigiama Molesto";
            DescrizioneDistintivo.Text = "Hai completato almeno 7 esperienze nella \n categoria Casa";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }



        private void creaUtenteFake()
        {
            user.ListaDistintivi["ViaggioMare"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Ristorante"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Sport"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Compleanno"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Maschera"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["ViaggioMontagna"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["ViaggioCitta"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Cultura"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Cocktail"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Casa"].Item2[1] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["ViaggioMare"].Item2[2] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Ristorante"].Item2[2] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Sport"].Item2[2] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Compleanno"].Item2[2] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Maschera"].Item2[2] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["ViaggioMontagna"].Item2[2] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Cultura"].Item2[2] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Cocktail"].Item2[2] = new Random().Next(0, 2) > 0;
            user.ListaDistintivi["Casa"].Item2[2] = new Random().Next(0, 2) > 0;
        }


    }
}
