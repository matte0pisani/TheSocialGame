using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Auth;
using TheSocialGame.Droid;

[assembly : Dependency(typeof(AuthDroid))]
namespace TheSocialGame.Droid
{
    public class AuthDroid : IAuth
    {
        public AuthDroid()
        {
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await Firebase.Auth.FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
       //       var token = user.User.GetIdToken(false);
                System.Diagnostics.Debug.WriteLine("token: " + user.User.Uid);
                return user.User.Uid;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public bool SignIn()
        {
            var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public bool SignOut()
        {
            try
            {
                Firebase.Auth.FirebaseAuth.Instance.SignOut();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<string> SignUpWithEmailAndPassword(string email, string password)
        {
            try
            {
                var newUser = await Firebase.Auth.FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = newUser.User.GetIdToken(false);
                System.Diagnostics.Debug.WriteLine("userID: " + newUser.User.Uid);
                System.Diagnostics.Debug.WriteLine("token: " + token);
                newUser.User.SendEmailVerification();
                return newUser.User.Uid;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public bool DeleteUser(string password)
        {


            FirebaseUser us = FirebaseAuth.Instance.CurrentUser;
            try
            {
                Firebase.Auth.FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(us.Email, password); ;
                us.DeleteAsync();
                return true;

            }
            catch (FirebaseAuthInvalidUserException e)
            {

                return false;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {

                return false;
            }
        }

        public string GetEmail()
        {
            FirebaseUser us = FirebaseAuth.Instance.CurrentUser;
            return us.Email;
        }

        public void ChangeEmail(string mail)
        {
            FirebaseUser us = FirebaseAuth.Instance.CurrentUser;
            us.UpdateEmailAsync(mail);
            us.SendEmailVerification();
          
        }

        public bool ValidPassword(string password)
        {
            FirebaseUser us = FirebaseAuth.Instance.CurrentUser;
            try
            {
                Firebase.Auth.FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(us.Email, password); ;


                return true;

            }
            catch (FirebaseAuthInvalidUserException e)
            {

                return false;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {

                return false;
            }
        }

       public bool ChangePassword(string password)
        {
            FirebaseUser us = FirebaseAuth.Instance.CurrentUser;
            try
            {
                us.UpdatePasswordAsync(password);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool MailVerificata()
        {
            return FirebaseAuth.Instance.CurrentUser.IsEmailVerified;
        }

        public bool PasswordDimenticata(string mail)
        {
            try
            {
                FirebaseAuth.Instance.SendPasswordResetEmail(mail);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
