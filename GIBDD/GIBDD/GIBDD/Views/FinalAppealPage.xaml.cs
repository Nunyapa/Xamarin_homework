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
    public partial class FinalAppealPage : ContentPage
    {
        public FinalAppealPage(AppealModelView context)
        {
            InitializeComponent();
            BindingContext = context;
            //document.getElementsByName("agree")[0].click()
            //document.getElementsByClassName("u-form__sbt")[0].click()
        }
    }
}