using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace GIBDD
{
    class SubsectionPickerModelView : INotifyPropertyChanged
    {

        const int UPDATE_MODE = 0;
        const int SAVE_MODE = 1;
        int mode;
        public Command SaveBtnCommandHandler { get; private set; }
        public Command CancelBtnCommandHandler { get; private set; }
        public Command DeleteBtnCommandHandler { get; private set; }
        public List<string> Regions { get; set; }
        public List<string> RegionsOfIncident { get; set; }

        private readonly FilesData fd = new FilesData();
        private ProfileValidator profileValidator = new ProfileValidator();
        private Dictionary<int, List<string>> _divDict = new Dictionary<int, List<string>>();
        private ProfilesTable ProfileToUpdate;

        public SubsectionPickerModelView() 
        {
            Initialize();
            mode = SAVE_MODE;
        }

        public SubsectionPickerModelView(ProfilesTable profile)
        {
            Initialize();
            SetFieldsFromExistProfile(profile);
            ProfileToUpdate = profile;
            mode = UPDATE_MODE;
        }


        private void Initialize()
        {
            Regions = fd.Regions.Values.ToList<string>();
            RegionsOfIncident = Regions;
            _divDict = fd.Divisions;
            SaveBtnCommandHandler = new Command(() => BtnHandler());
            CancelBtnCommandHandler = new Command(CancelBtnHandler);
            DeleteBtnCommandHandler = new Command(() => DeleteBtnHandler());
        }

        async private void DeleteBtnHandler()
        {
            string warnMessage = $"Press Ok to delete {ProfileToUpdate.Name} {ProfileToUpdate.Sername} profile";
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete", warnMessage, "OK", "NO");
            if (answer)
            {
                App.Database.DeleteRecordFromProfilesTable(ProfileToUpdate.Id);
                await Application.Current.MainPage.Navigation.PopModalAsync(true);
            }

        }

        private void SetFieldsFromExistProfile(ProfilesTable profile)
        {
            
            IsHuman = profile.TypeOfProfile;
            SerName = profile.Sername;
            Name = profile.Name;
            MiddleName = profile.MiddleName;
            Email = profile.Email;
            PhoneNumber = profile.Phone;
            SelectedRegion = profile.SelectedRegion;
            SelectedDiv = profile.SelectedDiv;
            SelectedRegionOfIncident = profile.SelectedRegionOfIncident;
            OrgOptionalInformation = profile.OrgOptionalInformation;
            OrgName = profile.OrgName;
            OutNumber = profile.OutNumber;
            RegistrOrgDate = profile.RegistrOrgDate;
            NumberLetter = profile.NumberLetter;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        async void BtnHandler()
        {
            ProfilesTable newProfile = new ProfilesTable()
            {
                TypeOfProfile = IsHuman,
                Sername = SerName,
                Name = Name,
                MiddleName = MiddleName,
                Email = Email,
                Phone = PhoneNumber,
                SelectedRegion = SelectedRegion,
                SelectedDiv = SelectedDiv,
                SelectedRegionOfIncident = SelectedRegionOfIncident,
                OrgOptionalInformation = OrgOptionalInformation,
                OrgName = OrgName,
                OutNumber = OutNumber,
                RegistrOrgDate = RegistrOrgDate,
                NumberLetter = NumberLetter
            };

            //Validation
            if (profileValidator.Validate(newProfile).IsValid)
            {
                switch (mode)
                {
                    case SAVE_MODE:
                        App.Database?.AddRecordToProfilesTable(newProfile);
                        break;
                    case UPDATE_MODE:
                        newProfile.Id = ProfileToUpdate.Id;
                        App.Database?.UpdateRecord(newProfile);
                        break;
                }
                await Application.Current.MainPage.Navigation.PopModalAsync(true);
            }
            else 
            {
                string warnMessage = "";
                foreach (var err in profileValidator.Validate(newProfile).Errors) {
                    warnMessage += err + "\n";
                }
                await Application.Current.MainPage.DisplayAlert("Non-valid", warnMessage, "Ok");
            }
        }


        async void CancelBtnHandler()
        {
            string warnMessage;
            if (mode == SAVE_MODE)
                warnMessage = "You will lose all infromation";
            else
                warnMessage = "Do not accept changes?";
            bool answer = await Application.Current.MainPage.DisplayAlert("Warning", warnMessage, "OK", "STAY");
            if (answer)
                await Application.Current.MainPage.Navigation.PopModalAsync();
        }


        public string BtnTitle
        {
            get {
                if (mode == SAVE_MODE)
                    return "Save";
                else
                    return "Update";
            }
        }

        public bool IsDeleteBtnOn
        {
            get
            {
                if (mode == UPDATE_MODE)
                    return true;
                return false;
            }
        }


        private string _selectedregion;
        public string SelectedRegion 
        {
            get { return _selectedregion; }
            set 
            {
                if (value != _selectedregion)
                {
                    _selectedregion = value;
                    SelectedDivisions = _divDict[int.Parse(value.Substring(0, 2))];
                    _selecteddiv = "";
                    SelectedRegionOfIncident = value;
                    //OnPropertyChanged(SelectedRegionOfIncident);
                }
            }
        }



        private List<string> _selecteddivisions;
        public List<string> SelectedDivisions
        {
            get { return _selecteddivisions; }
            set
            {
                if (value != _selecteddivisions)
                {
                    _selecteddivisions = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selecteddiv;
        public string SelectedDiv
        {
            get { return _selecteddiv; }
            set
            {
                if (value != _selecteddiv)
                {
                    _selecteddiv = value;
                }
            }
        }

        private string _selectedregionofincident;
        public string SelectedRegionOfIncident
        {
            get { return _selectedregionofincident; }
            set 
            {
                _selectedregionofincident = value;
                OnPropertyChanged();
            }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                }
            }
        }
        private string _sername = "";
        public string SerName
        {
            get { return _sername; }
            set
            {
                if (value != _sername)
                {
                    _sername = value;
                }
            }
        }
        private string _middlename = "";
        public string MiddleName
        {
            get { return _middlename; }
            set
            {
                if (value != _middlename)
                {
                    _middlename = value;
                }
            }
        }

        private string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                if (value != _email)
                {
                    _email = value;
                }
            }
        }
        private string _phonenumber = "";
        public string PhoneNumber
        {
            get { return _phonenumber; }
            set
            {
                if (value != _phonenumber)
                {
                    _phonenumber = value;
                }
            }
        }

        public string _currenttype;
        public string CurrentType
        {
            get { return _currenttype; }
            set
            {
                if (_currenttype != value)
                {
                    _currenttype = value;
                    if (value == "0")
                        IsHuman = true;
                    else if (value == "1")
                        IsHuman = false;
                }
            }
        }

        public List<string> _typesofprofile = new List<string>() { "Гражданин", "Организация" };
        public List<string> TypesOfProfile
        {
            get { return _typesofprofile; }
        }

        private bool _isHuman;
        public bool IsHuman
        {
            get { return _isHuman; }
            set 
            {
                if (value == true)
                {
                    OrgOptionalInformation = "";
                    OrgName = "";
                    OutNumber = "";
                    RegistrOrgDate = DateTime.Today;
                    NumberLetter = "";
                }
                _isHuman = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOrganization));
            }

        }

        public bool IsOrganization
        {
            get 
            {
                return !_isHuman; 
            }
        }

        //OrgNameEntry
        private string _orgname;
        public string OrgName
        {
            get { return _orgname; }
            set { _orgname = value; }
        }

        //OrgOptionalInformationEntry
        private string _orgoptionalinformation;
        public string OrgOptionalInformation
        {
            get { return _orgoptionalinformation; }
            set { _orgoptionalinformation = value; }
        }

        //OutNumberEntry
        private string _outnumber;
        public string OutNumber
        {
            get { return _outnumber; }
            set { _outnumber = value; }
        }
        //RegistrOrgDate
        private DateTime _registrorgdate = DateTime.Today;
        public DateTime RegistrOrgDate
        {
            get { return _registrorgdate; }
            set { _registrorgdate = value; }
        }

        //NumberLetterEntry
        private string _numberletter;
        public string NumberLetter
        {
            get { return _numberletter; }
            set { _numberletter = value; }
        }


    }
}
