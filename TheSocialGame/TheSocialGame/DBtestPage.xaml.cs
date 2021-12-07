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
                "SELECT * FROM prova_db.dbo.tabella_prova "
                    + "WHERE nome= @nome "
                    + "ORDER BY id;" +
                "SELECT COUNT(*) AS totale FROM prova_db.dbo.tabella_prova "
                    + "WHERE nome = @nome ";

            using (SqlConnection connection =
                new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@nome", NomeQuery.Text);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Console.WriteLine("\t\t{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                    while (reader.Read())
                    {
                        System.Diagnostics.Debug.Print("\t{0}\t{1}\t{2}",
                            reader[0], reader[1], reader[2]);
                    }

                    reader.NextResult();
                    reader.Read();
                    System.Diagnostics.Debug.Print("\tNumber of items: {0}\n", reader[0]);

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