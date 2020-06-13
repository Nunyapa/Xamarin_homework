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
        public Command UpdateProfileCommandBtn { get; set; }
        public Command AppealCommandBtn { get; set; }


        public MainPageModelView()
        {
            getListOfProfiles();
            App.Database.OnChangeRecord += EventOnChangeRecord;
            CreateProfileCommandBtn = new Command(() => CreateProfileBtn());
            RefreshCommandBtn = new Command(OnRefresh);
            UpdateProfileCommandBtn = new Command(() => UpdateProfileCommand(Profile), () => IsUpdatable);
            AppealCommandBtn = new Command(AppealBtnHandler);

        }

        private void AppealBtnHandler()
        {
            Application.Current.MainPage.Navigation.PushAsync(new AppealPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void EventOnChangeRecord(Object sender, EventArgs e)
        {
            getListOfProfiles();
        }

        private void CreateProfileBtn()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new CreateProfilePage());
            Profile = null;
        }

        private void UpdateProfileCommand(ProfilesTable prof)
        {
            if (prof != null)
                Application.Current.MainPage.Navigation.PushModalAsync(new CreateProfilePage(prof));
            Profile = null;
        }

        async void getListOfProfiles()
        {
            _listofprofiles = await App.Database?.GetAllRecords();
            OnPropertyChanged("ListOfProfiles");
        }

        void OnRefresh()
        {
            getListOfProfiles();
            IsRefreshing = false;
            Profile = null;
            OnPropertyChanged("IsRefreshing");
        }

        public string DisplayCurProfile
        {
            get 
            {
                if (Profile == null)
                    return "";
                return $"{Profile.Sername} {Profile.Name} {Profile.MiddleName}";
            }
        }

        private List<ProfilesTable> _listofprofiles;
        public List<ProfilesTable> ListOfProfiles
        {
            get { return _listofprofiles; }
        }

        private ProfilesTable _profile;
        public ProfilesTable Profile
        {
            get { return _profile; }
            set
            {
                if (_profile != value)
                {
                    _profile = value;
                }
                if (value == null)
                    IsUpdatable = false;
                else
                    IsUpdatable = true;
                OnPropertyChanged("DisplayCurProfile");
            }
        }

        private bool _isUpdatable;
        public bool IsUpdatable
        {
            get {return _isUpdatable; }
            set
            {
                _isUpdatable = value;
                if (value != false)
                    UpdateProfileCommandBtn.ChangeCanExecute();
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


    }
}
