using System;
using System.Collections.Generic;
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
            if(user.puntiSocial == 0) // provvisorio!!!!
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
            }

            if(user.fotoProfilo == null)
            {
                ProfilePicFrame.IsVisible = false;
                ChangeProfilePicButton.IsVisible = false;
                
            } else
            {
                ProfilePic.Source = user.fotoProfilo;
                ChangeProfilePicButton.IsVisible = true;
               
            }
            UsernameLabel.Text =this.user.username;
            RiempiProgressBar();
            if (user.livello < 10)
                 LabelLevelSingle.Text = Convert.ToString(user.livello);
            else LabelLevelDouble.Text = Convert.ToString(user.livello);

            ConfermaEliminazioneFrame.IsVisible = false;
            MenuPersonalita.IsVisible = false;
            TriangoloChiudi.IsVisible = false;
            AddPhotoFrame.IsVisible = false;
            BindingContext = this;
        }


        

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

        void SvuotaPersonalityBar()
        {
            Tipo1.Progress = 0;
            Tipo2.Progress = 0;
            Tipo3.Progress = 0;
            Tipo4.Progress = 0;
            Tipo5.Progress = 0;
            Tipo6.Progress = 0;
            Tipo7.Progress = 0;
            Tipo8.Progress = 0;
            Tipo9.Progress = 0;
            Tipo10.Progress = 0;
        }

        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void CameraClicked(Object sender, EventArgs e)
        {
           
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

        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void FromGalleryClicked(Object sender, EventArgs e)
        {
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

        void ApriMenuPersonalita(Object sender, EventArgs e)
        {
            TriangoloChiudi.IsVisible = true;
            TriangoloApri.IsVisible = false;
            MenuPersonalita.IsVisible = true;
            RiempiPersonalityBar();
        }

        void ChiudiMenuPersonalita(Object sender, EventArgs e)
        {
            TriangoloChiudi.IsVisible = false;
            TriangoloApri.IsVisible = true;
            MenuPersonalita.IsVisible = false;
            SvuotaPersonalityBar();
        }
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
    }
}
