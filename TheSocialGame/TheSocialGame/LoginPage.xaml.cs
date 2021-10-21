﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public bool IsUsernameSet { get; set; }
        public bool IsPasswordSet { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            StarImage.IsVisible = false;
            IsUsernameSet = false;
            IsPasswordSet = false;
            BindingContext = this;
        }

        private bool IsUserValid()
        {
            return IsUsernameSet && IsPasswordSet;
        }

        private void RefreshStarVisibility()
        {
            StarImage.IsVisible = IsUsernameSet && IsPasswordSet;
        }

        async private void Star_Tapped(object sender, EventArgs e)
        {
            if (IsUserValid())
                await Navigation.PushAsync(new ProfilePage(null)); // provvisoriamente manda null come parametro, poi dovrà mandare il riferimento all'utente che sta accedendo al suo profilo
        }

        private void UsernameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsUsernameSet = UsernameEntry.Text.Length > 0;
            RefreshStarVisibility();
        }

        private void PasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsPasswordSet = PasswordEntry.Text.Length > 0;
            RefreshStarVisibility();
        }
        private void SignUpLabel_Tapped(object sender, EventArgs e)
        {

        }

        private void ForgottenPasswordLabel_Tapped(object sender, EventArgs e)
        {

        }

    }
}