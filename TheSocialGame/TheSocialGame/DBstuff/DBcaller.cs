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

        private async Task<HttpResponseMessage> Call(string query)
        {
            return await client.GetAsync(query);
        }

        public async Task GetAllUsers(string name, List<UserSimple> result)
        {
            string query = String.Format(ConfigurationManager.AppSettings["selectAPI"], name);
            System.Diagnostics.Debug.Print("Selecting all users with name '{0}'\n", name);

            response = await client.GetAsync(query);
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
    }
}
