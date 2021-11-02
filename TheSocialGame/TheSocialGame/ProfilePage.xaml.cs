using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace TheSocialGame
{
    public partial class ProfilePage : ContentPage
    {
        Utente user;

        public ProfilePage(Utente us)
        {
            InitializeComponent();
           

            
            user = us;

            creaUtenteFake();

            if (user.fotoProfilo == null)
            {
                ProfilePicFrame.IsVisible = false;
                ChangeProfilePicButton.IsVisible = false;
                
            } else
            {
                ProfilePic.Source = user.fotoProfilo;
                ChangeProfilePicButton.IsVisible = true;
               
            }
            UsernameLabel.Text =this.user.username;
            AbilitaDistinitvi();
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

        async void RiempiPersonalityBar()
        {
            int puntiTotali = user.personalita1 + user.personalita2 + user.personalita3 + user.personalita4 + user.personalita5 + user.personalita6 + user.personalita7 + user.personalita8 + user.personalita9 + user.personalita10;
            await Tipo1.ProgressTo((double) user.personalita1/puntiTotali, 500, Easing.Linear);
            PercentualeTipo1.Text = Convert.ToString(user.personalita1 * 100 / puntiTotali) + "%";
            await Tipo2.ProgressTo((double)user.personalita2 / puntiTotali, 500, Easing.Linear);
            PercentualeTipo2.Text = Convert.ToString(user.personalita2 * 100 / puntiTotali) + "%";
            await Tipo3.ProgressTo((double)user.personalita3 / puntiTotali, 500, Easing.Linear);
            PercentualeTipo3.Text = Convert.ToString(user.personalita3 * 100 / puntiTotali) + "%";
            await Tipo4.ProgressTo((double)user.personalita4 / puntiTotali, 500, Easing.Linear);
            PercentualeTipo4.Text = Convert.ToString(user.personalita4 * 100 / puntiTotali) + "%";
            await Tipo5.ProgressTo((double)user.personalita5 / puntiTotali, 500, Easing.Linear);
            PercentualeTipo5.Text = Convert.ToString(user.personalita5 * 100 / puntiTotali) + "%";
            await Tipo6.ProgressTo((double)user.personalita6 / puntiTotali, 500, Easing.Linear);
            PercentualeTipo6.Text = Convert.ToString(user.personalita1 * 100 / puntiTotali) + "%";
            await Tipo7.ProgressTo((double)user.personalita6 / puntiTotali, 500, Easing.Linear);
            PercentualeTipo7.Text = Convert.ToString(user.personalita7 * 100 / puntiTotali) + "%";
            await Tipo8.ProgressTo((double)user.personalita8 / puntiTotali, 500, Easing.Linear);
            PercentualeTipo8.Text = Convert.ToString(user.personalita8 * 100 / puntiTotali) + "%";
            await Tipo9.ProgressTo((double)user.personalita9 / puntiTotali, 500, Easing.Linear);
            PercentualeTipo9.Text = Convert.ToString(user.personalita9 * 100 / puntiTotali) + "%";
            await Tipo10.ProgressTo((double)user.personalita10 / puntiTotali, 500, Easing.Linear);
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
        void AbilitaDistinitvi()
        {
            if (user.ViaggioMare >= 3)
                ViaggioMare1.IsVisible = false;
            if (user.Ristorante >= 3)
                Ristorante1.IsVisible = false;
            if (user.Sport >= 3)
                Sport1.IsVisible = false;
            if (user.Discoteca >= 3)
                Discoteca1.IsVisible = false;
            if (user.Compleanno >= 3)
                Compleanno1.IsVisible = false;
            if (user.Maschera >= 3)
                Maschera1.IsVisible = false;
            if (user.ViaggioMontagna >= 3)
                ViaggioMontagna1.IsVisible = false;
            if (user.ViaggioCitta >= 3)
                ViaggioCitta1.IsVisible = false;
            if (user.Cultura >= 3)
                Cultura1.IsVisible = false;
            if (user.Cocktail >= 3)
                Cocktail1.IsVisible = false;
            if (user.Casa >= 3)
                Casa1.IsVisible = false;
           

        }

        /* gestione foto profilo da fotocamera*/
        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void CameraClicked(Object sender, EventArgs e)
        {
           if(Device.iOS != null)
                ProfilePicFrame.Rotation=90;
           else
                ProfilePicFrame.Rotation=0;

          
            var foto = await MediaPicker.CapturePhotoAsync();

            if (foto != null)
            {
                var stream = await foto.OpenReadAsync();
                user.fotoProfilo = ImageSource.FromStream(() => stream);
                ProfilePicFrame.IsVisible = true;
                ChangeProfilePicButton.IsVisible = true;
                ProfilePic.Source = ImageSource.FromStream(() => stream);
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
                var stream = await foto.OpenReadAsync();
                user.fotoProfilo = ImageSource.FromStream(() => stream);
                ProfilePicFrame.IsVisible = true;
                ChangeProfilePicButton.IsVisible = true;
                ProfilePic.Source = ImageSource.FromStream(() => stream);
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
            user.fotoProfilo = null;
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
            BestFriendsFrame.TranslationY = 335;
            Scrolling.HeightRequest = 1000;
            RiempiPersonalityBar();
        }

        void ChiudiMenuPersonalita(Object sender, EventArgs e)
        {
            TriangoloChiudi.IsVisible = false;
            TriangoloApri.IsVisible = true;
            MenuPersonalita.IsVisible = false;
            BestFriendsFrame.TranslationY = 0;
            Scrolling.HeightRequest = 650;
            SvuotaPersonalityBar();
        }

        /* gestione foto profilo*/
        void AddPhoto(Object sender, EventArgs e)
        {
            AddPhotoFrame.IsVisible = true;
            if (user.fotoProfilo != null)
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
        }

        async void HomeClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }

        async void AddClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddExperiencePage());
        }

        async void SearchClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        async void RankingClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RankingPage());
        }

        async void SettingClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingPage());
        }

        async void DiaryClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DiaryPage());
        }

        async void FriendsClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FriendsPage());
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

            user.ViaggioMare = new Random().Next(5);
            user.Ristorante = new Random().Next(5);
            user.Sport = new Random().Next(5);
            user.Discoteca = new Random().Next(5);
            user.Compleanno = new Random().Next(5);
            user.Maschera = new Random().Next(5);
            user.ViaggioMontagna = new Random().Next(5);
            user.ViaggioCitta = new Random().Next(5);
            user.Cultura = new Random().Next(5);
            user.Cocktail = new Random().Next(5);
            user.Casa = new Random().Next(5);


        }


        
    }
}
