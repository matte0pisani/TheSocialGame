using System;
using System.Threading.Tasks;

namespace TheSocialGame
{
    public interface IAuth
    {
        
            Task<string> LoginWithEmailAndPassword(string email, string password);

            Task<string> SignUpWithEmailAndPassword(string email, string password);

            bool SignOut();
            bool SignIn();

     

    }
}
