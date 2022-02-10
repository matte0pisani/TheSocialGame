using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace TheSocialGame
{
    public class Esperienza
    {
        public bool Dirty { get; set; } // per conoscere se l'esperienza è stata creata/modificata nella sessione corrente
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

        [Preserve]
        [JsonConstructor]
        public Esperienza()
        {
            Dirty = false;
            ID = -1;
            Titolo = "dummy";
            Copertina = null;
            CopertinaLiveIOS = false;
            DataInizio = DateTime.Now;
            DataFine = DateTime.Now;
            Tipologia = "dummy";
            Privata = false;
            Live = false;
            ListaPartecipanti = new List<Utente>();
            Galleria = new List<byte[]>();
            Luoghi = new List<string>();
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
