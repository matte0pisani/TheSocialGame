using System;
using System.Collections.Generic;
using Xamarin.Forms;



namespace TheSocialGame
{
    public class Utente
    {
        public string username { get; set; }
        public string password { get; set; }
        public int puntiSocial { get; set; }
        public int livello { get; set; }
        public string pathFotoProfilo { get; set; }
        public bool fotoLiveiOS { get; set; }

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


        public Dictionary<string, (int, Dictionary<int, bool>)> listaDistintivi { get; set; }
        public List<Esperienza> esperienze { get; set; }
        public Color sfondo { get; set; }
        public Color primario { get; set; }
        public Color secondario { get; set; }
        public bool privato { get; set; }



        public Utente()
        {
           this.listaDistintivi = inizializzaListaDistintivi();
            this.sfondo = Color.GhostWhite;
            this.sfondo = Color.LightGray;
            this.sfondo = Color.WhiteSmoke;
            esperienze = new List<Esperienza>();
            fotoLiveiOS = false;
            privato = false;
        }


        private Dictionary<string, (int, Dictionary<int, bool>)> inizializzaListaDistintivi()
        {
            int livelloMax;
            livelloMax = 2;
            Dictionary<string,(int, Dictionary<int, bool>)> mappa = new Dictionary<string, (int, Dictionary<int, bool>)>();
            Dictionary<int, bool> livelliMare = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliRistorante = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliSport = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliDiscoteca = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliCompleanno = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliMaschera = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliMontagna = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliCitta = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliCultura = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliCocktail = new Dictionary<int, bool>();
            Dictionary<int, bool> livelliCasa = new Dictionary<int, bool>();


            for (int i = 1; i <= livelloMax; i++)
            {
                livelliMare.Add(i, false);
                livelliRistorante.Add(i, false);
                livelliSport.Add(i, false);
                livelliDiscoteca.Add(i, false);
                livelliCompleanno.Add(i, false);
                livelliMaschera.Add(i, false);
                livelliMontagna.Add(i, false);
                livelliCitta.Add(i, false);
                livelliCultura.Add(i, false);
                livelliCocktail.Add(i, false);
                livelliCasa.Add(i, false);
            }

            mappa.Add("ViaggioMare",(0, livelliMare));
            mappa.Add("Ristorante", (0, livelliRistorante));
            mappa.Add("Sport", (0, livelliSport));
            mappa.Add("Discoteca", (0, livelliDiscoteca));
            mappa.Add("Compleanno", (0, livelliCompleanno));
            mappa.Add("Maschera", (0, livelliMaschera));
            mappa.Add("ViaggioMontagna", (0, livelliMontagna));
            mappa.Add("ViaggioCitta", (0, livelliCitta));
            mappa.Add("Cultura", (0, livelliCultura));
            mappa.Add("Cocktail", (0, livelliCocktail));
            mappa.Add("Casa", (0, livelliCasa));
            return mappa;
        }

    }

   
}
