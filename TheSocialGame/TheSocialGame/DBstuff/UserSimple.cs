using System;
using System.Collections.Generic;
using System.Text;

namespace TheSocialGame.DBstuff
{
    class UserSimple
    {
        public UserSimple(int id, string name, string password, int puntiSocial, int livello) : this(name, password, puntiSocial, livello)
        {
            ID = id;
        }

        public UserSimple(string name, string password, int puntiSocial, int livello) : this(name, password)
        {
            PuntiSocial = puntiSocial;
            Livello = livello;
        }

        public UserSimple(string name, string password)
        {
            ID = -1;
            Username = name;
            Password = password;
            PuntiSocial = 0;
            Livello = 0;
        }

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int PuntiSocial { get; set; }
        public int Livello { get; set; }
    }
}
