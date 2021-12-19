using System;
using PCLAppConfig;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DBtestPage : ContentPage
    {
        public DBtestPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked_Sel(object sender, EventArgs e)
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

            StringBuilder resultString = new StringBuilder();
            string s;

            using (SqlConnection connection =
                new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@nome", NomeQuery.Text);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    s = string.Format("\t\t{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));
                    Console.WriteLine(s);
                    resultString.AppendLine(s);

                    while (reader.Read())
                    {
                        s = string.Format("\t{0}\t{1}\t{2}", reader[0], reader[1], reader[2]);
                        System.Diagnostics.Debug.Print(s);
                        resultString.AppendLine(s);
                    }

                    reader.NextResult();
                    reader.Read();
                    s = string.Format("\tNumber of items: {0}\n", reader[0]);
                    System.Diagnostics.Debug.Print(s);
                    resultString.AppendLine(s);
                    ResultLabel.Text = resultString.ToString();

                    reader.Close();

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
                System.Diagnostics.Debug.Print("\n");
            }
        }

        private async void Button_Clicked_Ins(object sender, EventArgs e)
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
                "INSERT INTO prova_db.dbo.tabella_prova VALUES (5, @nome, @pwd )"; //se è una stringa non va fra singoli apici

            using (SqlConnection connection =
                new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@nome", NomeQuery.Text);
                command.Parameters.AddWithValue("@pwd", DBNull.Value);

                try
                {
                    await connection.OpenAsync(); //ma schermata ancora si blocca
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