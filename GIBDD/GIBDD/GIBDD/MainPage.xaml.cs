using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GIBDD
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            Title = "Choose profile";
            
            ListViewProfiles.ItemsSource = new string[] { "one", "two", "three"};
        }

        private async void CreateProfileBtn_Pressed(object sender, EventArgs e)
        {
            
            await Navigation.PushModalAsync(new NavigationPage(new CreateProfilePage()));
            
        }
    }
}
