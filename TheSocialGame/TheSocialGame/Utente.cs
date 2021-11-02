using System;
using Xamarin.Forms;

namespace TheSocialGame
{
    public class Utente
    {
        public string username { get; set; }
        public string password { get; set; }
        public int puntiSocial { get; set; }
        public int livello { get; set; }
        public ImageSource fotoProfilo { get; set; }

        /* PERSONALITA*/
        public int personalita1 {get; set;}
        public int personalita2 { get; set; }
        public int personalita3 { get; set; }
        public int personalita4 { get; set; }
        public int personalita5 { get; set; }
        public int personalita6 { get; set; }
        public int personalita7 { get; set; }
        public int personalita8 { get; set; }
        public int personalita9 { get; set; }
        public int personalita10 { get; set; }

        /* MIGLIORI AMICI */
        public Utente BestFriend1 { get; set; }
        public Utente BestFriend2 { get; set; }
        public Utente BestFriend3 { get; set; }

        /* DISTINTIVI */
        public int ViaggioMare { get; set; }
        public int Ristorante { get; set; }
        public int Sport { get; set; }
        public int Discoteca { get; set; }
        public int Compleanno { get; set; }
        public int Maschera { get; set; }
        public int ViaggioMontagna { get; set; }
        public int ViaggioCitta { get; set; }
        public int Cultura { get; set; }
        public int Cocktail { get; set; }
        public int Casa { get; set; }




    }

   
}
