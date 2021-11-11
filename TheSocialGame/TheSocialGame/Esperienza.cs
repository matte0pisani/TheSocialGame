using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TheSocialGame
{
    public class Esperienza
    {
        public string Titolo { get; set; }
        public ImageSource Copertina { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public string Tipologia { get; set; }
        public List<Utente> ListaPartecipanti { get; set; }
        public bool privata { get; set; }

        public Esperienza()
        {
            ListaPartecipanti = new List<Utente>();
            privata = false;
        }
    }
}
