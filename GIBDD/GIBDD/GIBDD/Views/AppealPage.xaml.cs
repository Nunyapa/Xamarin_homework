using GIBDD.ModelViews;
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
    public partial class AppealPage : ContentPage
    {
        public AppealPage()
        {
            InitializeComponent();
            Title = "Appeal";
            BindingContext = new AppealModelView();
        }

        public AppealPage(ProfilesTable prof)
        {
            InitializeComponent();
            Title = "Appeal";
            BindingContext = new AppealModelView(prof);
        }
    }
}