using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using BabyTech.Pages;

namespace BabyTech
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
         
        private async void LoginButton_OnClicked(object sender, EventArgs e) => await Navigation.PushAsync(new LoginPage());

        private async void SignUpButton_OnClicked(object sender, EventArgs e) => await Navigation.PushAsync(new SignUpPage());
    }
}
