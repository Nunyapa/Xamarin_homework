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

        public Command SaveBtnCommandHandler { get; private set; }
        public Command CancelBtnCommandHandler { get; private set; }
        public List<string> Regions { get; set; }
        public List<string> RegionsOfIncident { get; set; }

        private readonly FilesData fd = new FilesData();
        private ProfileValidator profileValidator = new ProfileValidator();
        private Dictionary<int, List<string>> _divDict = new Dictionary<int, List<string>>();


        public SubsectionPickerModelView() 
        {
            Regions = fd.Regions.Values.ToList<string>();
            RegionsOfIncident = Regions;
            _divDict = fd.Divisions;
            SaveBtnCommandHandler = new Command(() => SaveBtnHandler());
            CancelBtnCommandHandler = new Command(CancelBtnHandler);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event EventHandler<ModalPoppedEventArgs> onPopped;
        protected void OnPoppedChanged(Page page)
        {
            onPopped?.Invoke(this, new ModalPoppedEventArgs(page));
        }


        static public event EventHandler OnSave;

        async void SaveBtnHandler()
        {
            Profile newProfile = new Profile()
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
                App.Database?.AddRecord(newProfile);
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
            string warnMessage = "You going to lose all infromation in the fields";
            bool answer = await Application.Current.MainPage.DisplayAlert("Are you sure?", warnMessage, "leave", "stay");
            if (answer)
                await Application.Current.MainPage.Navigation.PopModalAsync();
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


        private bool _isHuman = true;
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
