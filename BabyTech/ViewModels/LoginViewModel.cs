using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Text;
using Xamarin.Forms;

namespace BabyTech.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidLoginPrompt;
        public Action DisplayCorrectLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string email;
        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public ICommand SubmitCommand
        {
            protected set; get;
        }

        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }

        public void OnSubmit() 
        {
            if (email != "choe@gwu.edu" || password != "capstone")
            {
                DisplayInvalidLoginPrompt();
            } 
            else
            {
                DisplayCorrectLoginPrompt();
            }
        }
    }
}
