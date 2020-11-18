using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BabyTech.Pages
{
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        async void OnNextButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new QPage());
        }
    }
}
