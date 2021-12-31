using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TheSocialGame.DBstuff
{
    class DBcaller
    {
        private HttpClient client;
        private HttpResponseMessage response;

        public DBcaller()
        {
            client = new HttpClient();
            response = new HttpResponseMessage();
        }

        public async Task GetAllUsers(string name, List<UserSimple> result)
        {
            string url = String.Format(ConfigurationManager.AppSettings["selectAPI"], name);
            System.Diagnostics.Debug.Print("Selecting all users with name '{0}'\n", name);

            response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Response received from API\n");
            if (!response.IsSuccessStatusCode) throw new Exception("The API failed to respond correctly");

            string jsonString = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.Print("Response: {0}\n", jsonString);
            JObject json = JObject.Parse(jsonString);
            JArray jsonUsers = (JArray)json["result"];
            System.Diagnostics.Debug.Print("Array: {0}\n", jsonUsers.ToString());

            List<UserSimple> users = JsonConvert.DeserializeObject<List<UserSimple>>(jsonUsers.ToString());
            System.Diagnostics.Debug.Print("API response converted to a user list\n");
            System.Diagnostics.Debug.Print("Number of users: {0}\n", users.Count);
            System.Diagnostics.Debug.Print("Printing the users...\n{0}\n", users.ToString());

            result.AddRange(users);
            System.Diagnostics.Debug.Print("Result stored in list parameter\n");
        }

        public async Task InsertUser(UserSimple usr)
        {
            string url = ConfigurationManager.AppSettings["selectAPI"];
            System.Diagnostics.Debug.Print("Inserting in DB user {0} {1} {2} {3} {4}\n", usr.ID, usr.Username, usr.Password, usr.PuntiSocial, usr.Livello);
            // assumiamo che, dati i costruttori di UserSimple, tutte le proprietà di usr siano inizializzate. ID verrà aggiornata correttamente all'inserimento

            string jusr = JsonConvert.SerializeObject(usr);
            var body = new StringContent(jusr, Encoding.UTF8, "application/json");  // "application/json" corretto?
            System.Diagnostics.Debug.Print("POST body created\n");

            response = await client.PostAsync(url, body);
            System.Diagnostics.Debug.Print("Response received from API\n");
            if (!response.IsSuccessStatusCode) throw new Exception("The API failed to respond correctly");

            string resultString = await response.Content.ReadAsStringAsync();
            if (!Int32.TryParse(resultString, out int newId)) System.Diagnostics.Debug.Print("Error while converting response string to int\n");
            System.Diagnostics.Debug.Print("Response string parsed correctly\n");

            usr.ID = newId;
            System.Diagnostics.Debug.Print("Returning user with right ID: {0}\n", newId);
        }
    }
}
