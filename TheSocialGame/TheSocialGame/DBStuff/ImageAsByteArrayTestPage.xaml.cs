using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageAsByteArrayTestPage : ContentPage
    {
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
    }
}