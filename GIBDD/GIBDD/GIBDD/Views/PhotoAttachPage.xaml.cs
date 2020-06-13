using GIBDD.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GIBDD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoAttachPage : ContentPage
    {
        public PhotoAttachPage(AppealModelView context)
        {
            InitializeComponent();
            Title = "Attach photo";
            BindingContext = context;
        }
    }
}