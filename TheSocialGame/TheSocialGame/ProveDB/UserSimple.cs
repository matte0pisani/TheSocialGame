using Newtonsoft.Json;
using Xamarin.Forms.Internals;

namespace TheSocialGame.ProveDB
{
    class UserSimple
    {
        [Preserve]
        [JsonConstructor]
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

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            UserSimple that = (UserSimple)obj;
            if (this.ID == that.ID && this.Username == that.Username && this.Password == that.Password 
                && this.PuntiSocial == that.PuntiSocial && this.Livello == that.Livello)
                return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() + 17 * ID + Username.GetHashCode() + Password.GetHashCode() + PuntiSocial + Livello;
        }
    }
}
