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
        Utente user;

        public ProfilePage(Utente us)
        {
            InitializeComponent();
           
            user = us;
            App.Current.Resources["BackgroundColor"] = user.sfondo;
            App.Current.Resources["FirstColor"] = user.primario;
            App.Current.Resources["SecondColor"] = user.secondario;


            creaUtenteFake();

            if (user.pathFotoProfilo == null)
            {
                ProfilePicFrame.IsVisible = false;
                ChangeProfilePicButton.IsVisible = false;
                
            } else
            {
                if (user.fotoLiveiOS) ProfilePic.Rotation = 90;
                ProfilePic.Source = ImageSource.FromFile(user.pathFotoProfilo);
                ChangeProfilePicButton.IsVisible = true;
               
            }
            UsernameLabel.Text =this.user.username;
            AbilitaDistintivi();
            MostraAmici();
            RiempiProgressBar();
            if (user.livello < 10)
                 LabelLevelSingle.Text = Convert.ToString(user.livello);
            else LabelLevelDouble.Text = Convert.ToString(user.livello);

            Scrolling.HeightRequest = 650;
            ConfermaEliminazioneFrame.IsVisible = false;
            MenuPersonalita.IsVisible = false;
            TriangoloChiudi.IsVisible = false;
            AddPhotoFrame.IsVisible = false;
            BindingContext = this;
            
        }


        /* Mostra i tre "migliori amici", ordinati per numero di esperienze condivise, inserendo il loro username come testo degli appositi label*/
        void MostraAmici()
        {
            BF1.Text ="@"+user.BestFriend1.username;
            BF2.Text ="@" + user.BestFriend2.username;
            BF3.Text ="@" + user.BestFriend3.username;

            //provvisorio, bisognerà accedere ad esperienze in comune
            int x, y, z;
            x = new Random().Next(1000);
            y = new Random().Next(1000);
            z = new Random().Next(1000);

            if( x > y && x > z)
            {
                EspBF1.Text = Convert.ToString(x) + " esperienze insieme";
                if(y> z)
                {
                    EspBF2.Text = Convert.ToString(y) + " esperienze insieme";
                    EspBF3.Text = Convert.ToString(z) + " esperienze insieme";
                } else
                {
                    EspBF2.Text = Convert.ToString(z) + " esperienze insieme";
                    EspBF3.Text = Convert.ToString(y) + " esperienze insieme";
                }
            } else if(y>x && y> z)
            {
                EspBF1.Text = Convert.ToString(y) + " esperienze insieme";
                if (x > z)
                {
                    EspBF2.Text = Convert.ToString(x) + " esperienze insieme";
                    EspBF3.Text = Convert.ToString(z) + " esperienze insieme";
                }
                else
                {
                    EspBF2.Text = Convert.ToString(z) + " esperienze insieme";
                    EspBF3.Text = Convert.ToString(x) + " esperienze insieme";
                }
            }
            else
            {
                EspBF1.Text = Convert.ToString(z) + " esperienze insieme";
                if (x > y)
                {
                    EspBF2.Text = Convert.ToString(x) + " esperienze insieme";
                    EspBF3.Text = Convert.ToString(y) + " esperienze insieme";
                }
                else
                {
                    EspBF2.Text = Convert.ToString(z) + " esperienze insieme";
                    EspBF3.Text = Convert.ToString(y) + " esperienze insieme";
                }
            }

        }
        
        /*riempimento barre fino a soglia indicata */
        async void RiempiProgressBar() {
            await SocialPointBar.ProgressTo((double)(user.puntiSocial % 10) / 10, 3000, Easing.Linear);
        }

        void RiempiPersonalityBar()
        {
            int puntiTotali = user.personalita1 + user.personalita2 + user.personalita3 + user.personalita4 + user.personalita5 + user.personalita6 + user.personalita7 + user.personalita8 + user.personalita9 + user.personalita10;
            
                Tipo1.ProgressTo((double)user.personalita1 / puntiTotali, 2000, Easing.Linear);
                Tipo2.ProgressTo((double)user.personalita2 / puntiTotali, 2000, Easing.Linear);
                 Tipo3.ProgressTo((double)user.personalita3 / puntiTotali, 2000, Easing.Linear);
                 Tipo4.ProgressTo((double)user.personalita4 / puntiTotali, 2000, Easing.Linear);
                Tipo5.ProgressTo((double)user.personalita5 / puntiTotali, 2000, Easing.Linear);
                 Tipo6.ProgressTo((double)user.personalita6 / puntiTotali, 2000, Easing.Linear);
                Tipo7.ProgressTo((double)user.personalita6 / puntiTotali, 2000, Easing.Linear);
                 Tipo8.ProgressTo((double)user.personalita8 / puntiTotali, 2000, Easing.Linear);
                Tipo9.ProgressTo((double)user.personalita9 / puntiTotali, 2000, Easing.Linear);
                Tipo10.ProgressTo((double)user.personalita10 / puntiTotali, 2000, Easing.Linear);

            PercentualeTipo1.Text = Convert.ToString(user.personalita1 * 100 / puntiTotali) + "%";
            PercentualeTipo2.Text = Convert.ToString(user.personalita2 * 100 / puntiTotali) + "%";
            PercentualeTipo3.Text = Convert.ToString(user.personalita3 * 100 / puntiTotali) + "%";
            PercentualeTipo4.Text = Convert.ToString(user.personalita4 * 100 / puntiTotali) + "%";
            PercentualeTipo5.Text = Convert.ToString(user.personalita5 * 100 / puntiTotali) + "%";
            PercentualeTipo6.Text = Convert.ToString(user.personalita1 * 100 / puntiTotali) + "%";
            PercentualeTipo7.Text = Convert.ToString(user.personalita7 * 100 / puntiTotali) + "%";
            PercentualeTipo8.Text = Convert.ToString(user.personalita8 * 100 / puntiTotali) + "%";
            PercentualeTipo9.Text = Convert.ToString(user.personalita9 * 100 / puntiTotali) + "%";
            PercentualeTipo10.Text = Convert.ToString(user.personalita10 * 100 / puntiTotali) + "%";
            
        }

        /*inizializza indicatori personalità, necessario per far avvenire animazione ogni volta che viene aperto il menu */
        void SvuotaPersonalityBar()
        {
            Tipo1.Progress = 0;
            PercentualeTipo1.Text="";
            Tipo2.Progress = 0;
            PercentualeTipo2.Text="";
            Tipo3.Progress = 0;
            PercentualeTipo3.Text="";
            Tipo4.Progress = 0;
            PercentualeTipo4.Text="";
            Tipo5.Progress = 0;
            PercentualeTipo5.Text="";
            Tipo6.Progress = 0;
            PercentualeTipo6.Text="";
            Tipo7.Progress = 0;
            PercentualeTipo7.Text="";
            Tipo8.Progress = 0;
            PercentualeTipo8.Text="";
            Tipo9.Progress = 0;
            PercentualeTipo9.Text="";
            Tipo10.Progress = 0;
            PercentualeTipo10.Text="";
        }

        /* abilita i distintivi ottenuti */
        void AbilitaDistintivi()
        {
            int guadagnati = 0;


            ViaggioMare1.IsVisible = !user.listaDistintivi["ViaggioMare"].Item2[1];
            Ristorante1.IsVisible = !user.listaDistintivi["Ristorante"].Item2[1];
            Sport1.IsVisible = !user.listaDistintivi["Sport"].Item2[1];
            Discoteca1.IsVisible = !user.listaDistintivi["Discoteca"].Item2[1];
            Compleanno1.IsVisible = !user.listaDistintivi["Compleanno"].Item2[1];
            Maschera1.IsVisible = !user.listaDistintivi["Maschera"].Item2[1];
            ViaggioMontagna1.IsVisible = !user.listaDistintivi["ViaggioMontagna"].Item2[1];
            ViaggioCitta1.IsVisible = !user.listaDistintivi["ViaggioCitta"].Item2[1];
            Cultura1.IsVisible = !user.listaDistintivi["Cultura"].Item2[1];
            Cocktail1.IsVisible = !user.listaDistintivi["Cocktail"].Item2[1];
            Casa1.IsVisible = !user.listaDistintivi["Casa"].Item2[1];
            ViaggioMare2.IsVisible = !user.listaDistintivi["ViaggioMare"].Item2[2];
            Ristorante2.IsVisible = !user.listaDistintivi["Ristorante"].Item2[2];
            Sport2.IsVisible = !user.listaDistintivi["Sport"].Item2[2];
            Disco2.IsVisible = !user.listaDistintivi["Discoteca"].Item2[2];
            Compleanno2.IsVisible = !user.listaDistintivi["Compleanno"].Item2[2];
            Maschera2.IsVisible = !user.listaDistintivi["Maschera"].Item2[2];
            Montagna2.IsVisible = !user.listaDistintivi["ViaggioMontagna"].Item2[2];
            Citta2.IsVisible = !user.listaDistintivi["ViaggioCitta"].Item2[2];
            Cultura2.IsVisible = !user.listaDistintivi["Cultura"].Item2[2];
            Cocktail2.IsVisible = !user.listaDistintivi["Cocktail"].Item2[2];
            Casa2.IsVisible = !user.listaDistintivi["Casa"].Item2[2];


            Dictionary<string, (int, Dictionary<int, bool>)>.ValueCollection valori = user.listaDistintivi.Values;
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
                
                var newFile = Path.Combine(FileSystem.CacheDirectory, foto.FileName);
                using (var stream = await foto.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

                

                user.pathFotoProfilo = newFile;
                
                ProfilePicFrame.IsVisible = true;
                ChangeProfilePicButton.IsVisible = true;
                ProfilePic.Source = newFile;
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        user.fotoLiveiOS = true;
                        ProfilePic.Rotation = 90;
                        break;
                }
            }
        }

       
        


        /* gestione foto profilo da galleria*/
        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void FromGalleryClicked(Object sender, EventArgs e)
        {
            ProfilePicFrame.Rotation=0;
            var foto = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Scegli la tua immagine del profilo!"
            });

            if (foto != null)
            {
                ProfilePic.Rotation = 0;
                user.fotoLiveiOS = false;
                user.pathFotoProfilo = foto.FullPath;
                ProfilePicFrame.IsVisible = true;
                ChangeProfilePicButton.IsVisible = true;
                ProfilePic.Source = foto.FullPath;
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
            user.pathFotoProfilo = null;
            ProfilePicFrame.IsVisible = false;
            ChangeProfilePicButton.IsVisible = false;
            ConfermaEliminazioneFrame.IsVisible = false;
        }

        void AnnullaElimina(Object sender, EventArgs e)
        {
            ConfermaEliminazioneFrame.IsVisible = false;
        }

        /* gestione menu personalita */
        void ApriMenuPersonalita(Object sender, EventArgs e)
        {
            TriangoloChiudi.IsVisible = true;
            TriangoloApri.IsVisible = false;
            MenuPersonalita.IsVisible = true;
            BestFriendsFrame.TranslateTo(0, 335);
            Scrolling.HeightRequest = 1000;
            RiempiPersonalityBar();
        }

        void ChiudiMenuPersonalita(Object sender, EventArgs e)
        {
            TriangoloChiudi.IsVisible = false;
            TriangoloApri.IsVisible = true;
            MenuPersonalita.IsVisible = false;
            BestFriendsFrame.TranslateTo(0, 0);
            Scrolling.HeightRequest = 650;
            SvuotaPersonalityBar();
        }

        /* gestione foto profilo*/
        void AddPhoto(Object sender, EventArgs e)
        {
            AddPhotoFrame.IsVisible = true;
            if (user.pathFotoProfilo != null)
                EliminaButton.IsVisible = true;
            else
                EliminaButton.IsVisible = false;
        }


        void ExitFromAddPhoto(Object sender, EventArgs e)
        {

            AddPhotoFrame.IsVisible = false;

        }

        /* metodi di apertura pagine connesse */
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
            await Navigation.PushAsync(new RankingPage());
            Navigation.RemovePage(this);
        }

        async void SettingClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingPage( user));
           
        }

        async void DiaryClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DiaryPage(user));
           
        }

        async void FriendsClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FriendsPage());
            Navigation.RemovePage(this);
        }

        void creaUtenteFake()
        {
            user.puntiSocial = new Random().Next(100);
            user.livello = (user.puntiSocial / 10) + 1;
            user.personalita1 = new Random().Next(100);
            user.personalita2 = new Random().Next(100);
            user.personalita3 = new Random().Next(100);
            user.personalita4 = new Random().Next(100);
            user.personalita5 = new Random().Next(100);
            user.personalita6 = new Random().Next(100);
            user.personalita7 = new Random().Next(100);
            user.personalita8 = new Random().Next(100);
            user.personalita9 = new Random().Next(100);
            user.personalita10 = new Random().Next(100);

            user.BestFriend1 = new Utente();
            user.BestFriend2 = new Utente();
            user.BestFriend3 = new Utente();
            user.BestFriend1.username = "BestFriend1";
            user.BestFriend2.username = "BestFriend2";
            user.BestFriend3.username = "BestFriend3";


           
            user.listaDistintivi["ViaggioMare"].Item2[1] = new Random().Next(0,2) > 0;
            user.listaDistintivi["Ristorante"].Item2[1] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Sport"].Item2[1] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Compleanno"].Item2[1] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Maschera"].Item2[1] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["ViaggioMontagna"].Item2[1] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["ViaggioCitta"].Item2[1] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Cultura"].Item2[1] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Cocktail"].Item2[1] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Casa"].Item2[1] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["ViaggioMare"].Item2[2] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Ristorante"].Item2[2] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Sport"].Item2[2] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Compleanno"].Item2[2] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Maschera"].Item2[2] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["ViaggioMontagna"].Item2[2] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["ViaggioCitta"].Item2[2] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Cultura"].Item2[2] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Cocktail"].Item2[2] = new Random().Next(0, 2) > 0;
            user.listaDistintivi["Casa"].Item2[2] = new Random().Next(0, 2) > 0;



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
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Compleanno";
            DescriptionFrame.IsVisible = true;
            await Task.Delay(2500);
            DescriptionFrame.IsVisible = false;
        }

        async void Montagna2Clicked(System.Object sender, System.EventArgs e)
        {
            DescriptionFrame.TranslationX = 960;
            TitoloDistintivo.Text = "Avventuriero";
            DescrizioneDistintivo.Text = "Hai completato almeno 3 esperienze nella \n categoria Viaggio in Montagna";
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



    }
}
