using System;
using PCLAppConfig;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DBtestPage : ContentPage
    {
        public DBtestPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string settings = ConfigurationManager.AppSettings["connectString"];
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            if (null != settings)
            {
                string connectString = settings;
                System.Diagnostics.Debug.Print("Modified: {0}", connectString);

                builder.ConnectionString = connectString;

                builder.Password = "";
                System.Diagnostics.Debug.Print("Modified: {0}", builder.ConnectionString);
            }
            else
                return;

            string queryString =
                "SELECT * FROM prova_db.dbo.tabella_prova "
                    + "WHERE nome='pippo' "
                    + "ORDER BY id;";

            // Provare una query con dei parametri esterni

            using (SqlConnection connection =
                new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        System.Diagnostics.Debug.Print("\t{0}\t{1}\t{2}",
                            reader[0], reader[1], reader[2]);
                    }
                    reader.Close();
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