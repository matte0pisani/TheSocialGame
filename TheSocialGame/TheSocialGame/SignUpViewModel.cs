using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TheSocialGame
{
    public class SignUpViewModel : INotifyPropertyChanged

    {
        public SignUpViewModel()
        {
            email = string.Empty;
            username = string.Empty;
            password = string.Empty;
            confermaPassword = string.Empty;
            errorLabel = string.Empty;
            isInfoValid = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string username;

        public string Username
        {
            get => username;

            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged("Username");
                    CheckValid();
                }
            }
        }

        private string email;

        public string Email
        {
            get => email;

            set
            {
                if (email != value)     //non so come ma evita alcuni errori
                {
                    email = value;
                    OnPropertyChanged("Email");
                    CheckValid();
                }
            }
        }

        private string password;

        public string Password
        {
            get => password;

            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged("Password");
                    CheckValid();
                }
            }
        }

        private string confermaPassword;

        public string ConfermaPassword
        {
            get => confermaPassword;

            set
            {
                if (confermaPassword != value)
                {
                    confermaPassword = value;
                    OnPropertyChanged("ConfermaPassword");
                    CheckValid();
                }
            }
        }

        private string errorLabel;

        public string ErrorLabel
        {
            get => errorLabel;

            set
            {
                if (errorLabel != value)
                {
                    errorLabel = value;
                    OnPropertyChanged("ErrorLabel");
                }
            }
        }

        private bool isInfoValid;

        public bool IsInfoValid
        {
            get => isInfoValid;

            set
            {
                if (isInfoValid != value)
                {
                    isInfoValid = value;
                    OnPropertyChanged("IsInfoValid");
                }
            }
        }

        private void CheckValid()
        {
            if (UsernameNotValid())
            {
                ErrorLabel = "Username non valido";
                IsInfoValid = false;
            }
            else if (EmailNotValid())
            {
                ErrorLabel = "Email non valida";
                IsInfoValid = false;
            }
            else if (PasswordNotValid())
            {
                ErrorLabel = "Le password non coincidono";
                IsInfoValid = false;
            }
            else
            {
                ErrorLabel = string.Empty;
                IsInfoValid = true;
            }


        }

        private bool UsernameNotValid()
        {
            return username.Length <= 2;
        }

        private bool EmailNotValid()
        {
            return email.Length <= 5;
        }

        private bool PasswordNotValid()
        {
            return password == string.Empty || !password.Equals(confermaPassword);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
