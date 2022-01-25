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

            bool DeleteUser(string password);

            string GetEmail();

            void ChangeEmail(string mail);

            bool ValidPassword(string password);

            bool ChangePassword(string password);

            bool MailVerificata();

            bool PasswordDimenticata(string mail);


    }

   
    
}
