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
        public static async Task<bool> InserisciUtente(Utente usr)
        {
            string url = ConfigurationManager.AppSettings["insertUserAPI"];
            System.Diagnostics.Debug.Print("Inserisco in DB utente {0} {1}\n", usr.ID, usr.Username);

            string jstring = JsonConvert.SerializeObject(usr);
            System.Diagnostics.Debug.Print("Creato JSON da oggetto Utente: {0}\n", jstring);

            JObject json = JObject.Parse(jstring);
            json.Remove("ListaDistintivi");
            json.Remove("Esperienze");
            json.Remove("Amici");
            json["Sfondo"] = usr.Sfondo.ToHex();
            json["Primario"] = usr.Primario.ToHex();
            json["Secondario"] = usr.Secondario.ToHex();
            if (usr.FotoBytes != null)
            {
                json["FotoBytes"] = Convert.ToBase64String(usr.FotoBytes);
            }
            jstring = json.ToString();
            System.Diagnostics.Debug.Print("JSON modificato: {0}\n", jstring);

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

        /**
        * Per ora inizializza soltanto le proprietà "semplici" dell'utente e i distintivi, quindi no amici/esperienze
        */
        public static async Task<Utente> GetUtente(string userID)
        {
            string url = string.Format(ConfigurationManager.AppSettings["selectUserAPI"], userID);
            System.Diagnostics.Debug.Print("Prendo l'utente con ID: '{0}'\n", userID);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Risposta ricevuta da selectUserAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("La selectUserAPI non ha risposto correttamente");

            string jsonString = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.Print("Risposta: {0}\n", jsonString);

            JObject jres = JObject.Parse(jsonString);
            Utente res = new Utente
            {
                ID = jres["ID"].ToString(),
                Username = jres["Username"].ToString(),
                PuntiSocial = (int)jres["PuntiSocial"],
                PuntiEsperienza = (int)jres["PuntiEsperienza"],
                Livello = (int)jres["Livello"],
                FotoBytes = Convert.FromBase64String(jres["FotoBytes"].ToString()),
                FotoLiveiOS = (bool)jres["FotoLiveiOS"],
                Personalita1 = (int)jres["Personalita1"],
                Personalita2 = (int)jres["Personalita2"],
                Personalita3 = (int)jres["Personalita3"],
                Personalita4 = (int)jres["Personalita4"],
                Personalita5 = (int)jres["Personalita5"],
                Sfondo = Color.FromHex(jres["Sfondo"].ToString()),
                Primario = Color.FromHex(jres["Primario"].ToString()),
                Secondario = Color.FromHex(jres["Secondario"].ToString()),
                Privato = (bool)jres["Privato"]
            };
            System.Diagnostics.Debug.Print("Utente creato\n");

            if(await AggiungiDistintivi(res)) { System.Diagnostics.Debug.Print("Caricamento distintivi completato\n"); }
            else { System.Diagnostics.Debug.Print("Errore nel caricamento dei distintivi\n"); }

            return res;
        }

        private static async Task<bool> AggiungiDistintivi(Utente usr)
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
                for(int i = 1; i <= Utente.maxLivDistintivo; i++)
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
    }
}
