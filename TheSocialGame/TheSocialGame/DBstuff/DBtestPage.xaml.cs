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
        static private DBcaller caller = new DBcaller();
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
            await caller.InsertUser(dummyUsr);
            ResultLabel.Text = string.Format("{0} {1} {2} {3} {4}", dummyUsr.ID, dummyUsr.Username, dummyUsr.Password, dummyUsr.PuntiSocial, dummyUsr.Livello);
        }

        // questi altri metodi al momento non funzionano più; da aggiornare

        private void Button_Clicked_Del(object sender, EventArgs e)
        {
            string connectString = ConfigurationManager.AppSettings["connectString"];
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            if (null != connectString)
            {
                System.Diagnostics.Debug.Print("Connection string: {0}", connectString);
                builder.ConnectionString = connectString;
            }
            else
            {
                System.Diagnostics.Debug.Print("Connection string is null");
                return;
            }

            string queryString =
                "DELETE FROM prova_db.dbo.tabella_prova WHERE nome = @nome";

            using (SqlConnection connection =
                new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@nome", NomeQuery.Text);

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    ResultLabel.Text = string.Format("\tNumber of modified rows: {0}", result);

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
                System.Diagnostics.Debug.Print("\n");
            }
        }

        private void Button_Clicked_Upd(object sender, EventArgs e)
        {
            string connectString = ConfigurationManager.AppSettings["connectString"];
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            if (null != connectString)
            {
                System.Diagnostics.Debug.Print("Connection string: {0}", connectString);
                builder.ConnectionString = connectString;
            }
            else
            {
                System.Diagnostics.Debug.Print("Connection string is null");
                return;
            }

            string queryString =
                "UPDATE prova_db.dbo.tabella_prova SET password = @pwd WHERE nome = @nome";

            using (SqlConnection connection =
                new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@nome", NomeQuery.Text);
                command.Parameters.AddWithValue("@pwd", "not null");

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    ResultLabel.Text = string.Format("\tNumber of modified rows: {0}", result);

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
                System.Diagnostics.Debug.Print("\n");
            }
        }
    }
}