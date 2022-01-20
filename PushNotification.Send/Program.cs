using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace PushNotification.Send
{
    class Program
    {
         static void Main(string[] args)
        {

           

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("private_key.json")
            });

            //This registration token comes from the client FCM SKDs.
           // var registrationToken = "ADD_TOKEN";
            //See documentation on defining a massage payload
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
            { 
                     { "myData", "1337"},
            },
                   // Token = registrationToken,
                   Topic = "nuovaEsperienza",
                    Notification = new Notification()
        {            Title = "NUOVA ESPERIENZA",
                     Body = "Qualcuno ti ha appena aggiunto in un'esperienza, corri a vederla!"
                     }
    };

            //Send a message to the device corresponding to the provided registration token
            string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
    //Response is a message ID string
    Console.WriteLine("Successfully sent message: " + response);


        }
    }
}
