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
            /*Utente di default finchè non abbiamo database*/

            if(us == null)
            {
                this.user = new Utente();
                user.username = "Cavia";
                user.puntiSocial = new Random().Next(100);
                user.livello = (user.puntiSocial / 10) + 1;


            }
            else user = us;
            if(user.fotoProfilo == null)
            {
                ProfilePicFrame.IsVisible = false;
            } else
            {
                ProfilePic.Source = user.fotoProfilo; 
            }

            UsernameLabel.Text =this.user.username;
            RiempiProgressBar();
            if (user.livello < 10)
                 LabelLevelSingle.Text = Convert.ToString(user.livello);
            else LabelLevelDouble.Text = Convert.ToString(user.livello);
               

            AddPhotoFrame.IsVisible = false;
            BindingContext = this;
        }


        

        async void RiempiProgressBar() {
            await SocialPointBar.ProgressTo((double)(user.puntiSocial % 10) / 10, 3000, Easing.Linear);
        }

        async void CameraClicked(Object sender, EventArgs e)
        {
            var foto = await MediaPicker.CapturePhotoAsync();

            if (foto != null)
            {
                var stream = await foto.OpenReadAsync();
                user.fotoProfilo = ImageSource.FromStream(() => stream);
                ProfilePicFrame.IsVisible = true;
                ProfilePic.Source = ImageSource.FromStream(() => stream);
            }
        }

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
                ProfilePic.Source = ImageSource.FromStream(() => stream);
            }
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
        }

        void ExitFromAddPhoto(Object sender, EventArgs e)
        {
            AddPhotoFrame.IsVisible = false;
        }
    }
}
