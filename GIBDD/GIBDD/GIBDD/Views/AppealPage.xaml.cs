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
        public AppealPage(AppealModelView context)
        {
            InitializeComponent();
            Title = "Appeal";
            BindingContext = context;
        }
    }
}