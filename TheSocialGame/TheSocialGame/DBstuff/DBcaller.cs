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

        /** 
         * Adds all the users in the db with a given @name inside the list @result
         */
        public async Task GetAllUsers(string name, List<UserSimple> result)
        {
            string url = String.Format(ConfigurationManager.AppSettings["selectAPI"], name);
            System.Diagnostics.Debug.Print("Selecting all users with name '{0}'\n", name);

            response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Response received from selectAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("The selectAPI failed to respond correctly");

            string jsonString = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.Print("Response: {0}\n", jsonString);
            JObject json = JObject.Parse(jsonString);
            JArray jsonUsers = (JArray)json["result"];
            System.Diagnostics.Debug.Print("Array: {0}\n", jsonUsers.ToString());

            List<UserSimple> users = JsonConvert.DeserializeObject<List<UserSimple>>(jsonUsers.ToString());
            System.Diagnostics.Debug.Print("selectAPI response converted to a user list\n");
            System.Diagnostics.Debug.Print("Number of users: {0}\n", users.Count);

            result.AddRange(users);
            System.Diagnostics.Debug.Print("Result stored in list parameter\n");
        }
        
        /**
         * Inserts the user @usr inside the db, modifying @usr 's ID field according to his ID in the db
         */
        public async Task InsertUser(UserSimple usr)
        {
            string url = ConfigurationManager.AppSettings["insertAPI"];
            System.Diagnostics.Debug.Print("Inserting in DB user {0} {1} {2} {3} {4}\n", usr.ID, usr.Username, usr.Password, usr.PuntiSocial, usr.Livello);
            // assumiamo che, dati i costruttori di UserSimple, tutte le proprietà di usr siano inizializzate. ID verrà aggiornata correttamente all'inserimento

            string jusr = JsonConvert.SerializeObject(usr);
            System.Diagnostics.Debug.Print("JSON created from method input: {0}\n", jusr);

            var body = new StringContent(jusr, Encoding.UTF8, "application/json");
            System.Diagnostics.Debug.Print("POST body created\n");

            response = await client.PostAsync(url, body);
            System.Diagnostics.Debug.Print("Response received from insertAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("The insertAPI failed to respond correctly");

            string resultString = await response.Content.ReadAsStringAsync();
            if (!Int32.TryParse(resultString, out int newId)) System.Diagnostics.Debug.Print("Error while converting response string to int\n");
            System.Diagnostics.Debug.Print("Response string parsed correctly\n");

            usr.ID = newId;
            System.Diagnostics.Debug.Print("Returning user with right ID: {0}\n", newId);
        }

        /**
         * Removes all the users in the db with a given @name and saves the deleted users in @deleted. Returns the number of users deleted
         */
        public async Task<int> DeleteAllUsers(string name, List<UserSimple> deleted)
        {
            string url = String.Format(ConfigurationManager.AppSettings["deleteAPI"], name);
            System.Diagnostics.Debug.Print("Deleting all users with name '{0}'\n", name);

            response = await client.GetAsync(url);
            System.Diagnostics.Debug.Print("Response received from deleteAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("The deleteAPI failed to respond correctly");

            string jsonString = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.Print("Response: {0}\n", jsonString);
            JObject json = JObject.Parse(jsonString);
            JArray deletedUsers = (JArray)json["result"];
            System.Diagnostics.Debug.Print("Deleted users array: {0}\n", deletedUsers.ToString());

            List<UserSimple> users = JsonConvert.DeserializeObject<List<UserSimple>>(deletedUsers.ToString());
            System.Diagnostics.Debug.Print("Deleted users array converted to a user list\n");
            System.Diagnostics.Debug.Print("Checking correct number of deleted users: {0}={1}\n", (int)json["number"], users.Count);

            deleted.AddRange(users);
            System.Diagnostics.Debug.Print("Result stored in list parameter\n");

            return (int)json["number"]; // metodi asincroni possono ritornare valori, se il metodo chiamante ne attende l'esecuzione con un await
        }

        /**
         * Updates the user in the db with @usr 's ID: his new Username, Password, PuntiSocial and Livello are those of @usr.
         * Return true if the update is successfull, false otherwise
         */
        public async Task<bool> UpdateUser(UserSimple usr)
        {
            string url = ConfigurationManager.AppSettings["updateAPI"];
            System.Diagnostics.Debug.Print("Updating in DB user {0} {1} {2} {3} {4}\n", usr.ID, usr.Username, usr.Password, usr.PuntiSocial, usr.Livello);

            string jusr = JsonConvert.SerializeObject(usr);
            System.Diagnostics.Debug.Print("JSON created from method input: {0}\n", jusr);

            var body = new StringContent(jusr, Encoding.UTF8, "application/json");
            System.Diagnostics.Debug.Print("POST body created\n");

            response = await client.PostAsync(url, body);
            System.Diagnostics.Debug.Print("Response received from updateAPI\n");
            if (!response.IsSuccessStatusCode) throw new Exception("The updateAPI failed to respond correctly");

            string resultString = await response.Content.ReadAsStringAsync();
            UserSimple result = JsonConvert.DeserializeObject<UserSimple>(resultString);
            return result.Equals(usr);
            // non trovato modo per far tornare dalla chiamata a tsg-db-update-api il vecchio utente
            // serve implementare una lambda che faccia select nel db in base all'id e non al nome
        }
    }
}
