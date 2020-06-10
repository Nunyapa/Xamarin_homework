using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GIBDD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profilepage : ContentPage
    {
            
        public Profilepage()
        {
            InitializeComponent();
        }

        private void fEntry_Completed(object sender, EventArgs e)
        {

        }
        private void iEntry_Completed(object sender, EventArgs e)
        {

        }
        private void oEntry_Completed(object sender, EventArgs e)
        {

        }

        private void EmailEntry_Completed(object sender, EventArgs e)
        {

        }

        private void PhoneEntry_Completed(object sender, EventArgs e)
        {

        }

        private void SubsectionRegionEntry_Completed(object sender, EventArgs e)
        {

        }

        private void SubsectionEntry_Completed(object sender, EventArgs e)
        {

        }

        private void IncidentRegionEntry_Completed(object sender, EventArgs e)
        {

        }

        private async void SaveBtn_Pressed(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void CancelBtn_Pressed(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}