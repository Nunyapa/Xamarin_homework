using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace GIBDD
{
    class MainPageModelView : INotifyPropertyChanged
    {
        public Command CreateProfileCommandBtn { get; set; }
        public Command RefreshCommandBtn { get; set; }
        public MainPageModelView()
        {
            getListOfProfiles();
            CreateProfileCommandBtn = new Command(() => CreateProfileBtn());
            RefreshCommandBtn = new Command(OnRefresh);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private void CreateProfileBtn()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new CreateProfilePage());
        }

        async void getListOfProfiles()
        {
            _listofprofiles = await App.Database?.GetAllRecords();
            string temp;
            _listofprofstrings = new List<string>();
            foreach (var prof in ListOfProfiles)
            {
                temp = $"{prof.Name} {prof.Sername} {prof.SelectedRegion} {prof.SelectedDiv}";
                _listofprofstrings.Add(temp);
            }
            OnPropertyChanged("ListOfProfStrings");
        }

        void OnRefresh()
        {
            getListOfProfiles();
            IsRefreshing = false;
            OnPropertyChanged("IsRefreshing");
            SelectedProfile = "";
        }

        private List<ProfilesTable> _listofprofiles;
        public List<ProfilesTable> ListOfProfiles
        {
            get { return _listofprofiles; }
        }

        private List<string> _listofprofstrings;
        public List<string> ListOfProfStrings
        {
            get
            {
                return _listofprofstrings;
            }
        }

        private bool _isrefreshing = false;
        public bool IsRefreshing
        {
            get
            {
                return _isrefreshing;
            }
            set
            {
                _isrefreshing = value;
            }
        }

        private int _selectedprofileindex;
        private string _selectedprofile;
        public string SelectedProfile
        {
            get
            {
                return _selectedprofile;
            }
            set
            {
                if (ListOfProfStrings.Contains(value))
                {
                    _selectedprofile = value;
                    _selectedprofileindex = ListOfProfStrings.IndexOf(value);
                }
                else
                {
                    _selectedprofile = "";
                    _selectedprofileindex = -1;
                }
            }
        }


    }
}
