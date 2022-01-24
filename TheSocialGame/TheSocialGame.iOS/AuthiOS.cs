using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using TheSocialGame.iOS;
using Firebase.Auth;
using Foundation;

[assembly: Dependency(typeof(AuthiOS))]
namespace TheSocialGame.iOS
{
    public class AuthiOS : IAuth
    {
        public AuthiOS()
        {
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                System.Diagnostics.Debug.WriteLine("token: " + newUser.User.Uid);

                return await user.User.GetIdTokenAsync();
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public bool SignIn()
        {
            var user = Auth.DefaultInstance.CurrentUser;
            return user != null;
        }

        public bool SignOut()
        {
            try
            {
                _ = Auth.DefaultInstance.SignOut(out NSError error);
                return error == null;
            }catch (Exception e)
            {
                return false;
            }
        }

        public async Task<string> SignUpWithEmailAndPassword(string email, string password)
        {
            try
            {
                var newUser = await Auth.DefaultInstance.CreateUserAsync(email, password);
                System.Diagnostics.Debug.WriteLine("token: " + newUser.User.Uid);

                return await newUser.User.GetIdTokenAsync();
                
            } catch (Exception e)
            {
                return string.Empty;
            }

        }

        public bool DeleteUser(string password)
        {

           
            var us = Auth.DefaultInstance.CurrentUser;
            try
            {
                Auth.DefaultInstance.SignInWithPasswordAsync(us.Email, password);
                us.DeleteAsync();
                return true;

            }
            catch (Exception e)
            {

                return false;
            }
           
        }

        public string GetEmail()
        {
            var us = Auth.DefaultInstance.CurrentUser;
            return us.Email;
        }

        public void ChangeEmail(string mail)
        {
            var us = Auth.DefaultInstance.CurrentUser;
            us.UpdateEmailAsync(mail);
        }

        bool ValidPassword(string password)
        {
            var us = Auth.DefaultInstance.CurrentUser;
            try
            {
                Auth.DefaultInstance.SignInWithPasswordAsync(us.Email, password);
                
                return true;

            }
            catch (Exception e)
            {

                return false;
            }
        }

        bool ChangePassword(string password)
        {
            var us = Auth.DefaultInstance.CurrentUser;
            try
            {
                us.UpdatePasswordAsync(password);
                return true
            } catch(Exception e)
            {
                return false;
            }
    }
}
