using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLAppConfig;

namespace TheSocialGame
{
    public static class DBmanager
    {
        static async public Task<bool> InserisciNuovoUtente(Utente usr)
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
            if(usr.FotoBytes != null)
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
            if (!response.IsSuccessStatusCode) throw new Exception("L'API non ha risposto correttamente");

            string resultString = await response.Content.ReadAsStringAsync();
            if (!Boolean.TryParse(resultString, out bool result)) throw new Exception("Errore nel parsing del risultato");
            System.Diagnostics.Debug.Print("Parsing del risultato corretto\n");

            return result;
        }
    }
}
