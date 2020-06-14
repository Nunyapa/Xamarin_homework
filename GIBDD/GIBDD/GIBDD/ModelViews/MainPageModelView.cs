using GIBDD.ModelViews;
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
        private AppealModelView AppealModelView;


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainPageModelView()
        {

            getListOfProfiles();
            AppealModelView = new AppealModelView();
            App.Database.OnChangeProfilesTableRecord += EventOnChangeRecord;
            AppealModelView.SendAppealEvent += AppealSended;
            CreateProfileCommandBtn = new Command(() => CreateProfileBtn());
            RefreshCommandBtn = new Command(OnRefresh);
            UpdateProfileCommandBtn = new Command(() => UpdateProfileCommand(Profile), IsUpdatable);
            AppealCommandBtn = new Command(AppealBtnHandler, IsAppealable);
        }

        private bool IsAppealable()
        {
            return Profile != null;
        }

        private bool IsUpdatable()
        {
            return Profile != null;
        }

        private void AppealBtnHandler()
        {
            AppealModelView.CurrentProfile = Profile;
            Application.Current.MainPage.Navigation.PushAsync(new AppealPage(AppealModelView));
        }

        private void AppealSended(Object sender, EventArgs e)
        {
            AppealModelView = new AppealModelView();
            AppealModelView.SendAppealEvent += AppealSended;
        }

        private void EventOnChangeRecord(Object sender, EventArgs e)
        {
            getListOfProfiles();
        }

        private async void CreateProfileBtn()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new CreateProfilePage());
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
            _listofprofiles = await App.Database?.GetAllRecordsFromProfilesTable();
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
                UpdateProfileCommandBtn.ChangeCanExecute();
                AppealCommandBtn.ChangeCanExecute();
                OnPropertyChanged("DisplayCurProfile");
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
