using System;
using Xamarin.Forms;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLAppConfig;
using System.Collections.Generic;

namespace TheSocialGame
{
    public static class DBmanager
    {
        private static Utente current = null;
        private static Utente ConvertiJsonInUtente(JObject json)
        {
            return new Utente
            {
                ID = json["ID"].ToString(),
                Username = json["Username"].ToString(),
                PuntiSocial = (int)json["PuntiSocial"],
                PuntiEsperienza = (int)json["PuntiEsperienza"],
                Livello = (int)json["Livello"],
                FotoBytes = Convert.FromBase64String(json["FotoBytes"].ToString()),
                FotoLiveiOS = (bool)json["FotoLiveiOS"],
                Personalita1 = (int)json["Personalita1"],
                Personalita2 = (int)json["Personalita2"],
                Personalita3 = (int)json["Personalita3"],
                Personalita4 = (int)json["Personalita4"],
                Personalita5 = (int)json["Personalita5"],
                Sfondo = Color.FromHex(json["Sfondo"].ToString()),
                Primario = Color.FromHex(json["Primario"].ToString()),
                Secondario = Color.FromHex(json["Secondario"].ToString()),
                Privato = (bool)json["Privato"]
            };
        }

        private static JObject ConvertiUtenteInJson(Utente usr)
        {
            JObject res = new JObject();
            res.Add("ID", usr.ID);
            res.Add("Username", usr.Username);
            res.Add("PuntiSocial", usr.PuntiSocial);
            res.Add("PuntiEsperienza", usr.PuntiEsperienza);
            res.Add("Livello", usr.Livello);
            if (usr.FotoBytes != null) { res.Add("FotoBytes", Convert.ToBase64String(usr.FotoBytes)); }
            else { res.Add("FotoBytes", null); }
            res.Add("FotoLiveiOS", usr.FotoLiveiOS);
            res.Add("Personalita1", usr.Personalita1);
            res.Add("Personalita2", usr.Personalita2);
            res.Add("Personalita3", usr.Personalita3);
            res.Add("Personalita4", usr.Personalita4);
            res.Add("Personalita5", usr.Personalita5);
            res.Add("Sfondo", usr.Sfondo.ToHex());
            res.Add("Primario", usr.Primario.ToHex());
            res.Add("Secondario", usr.Secondario.ToHex());
            res.Add("Privato", usr.Privato);
            return res;
        }

        private static JObject ConvertiEsperienzaInJson(Esperienza exp)
        {
            JObject res = new JObject();
            res.Add("Titolo", exp.Titolo);
            if (exp.Copertina != null) { res.Add("Copertina", Convert.ToBase64String(exp.Copertina)); }
            else { res.Add("Copertina", null); }
            res.Add("CopertinaLiveiOS", exp.CopertinaLiveiOS);
            res.Add("DataInizio", exp.DataInizio.ToString());
            res.Add("DataFine", exp.DataFine.ToString());
            res.Add("Tipologia", exp.Tipologia);
            res.Add("Privata", exp.Privata);
            res.Add("Live", exp.Live);
            JArray members = new JArray();
            foreach (Utente mem in exp.ListaPartecipanti)
            {
                members.Add(mem.ID);
            }
            res.Add("Partecipanti", members.ToString());
            return res;
        }

        public static async Task<bool> InserisciUtente(Utente usr)
        {
            string url = ConfigurationManager.AppSettings["insertUserAPI"];
            System.Diagnostics.Debug.Print("Inserisco in DB utente {0} {1}\n", usr.ID, usr.Username);

            string jstring = ConvertiUtenteInJson(usr).ToString();
            System.Diagnostics.Debug.Print("Creato JSON da oggetto Utente: {0}\n", jstring);

            var body = new StringContent(jstring, Encoding.UTF8, "application/json");
            System.Diagnostics.Debug.Print("POST body creato\n");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url, body);
            System.Diagnostics.Debug.Print("Risposta ricevuta da insertUserAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La insertUserAPI non ha risposto correttamente");

            string resultString = await response.Content.ReadAsStringAsync();
            if (!Boolean.TryParse(resultString, out bool result)) throw new Exception("Errore nel parsing del risultato");
            System.Diagnostics.Debug.Print("Parsing del risultato corretto: {0}\n", result);

            return result;
        }

        public static async void InserisciEsperienza(Esperienza exp)
        {
            string url = ConfigurationManager.AppSettings["insertExperienceAPI"];
            System.Diagnostics.Debug.Print("Inserisco in DB utente {0} {1}\n", exp.ID, exp.Titolo);

            string jstring = ConvertiEsperienzaInJson(exp).ToString();
            System.Diagnostics.Debug.Print("Creato JSON da oggetto Esperienza: {0}\n", jstring);

            var body = new StringContent(jstring, Encoding.UTF8, "application/json");
            System.Diagnostics.Debug.Print("POST body creato\n");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url, body);
            System.Diagnostics.Debug.Print("Risposta ricevuta da insertExperienceAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La insertExperienceAPI non ha risposto correttamente");

            string resultString = await response.Content.ReadAsStringAsync();
            if (!Int32.TryParse(resultString, out int eid)) throw new Exception("Errore nel parsing del risultato per insertExperienceAPI");
            System.Diagnostics.Debug.Print("Parsing del risultato corretto per insertExperienceAPI: {0}\n", eid);

            exp.ID = eid;
        }

        /**
         * Inizializza completamente un utente, quindi anche la lista degli amici (a loro volta inizializzati solo a livello "base") e delle esperienze
         */
        public static async Task<Utente> GetUtente(string userID)
        {
            Utente res = await GetUtenteBase(userID);
            current = res;
            res.Amici = await GetTuttiAmici(res.ID);
            res.Esperienze = await GetTutteEsperienze(res.ID);
            return res;
        }

        /**
        * Inizializza soltanto le proprietà "semplici" dell'utente e i distintivi, quindi no amici/esperienze, che sono lasciate così come le inizializza
        * Utente() (cioè vuote).
        */
        public static async Task<Utente> GetUtenteBase(string userID)
        {
            string url = string.Format(ConfigurationManager.AppSettings["selectUserAPI"], userID);
            System.Diagnostics.Debug.Print("Prendo l'utente con ID: '{0}'\n", userID);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Risposta ricevuta da selectUserAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La selectUserAPI non ha risposto correttamente");

            string jsonString = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.Print("Risposta: {0}\n", jsonString);

            Utente res = ConvertiJsonInUtente(JObject.Parse(jsonString));
            System.Diagnostics.Debug.Print("Utente creato\n");

            if (await GetDistintivi(res)) { System.Diagnostics.Debug.Print("Caricamento distintivi completato\n"); }
            else { System.Diagnostics.Debug.Print("Errore nel caricamento dei distintivi\n"); }

            return res;
        }

        public static async Task<Utente> GetUtenteBasePerNome(string username)
        {
            string url = string.Format(ConfigurationManager.AppSettings["selectUserByNameAPI"], username);
            System.Diagnostics.Debug.Print("Prendo l'utente con username: '{0}'\n", username);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Risposta ricevuta da selectUserByNameAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La selectUserByNameAPI non ha risposto correttamente");

            string jsonString = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.Print("Risposta: {0}\n", jsonString);

            if (jsonString == "{}")
            {
                System.Diagnostics.Debug.Print("Non è stato trovato alcun utente con nome: {0}", username);
                return null;
            }
            Utente res = ConvertiJsonInUtente(JObject.Parse(jsonString));
            System.Diagnostics.Debug.Print("Utente creato\n");

            if (await GetDistintivi(res)) { System.Diagnostics.Debug.Print("Caricamento distintivi completato\n"); }
            else { System.Diagnostics.Debug.Print("Errore nel caricamento dei distintivi\n"); }

            return res;
        }
        
        private static async Task<bool> GetDistintivi(Utente usr)
        {
            string url = string.Format(ConfigurationManager.AppSettings["selectBadgesAPI"], usr.ID);
            System.Diagnostics.Debug.Print("Prendo i distintivi dell'utente con ID: '{0}'\n", usr.ID);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Risposta ricevuta da selectBadgesAPI\n");
            if (!response.IsSuccessStatusCode) return false;

            JArray resArray = JArray.Parse(await response.Content.ReadAsStringAsync());
            System.Diagnostics.Debug.Print("Risposta: {0}\n", resArray.ToString());

            foreach (JObject item in resArray)
            {
                string tipoExp = item["Nome"].ToString();
                int livMax = (int)item["LivMax"];
                int numExp = (int)item["NumeroExp"]; 
                Dictionary<int, bool> badges = new Dictionary<int, bool>();
                for(int i = 1; i <= Constants.maxLivDistintivo; i++)
                {
                    badges[i] = false;
                }
                for(int i = 1; i <= livMax; i++)
                {
                    badges[i] = true;
                }
                usr.ListaDistintivi[tipoExp] = (numExp, badges);
                System.Diagnostics.Debug.Print("Aggiunti distintivi per esperienza: {0}, n. exp. {1} e max.lev. {2}\n", tipoExp, numExp, livMax);
            }

            return true;
        }

        private static async Task<Dictionary<Utente, int>> GetTuttiAmici(string uid)
        {
            Dictionary<Utente, int> result = new Dictionary<Utente, int>();
            string url = string.Format(ConfigurationManager.AppSettings["selectFriendsAPI"], uid);
            System.Diagnostics.Debug.Print("Prendo tutti gli amici dell'utente con ID: '{0}'\n", uid);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Risposta ricevuta da selectFriendsAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La selectFriendsAPI non ha risposto correttamente");

            JArray resArray = JArray.Parse(await response.Content.ReadAsStringAsync());
            System.Diagnostics.Debug.Print("Risposta: {0}\n", resArray.ToString());

            foreach (JObject item in resArray)
            {
                result.Add(await GetUtenteBase(item["ID"].ToString()), (int)item["NumeroEsperienze"]);
                System.Diagnostics.Debug.Print("Recuperato amico da DB e aggiunto a risultato");
            }
            System.Diagnostics.Debug.Print("Calcolo risultato completato; ritorno");

            return result;
        }

        private static async Task<List<Esperienza>> GetTutteEsperienze(string uid)
        {
            string url = string.Format(ConfigurationManager.AppSettings["selectExperiencesAPI"], uid);
            System.Diagnostics.Debug.Print("Prendo tutte le esperienze dell'utente con ID: '{0}'\n", uid);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Risposta ricevuta da selectExperiencesAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La selectExperiencesAPI non ha risposto correttamente");

            JArray resArray = JArray.Parse(await response.Content.ReadAsStringAsync());
            System.Diagnostics.Debug.Print("Risposta: {0}\n", resArray.ToString());

            List<Esperienza> result = JsonConvert.DeserializeObject<List<Esperienza>>(resArray.ToString());
            System.Diagnostics.Debug.Print("Conversione risposta riuscita\n");

            foreach (Esperienza exp in result)
            {
                GetInfoEsperienza(exp, uid);    
                // non fatto await così che dei thread separati carichino le esperienze, lavorando in parallelo; ma il thread "padre" potrebbe ritornare
                // la lista di esperienze prima che siano "complete"
            }
            System.Diagnostics.Debug.Print("Caricate informazioni sulle esperienze\n");

            return result;
        }

        private static async void GetInfoEsperienza(Esperienza exp, string uid)
        {
            string url = string.Format(ConfigurationManager.AppSettings["selectExperienceInfoAPI"], exp.ID);
            System.Diagnostics.Debug.Print("Prendo tutte le informazioni dell'esperienza con ID: '{0}'\n", exp.ID);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Risposta ricevuta da selectExperienceInfoAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La selectExperienceInfoAPI non ha risposto correttamente");

            string jansw = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.Print("{0}\n", jansw);

            JArray resArray = JArray.Parse(jansw);
            System.Diagnostics.Debug.Print("Risposta: {0}\n", resArray.ToString());

            JArray jmembers = JArray.Parse(resArray[0].ToString());
            foreach (JObject mem in jmembers)
            {
                if (mem["ID"].ToString() == uid) { exp.ListaPartecipanti.Add(current); }
                else { exp.ListaPartecipanti.Add(ConvertiJsonInUtente(mem)); }
            }
            System.Diagnostics.Debug.Print("Partecipanti caricati\n");

            JArray jimages = JArray.Parse(resArray[1].ToString());
            foreach (JValue mem in jimages)
            {
                exp.Galleria.Add(Convert.FromBase64String(mem.ToString()));
            }
            System.Diagnostics.Debug.Print("Galleria caricata\n");

            exp.Luoghi = JsonConvert.DeserializeObject<List<string>>(resArray[2].ToString());
            exp.Slogan = JsonConvert.DeserializeObject<List<string>>(resArray[3].ToString());
            exp.Funfacts = JsonConvert.DeserializeObject<List<string>>(resArray[4].ToString());
            exp.Playlist = JsonConvert.DeserializeObject<List<string>>(resArray[5].ToString());
            exp.Recensioni = JsonConvert.DeserializeObject<List<string>>(resArray[6].ToString());
            exp.Altro = JsonConvert.DeserializeObject<List<string>>(resArray[7].ToString());
            System.Diagnostics.Debug.Print("Info rimanenti caricate\n");
        }

        public static void EliminaUtente(string uid)
        {
            string url = ConfigurationManager.AppSettings["deleteUserAPI"];
            System.Diagnostics.Debug.Print("Elimino da DB utente {0}\n", uid);

            var body = new StringContent(uid, Encoding.UTF8, "application/json");
            System.Diagnostics.Debug.Print("POST body creato\n");

            HttpClient client = new HttpClient();
            client.PostAsync(url, body);    // per maggiore velocità, non attendo che ritorni il risultato ma proseguo con l'esecuzione 
            System.Diagnostics.Debug.Print("Inviata query di eliminazione via deleteUserAPI\n");
        }

        public static async Task<bool> AggiornaUtenteInfoNonExp(Utente usr)
        {
            string url = ConfigurationManager.AppSettings["updateUserInfoNonExpAPI"];
            System.Diagnostics.Debug.Print("Aggiorno campi non-esperienziali di utente {0}\n", usr.ID);

            string jstring = ConvertiUtenteInJson(usr).ToString();
            System.Diagnostics.Debug.Print("Creato JSON da oggetto Utente: {0}\n", jstring);

            var body = new StringContent(jstring, Encoding.UTF8, "application/json");
            System.Diagnostics.Debug.Print("POST body creato\n");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url, body);
            System.Diagnostics.Debug.Print("Risposta ricevuta da updateUserInfoNonExpAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La updateUserInfoNonExpAPI non ha risposto correttamente");

            string resultString = await response.Content.ReadAsStringAsync();
            if (!Boolean.TryParse(resultString, out bool result)) throw new Exception("Errore nel parsing del risultato");
            System.Diagnostics.Debug.Print("Parsing del risultato corretto: {0}\n", result);

            return result;
        }

        public static async Task<bool> AggiornaUtentiInfoExp(List<Utente> users, string tipoExp)
        {
            string url = ConfigurationManager.AppSettings["updateUserInfoExpAPI"];
            System.Diagnostics.Debug.Print("Aggiorno campi esperienziali di utenti della nuova esperienza\n");

            JArray jbody = new JArray();
            foreach (Utente usr in users)
            {
                JObject jusr = ConvertiUtenteInJson(usr);
                int nexps = usr.ListaDistintivi[tipoExp].Item1;
                int levmax = 0;
                if (nexps >= Constants.sogliaSecondoLivello) { levmax = 2; }
                else if (nexps >= Constants.sogliaPrimoLivello) { levmax = 1; }
                jusr.Add("Distintivo", new JArray(tipoExp, levmax, nexps));
                jbody.Add(jusr);
                System.Diagnostics.Debug.Print("Creato JSON da oggetto Utente: {0}\n", jusr);
            }

            var body = new StringContent(jbody.ToString(), Encoding.UTF8, "application/json");
            System.Diagnostics.Debug.Print("POST body creato\n");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url, body);
            System.Diagnostics.Debug.Print("Risposta ricevuta da updateUserInfoExpAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La updateUserInfoExpAPI non ha risposto correttamente");

            string resultString = await response.Content.ReadAsStringAsync();
            if (!Boolean.TryParse(resultString, out bool result)) throw new Exception("Errore nel parsing del risultato");
            System.Diagnostics.Debug.Print("Parsing del risultato corretto: {0}\n", result);

            return result;
        }

    }
}
