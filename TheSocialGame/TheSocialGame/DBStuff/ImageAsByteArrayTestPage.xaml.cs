using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageAsByteArrayTestPage : ContentPage
    {
        private static readonly string connStr = "";
        private static readonly string insertQueryStr = "INSERT INTO tsg.dbo.test_image VALUES (@bytes)";
        private static readonly string selectQueryStr = "SELECT bytes FROM tsg.dbo.test_image WHERE id = @id";
        public byte[] ImageBytes { get; set; }
        public ImageAsByteArrayTestPage()
        {
            InitializeComponent();
            ImageBytes = null;
        }

        private async void ImageFromGallery(object sender, EventArgs e)
        {
            FileResult foto = await MediaPicker.PickPhotoAsync();

            if (foto != null)
            {
                using (Stream stream = await foto.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        ImageBytes = ms.ToArray();
                    }
                }
            }
        }

        private async void ImageFromCamera(object sender, EventArgs e)
        {
            FileResult foto = await MediaPicker.CapturePhotoAsync();

            if (foto != null)
            {
                using (Stream stream = await foto.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        ImageBytes = ms.ToArray();
                    }
                }
            }
        }

        private void ShowFromBytes(object sender, EventArgs e)
        {
            if (ImageBytes != null)
            {
                ImageFromBytes.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(ImageBytes);
                });
                InfoLabel.Text = "Image size: " + ImageBytes.Length + " bytes";
            }
            else
                InfoLabel.Text = "No image available";
        }

        private void DeleteArray(object sender, EventArgs e)
        {
            ImageBytes = null;
            InfoLabel.Text = "Content of the array deleted";
            ImageFromBytes.Source = null;
        }

        private async void UploadInDb(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connStr);

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(insertQueryStr, connection);
                command.Parameters.Add("@bytes", SqlDbType.Binary);
                if (ImageBytes == null)
                {
                    command.Parameters["@bytes"].Value = DBNull.Value;
                }
                else
                {
                    command.Parameters["@bytes"].Value = ImageBytes;
                }


                try
                {
                    await connection.OpenAsync();
                    int count = await command.ExecuteNonQueryAsync();
                    InfoLabel.Text = "Image succesfully uploaded";
                }
                catch(Exception)
                {
                    InfoLabel.Text = "An error occurred while trying to upload the image";
                }
            }
        }

        private async void DownloadFromDb(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connStr);

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(selectQueryStr, connection);
                command.Parameters.AddWithValue("@id", Int32.Parse(ImageId.Text));

                try
                {
                    await connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var res = reader[0];
                        if(res == DBNull.Value)
                        {
                            ImageBytes = null;
                            InfoLabel.Text = "Image downloaded successfully, but it's an empty image";
                        }
                        else
                        {
                            ImageBytes = (byte[])res;
                            InfoLabel.Text = "Image downloaded successfully";
                        }
                    }

                    
                }
                catch (Exception)
                {
                    InfoLabel.Text = "Something went wrong while downloading the image with id " + Int32.Parse(ImageId.Text);
                }
            }
        }
    }
}
