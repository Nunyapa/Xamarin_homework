using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GIBDD
{
    public partial class App : Application
    {

        static DatabaseData database;
        public static DatabaseData Database
        {
            get
            {
                if (database == null)
                {
                    database = new DatabaseData();
                }
                return database;
            }
        }

        static public IPhotoPlatform platform;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
