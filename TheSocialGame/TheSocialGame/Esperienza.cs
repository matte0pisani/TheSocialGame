using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TheSocialGame
{
    public class Esperienza
    {
        public string Titolo { get; set; }
        public byte[] Copertina { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public string Tipologia { get; set; }
        public List<Utente> ListaPartecipanti { get; set; }
        public bool privata { get; set; }
        public bool copertinaLiveIOS { get; set; }
        public bool live { get; set; }
        public List<string> Galleria { get; set; }  // probabilmente dovrà diventare una lista di byte[]/Image
        public List<string> luoghi { get; set; }
        public List<string> slogan { get; set; }
        public List<string> funfacts { get; set; }
        public List<string> playlist { get; set; }
        public List<string> recensioni { get; set; }
        public List<string> altro { get; set; }

        public Esperienza()
        {
            ListaPartecipanti = new List<Utente>();
            Galleria = new List<string>();
            luoghi = new List<string>();
            privata = false;
            copertinaLiveIOS = false;
            live = false;
            slogan = new List<string>();
            funfacts = new List<string>();
            playlist = new List<string>();
            recensioni = new List<string>();
            altro = new List<string>();
            



        }

        public void elimina()
        {
            foreach (Utente u in this.ListaPartecipanti)
            {
                u.esperienze.Remove(this);
                foreach (Utente us in this.ListaPartecipanti)
                    u.decrementaAmici(this.ListaPartecipanti);
            }
        }
    }
}
