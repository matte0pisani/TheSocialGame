using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TheSocialGame
{
    class LoginPageViewModel: INotifyPropertyChanged
    {
        public LoginPageViewModel()
        {
            this.isCredentialsInserted = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isCredentialsInserted;
        public bool IsCredentialsInserted
        {
            get
            {
                if (Password == null || Username == null) return false;
                return Password.Length > 0 && Username.Length > 0;
            }
            set
            {
                isCredentialsInserted = value;
                var args = new PropertyChangedEventArgs(nameof(isCredentialsInserted));
                PropertyChanged?.Invoke(this, args);
            }

        }


        public string Username { get; set; }

        public string Password { get; set; }

    }
}
