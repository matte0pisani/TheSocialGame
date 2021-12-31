using System;
using System.Collections.Generic;
using System.Text;

namespace TheSocialGame.DBstuff
{
    class UserSimple
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int PuntiSocial { get; set; }
        public int Livello { get; set; }
    }
}
