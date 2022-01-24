using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Runtime.Internal.Transform;
using Xamarin.Forms;



namespace TheSocialGame
{
    public class Utente
    {
        public string ID { get; set; }
        public string username { get; set; }
        public int puntiSocial { get; set; }
        public int puntiEsperienza { get; set; }
        public int livello { get; set; }
        public byte[] fotoBytes { get; set; }
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

       

        public Dictionary<string, (int, Dictionary<int, bool>)> listaDistintivi { get; set; }
        public List<Esperienza> esperienze { get; set; }
        public Dictionary<Utente, int> amici { get; set; }
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
            amici = new Dictionary<Utente, int>();
            fotoLiveiOS = false;
            privato = false;
            puntiEsperienza = 0;
            puntiFake();
            
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



        public Dictionary<Utente, int> ClassificaEsperienze()
        {


            Dictionary<Utente, int> classifica = new Dictionary<Utente, int>();
            classifica.Add(this, this.puntiEsperienza);
            foreach (Utente u in this.amici.Keys)
            {
                classifica.Add(u, u.puntiEsperienza);
            }

            Dictionary<Utente, int> ordinata = new Dictionary<Utente, int>();
            foreach (KeyValuePair< Utente, int> coppia in classifica.OrderByDescending(key => key.Value))
            {
                ordinata.Add(coppia);
            }

            return ordinata;
        }

        public Dictionary<Utente, int> ClassificaGenerale()
        {
            Dictionary<Utente, int> classifica = new Dictionary<Utente, int>();
            classifica.Add(this, this.puntiEsperienza+this.livello+this.personalita1+this.personalita2+this.personalita3+this.personalita4+this.personalita5+this.personalita6+this.personalita7+this.personalita8+this.personalita9+this.personalita10);

            foreach (Utente u in this.amici.Keys)
            {
                int punteggio = u.puntiEsperienza + u.livello +u.personalita1 +u.personalita2 + u.personalita3 + u.personalita4 + u.personalita5 + u.personalita6 + u.personalita7 + u.personalita8 + u.personalita9 + u.personalita10;
                classifica.Add(u, punteggio);
            }
            Dictionary<Utente, int> ordinata = new Dictionary<Utente, int>();
            foreach (KeyValuePair<Utente, int> coppia in classifica.OrderByDescending(key => key.Value))
            {
                ordinata.Add(coppia);
            }

            return ordinata;
        }


        public void aggiungiAmici(List<Utente> partecipanti)
        {
            foreach (Utente u in partecipanti)
            {
                if (!(u == this))
                {

                    if (this.amici.Keys.Contains(u)) this.amici[u]++;
                    else this.amici.Add(u, 1);
                }
              
            }
        }

        public void decrementaAmici(List<Utente> partecipanti)
        {
            foreach (Utente u in partecipanti)
            {
                if (this.amici.ContainsKey(u))
                {
                    if (this.amici[u] == 1)
                        this.amici.Remove(u);
                    else this.amici[u]--;
                }
            }
        }


        public Dictionary<Utente, int> getBestFriends()
        {
            Dictionary<Utente, int> best = new Dictionary<Utente, int>();
            foreach (KeyValuePair<Utente, int> coppia in amici.OrderByDescending(key => key.Value).Take(3))
            {
                best.Add(coppia);
            }
            return best;
        }

        public void elimina()
        {
            // da implementare eliminazione da database
            return;
        }

        public void puntiFake()
        {
            this.puntiSocial = new Random().Next(100);
            this.livello = (this.puntiSocial / 10) + 1;
            this.personalita1 = new Random().Next(100);
            this.personalita2 = new Random().Next(100);
            this.personalita3 = new Random().Next(100);
            this.personalita4 = new Random().Next(100);
            this.personalita5 = new Random().Next(100);
            this.personalita6 = new Random().Next(100);
            this.personalita7 = new Random().Next(100);
            this.personalita8 = new Random().Next(100);
            this.personalita9 = new Random().Next(100);
            this.personalita10 = new Random().Next(100);

        }










    }

   
}
