﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame.DBstuff
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DBmanagerTestPage : ContentPage
    {
        public DBmanagerTestPage()
        {
            InitializeComponent();
        }

        private async void TriggerInserisci(object sender, EventArgs e)
        {
            Utente usr = new Utente
            {
                ID = IDentry.Text,
                Username = UsernameEntry.Text
            };

            InfoLabel.Text = (await DBmanager.InserisciNuovoUtente(usr)).ToString();
        }
    }
}