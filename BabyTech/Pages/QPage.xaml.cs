using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BabyTech.Pages
{
    public partial class QPage : ContentPage
    {
        public QPage()
        {
            InitializeComponent();
        }

        private async void RegisterCase_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new InfoPage());
    }
}
