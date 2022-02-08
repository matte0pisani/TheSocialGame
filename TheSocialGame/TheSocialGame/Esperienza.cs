using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TheSocialGame
{
    public class Esperienza
    {
        public int ID { get; set; }
        public string Titolo { get; set; }
        public byte[] Copertina { get; set; }
        public bool CopertinaLiveIOS { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public string Tipologia { get; set; }
        public bool Privata { get; set; }
        public bool Live { get; set; }
        public List<Utente> ListaPartecipanti { get; set; }
        public List<byte[]> Galleria { get; set; }
        public List<string> Luoghi { get; set; }
        public List<string> Slogan { get; set; }
        public List<string> Funfacts { get; set; }
        public List<string> Playlist { get; set; }
        public List<string> Recensioni { get; set; }
        public List<string> Altro { get; set; }

        public Esperienza()
        {
            ListaPartecipanti = new List<Utente>();
            Galleria = new List<byte[]>();
            Luoghi = new List<string>();
            Privata = false;
            CopertinaLiveIOS = false;
            Live = false;
            Slogan = new List<string>();
            Funfacts = new List<string>();
            Playlist = new List<string>();
            Recensioni = new List<string>();
            Altro = new List<string>();
        }


        public void Elimina()
        {
            foreach (Utente u in this.ListaPartecipanti)
            {
                u.Esperienze.Remove(this);
                foreach (Utente us in this.ListaPartecipanti)
                    u.DecrementaAmici(this.ListaPartecipanti);
            }
        }
    }
}
