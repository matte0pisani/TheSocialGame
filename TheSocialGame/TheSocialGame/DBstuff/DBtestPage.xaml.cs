using System;
using PCLAppConfig;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using TheSocialGame.DBstuff;
using System.Text;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DBtestPage : ContentPage
    {
        private static readonly DBcaller caller = new DBcaller();
        public DBtestPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked_Sel(object sender, EventArgs e)
        {
            List<UserSimple> result = new List<UserSimple>();
            ResultLabel.Text = "Waiting...";
            await caller.GetAllUsers(NomeQuery.Text, result);

            StringBuilder sbuild = new StringBuilder();
            sbuild.AppendLine(string.Format("Number of users: {0}", result.Count));
            foreach (UserSimple u in result)
            {
                sbuild.AppendLine(string.Format("{0} {1} {2} {3} {4}", u.ID, u.Username, u.Password, u.PuntiSocial, u.Livello));
            }
            ResultLabel.Text = sbuild.ToString();
        }

        private async void Button_Clicked_Ins(object sender, EventArgs e)
        {
            UserSimple dummyUsr = new UserSimple(NomeQuery.Text, "dummy");
            ResultLabel.Text = "Waiting...";
            await caller.InsertUser(dummyUsr);
            ResultLabel.Text = string.Format("{0} {1} {2} {3} {4}", dummyUsr.ID, dummyUsr.Username, dummyUsr.Password, dummyUsr.PuntiSocial, dummyUsr.Livello);
        }

        private async void Button_Clicked_Del(object sender, EventArgs e)
        {
            List<UserSimple> result = new List<UserSimple>();
            ResultLabel.Text = "Waiting...";
            int numberDeleted = await caller.DeleteAllUsers(NomeQuery.Text, result);

            if (numberDeleted == 0)
                ResultLabel.Text = "There are no users with such name\n";
            else
            {
                StringBuilder sbuild = new StringBuilder();
                sbuild.AppendLine(string.Format("Number of deleted users: {0}", result.Count));
                foreach (UserSimple u in result)
                {
                    sbuild.AppendLine(string.Format("{0} {1} {2} {3} {4}", u.ID, u.Username, u.Password, u.PuntiSocial, u.Livello));
                }
                ResultLabel.Text = sbuild.ToString();
            }

        }


        private async void Button_Clicked_Upd(object sender, EventArgs e)
        {
            UserSimple dummyUsr = new UserSimple(4, NomeQuery.Text, "dummy", 0, 0);     // andiamo sempre a modificare, in questo esempio, la quarta riga
            ResultLabel.Text = "Waiting...";
            bool result = await caller.UpdateUser(dummyUsr);
            if (result)
                ResultLabel.Text = "User #4 updated successfully\n";
            else
                ResultLabel.Text = "Some error occurred while trying to update the user\n";
        }
    }
}