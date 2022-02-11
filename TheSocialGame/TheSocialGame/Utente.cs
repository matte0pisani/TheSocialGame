using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.Runtime.Internal.Transform;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TheSocialGame
{
    public class Utente
    {
        public static int maxLivDistintivo = 2;
        public static int sogliaPrimoLivello = 3;
        public static int sogliaSecondoLivello = 7;
        public string ID { get; set; }
        public string Username { get; set; }
        public int PuntiSocial { get; set; }
        public int PuntiEsperienza { get; set; }
        public int Livello { get; set; }
        public byte[] FotoBytes { get; set; }
        public bool FotoLiveiOS { get; set; }

        /* PERSONALITA*/
        public int Personalita1 {get; set;}
        public int Personalita2 { get; set; }
        public int Personalita3 { get; set; }
        public int Personalita4 { get; set; }
        public int Personalita5 { get; set; }

        public Dictionary<string, (int, Dictionary<int, bool>)> ListaDistintivi { get; set; }
        public List<Esperienza> Esperienze { get; set; }
        public Dictionary<Utente, int> Amici { get; set; }
        public Color Sfondo { get; set; }
        public Color Primario { get; set; }
        public Color Secondario { get; set; }
        public bool Privato { get; set; }


        [Preserve]
        [JsonConstructor]
        public Utente()
        {
            ID = "dummy";
            Username = "dummy";
            PuntiSocial = PuntiEsperienza = 0;
            Livello = 1;    
            FotoBytes = null;
            FotoLiveiOS = false;
            Personalita1 = Personalita2 = Personalita3 = Personalita4 = Personalita5 = 1;
            ListaDistintivi = InizializzaListaDistintivi();
            Esperienze = new List<Esperienza>();
            Amici = new Dictionary<Utente, int>();
            Sfondo = Color.GhostWhite;
            Primario = Color.LightGray;
            Secondario = Color.WhiteSmoke;
            Privato = false;
            // puntiFake();
        }


        private Dictionary<string, (int, Dictionary<int, bool>)> InizializzaListaDistintivi()
        {
            int livelloMax = maxLivDistintivo;
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
            classifica.Add(this, this.PuntiEsperienza);
            foreach (Utente u in this.Amici.Keys)
            {
                classifica.Add(u, u.PuntiEsperienza);
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
            classifica.Add(this, this.PuntiEsperienza+this.Livello+this.Personalita1+this.Personalita2+this.Personalita3+this.Personalita4+this.Personalita5);

            foreach (Utente u in this.Amici.Keys)
            {
                int punteggio = u.PuntiEsperienza + u.Livello +u.Personalita1 +u.Personalita2 + u.Personalita3 + u.Personalita4 + u.Personalita5;
                classifica.Add(u, punteggio);
            }
            Dictionary<Utente, int> ordinata = new Dictionary<Utente, int>();
            foreach (KeyValuePair<Utente, int> coppia in classifica.OrderByDescending(key => key.Value))
            {
                ordinata.Add(coppia);
            }

            return ordinata;
        }


        public void AggiungiAmici(List<Utente> partecipanti)
        {
            foreach (Utente u in partecipanti)
            {
                if (!(u == this))
                {
                    if (this.Amici.Keys.Contains(u)) this.Amici[u]++;
                    else this.Amici.Add(u, 1);
                }
              
            }
        }

        public void DecrementaAmici(List<Utente> partecipanti)
        {
            foreach (Utente u in partecipanti)
            {
                if (this.Amici.ContainsKey(u))
                {
                    if (this.Amici[u] == 1)
                        this.Amici.Remove(u);
                    else this.Amici[u]--;
                }
            }
        }


        public Dictionary<Utente, int> GetBestFriends()
        {
            Dictionary<Utente, int> best = new Dictionary<Utente, int>();
            foreach (KeyValuePair<Utente, int> coppia in Amici.OrderByDescending(key => key.Value).Take(3))
            {
                best.Add(coppia);
            }
            return best;
        }

        public void Elimina()
        {
            // da implementare eliminazione da database
            return;
        }

        public void PuntiFake()
        {
            this.PuntiSocial = new Random().Next(100);
            this.Livello = (this.PuntiSocial / 10) + 1;
            this.Personalita1 = new Random().Next(100);
            this.Personalita2 = new Random().Next(100);
            this.Personalita3 = new Random().Next(100);
            this.Personalita4 = new Random().Next(100);
            this.Personalita5 = new Random().Next(100);
        }
        
        // forse questi equals e hash code non servono (e hanno poco senso)
        public override bool Equals(object obj)
        {
            Utente that = (Utente)obj;
            return this.Username == that.Username;
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }

    }

}
