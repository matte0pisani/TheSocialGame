using System;
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

            InfoLabel.Text = (await DBmanager.InserisciUtente(usr)).ToString();
        }

        private async void TriggerSeleziona(object sender, EventArgs e)
        {
            Utente usr = await DBmanager.GetUtente(IDentry.Text);
            InfoLabel.Text = usr.ToString();
        }

        private async void TriggerAmici(object sender, EventArgs e)
        {
            Dictionary<Utente, int> res = await DBmanager.GetTuttiAmici(IDentry.Text);
            InfoLabel.Text = res.ToString();
        }
    }
}