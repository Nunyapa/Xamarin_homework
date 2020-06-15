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
    public partial class ImageViewer : ContentPage
    {
        public ImageViewer(AppealModelView context)
        {
            InitializeComponent();
            Title = "Image Viewer";
            BindingContext = context;
        }
    }
}