using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TheSocialGame
{
    public partial class SettingPage : ContentPage
    {
        Utente user { get; set; }

        public SettingPage(Utente u)
        {
            user = u;
            InitializeComponent();
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

        async void AddClicked(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddExperiencePage(user));
            Navigation.RemovePage(this);
        }

        async void BackClicked(Object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
