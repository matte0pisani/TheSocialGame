using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TheSocialGame
{
    public partial class GalleryPage : ContentPage
    {
        public GalleryPage(List<string> foto)
        {
            InitializeComponent();

            List<Image> immagini = new List<Image>();

                foreach (string s in foto)
            {
                Image im = new Image();
                im.Source = ImageSource.FromFile(s);
                im.Aspect = Aspect.AspectFit;
                immagini.Add(im);

            }
            view.ItemsSource = immagini;

           
            
        }
    }
}
