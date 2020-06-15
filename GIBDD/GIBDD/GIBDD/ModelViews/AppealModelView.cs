using GIBDD.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace GIBDD.ModelViews
{
    public class AppealModelView : INotifyPropertyChanged
    {

        private IPhotoPlatform platform = App.platform;
        public Command TakePhotoCommandBtn { get; set; }
        public Command ChoosePhotoCommandBtn { get; set; }
        public Command GoToAttachScreen { get; set; }
        public Command GoToImageViewer { get; set; }
        public Command DeleteCurrentImage { get; set; }
        public Command AppealBtn { get; set; }
        public Command SendAppeal { get; set; }
        static public ProfilesTable CurrentProfile { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event EventHandler SendAppealEvent;

        protected void OnSendAppealEvent()
        {
            SendAppealEvent?.Invoke(this, EventArgs.Empty);
        }


        //CLASS TO EASY GET IMAGESOURCE INSIDE A VIEW
        public class Img
        {
            public ImageSource imageSource { get; set; }
            public Img(ImageSource input)
            {
                this.imageSource = input;
            }
        }

        public AppealModelView()
        {
            TakePhotoCommandBtn = new Command(platform.TakePhoto, platform.IsCameraAvaliable);
            ChoosePhotoCommandBtn = new Command(platform.ChoosePhoto);
            GoToAttachScreen = new Command(() => AttachScreen(), AllowToAttachPhoto);
            GoToImageViewer = new Command(() => ImageViewer());
            DeleteCurrentImage = new Command(DeleteImage);
            AppealBtn = new Command(() => Appeal(), AllowToAppeal);
            SendAppeal = new Command(SendAppealHandler, AllowToAppeal);
            platform.TakePhotoCallBack = PhotoTaken;
            //App.Database.OnChangeAppealsTableRecord += EventHandlerAppealsTable;
            ListOfAppealsInit();
        }

        private void SendAppealHandler()
        {
            var temp = new AppealsTable();
            temp.AppealText = EditorText;
            App.Database.AddRecordToAppealsTable(temp);
            Application.Current.MainPage.DisplayAlert("Sended", "", "OK");
            Application.Current.MainPage.Navigation.PopToRootAsync();
            OnSendAppealEvent();
        }

        //private void EventHandlerAppealsTable(Object sender, EventArgs e) {
        //    ListOfAppealsInit();
        //}

        private async void ListOfAppealsInit()
        {
            _listofappeals = await App.Database.GetAllRecordFromApppealsTable();
        }
        //APPEAL TEXT PAGE
        private bool AllowToAttachPhoto()
        {
            return EditorText?.Length > 0;
        }


        //APPEAL PHOTO LOOK PAGE COMMANDS
        private void DeleteImage()
        {
            TakenImages.Remove(ChoosenImage);
            ChoosenImage = null;
            Application.Current.MainPage.Navigation.PopAsync();
            AppealBtn.ChangeCanExecute();
            SendAppeal.ChangeCanExecute();
        }

        //APPEAL PHOTO ATTACH PAGE COMMANDS
        private bool AllowToAppeal()
        {
            return TakenImages.Count > 0;
        }
        
        async private void ImageViewer()
        {
            if (ChoosenImage != null)
                await Application.Current.MainPage.Navigation.PushAsync(new ImageViewer(this));
        }

        async private void AttachScreen()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PhotoAttachPage(this));
        }


        private void PhotoTaken(byte[] image)
        {
            Img temp = new Img(ImageSource.FromStream(() => new MemoryStream(image)));
            _takenimages.Add(temp);
            OnPropertyChanged("TakenImages");
            AppealBtn.ChangeCanExecute();
        }

        private async void Appeal()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new FinalAppealPage(this));
            
        }

        public string ProfileType
        {
            get
            {
                if (CurrentProfile.TypeOfProfile)
                    return "Гражданин";
                else
                    return "Организация";
            }
        }
        //APPEAL PHOTO ATTACH PAGE PROPS

        public ImageSource TakenFullScreenImage
        {
            get { return ChoosenImage.imageSource; }
        }

        private Img _choosenimage;
        public Img ChoosenImage
        {
            get { return _choosenimage; }
            set 
            {
                if (_choosenimage != value)
                {
                    _choosenimage = value;
                }
            }
        }


        private ObservableCollection<Img> _takenimages = new ObservableCollection<Img>();
        public ObservableCollection<Img> TakenImages
        {
            get { return _takenimages; }
        }

        //APPEAL TEXT PAGE PROPS
        private AppealsTable _editortextappealstable;
        public AppealsTable EditorTextAppealsTable
        {
            get { return _editortextappealstable; }
            set
            {
                if (_editortextappealstable != value && value != null)
                {
                    _editortextappealstable = value;
                    EditorText = value.AppealText;
                    OnPropertyChanged("EditorText");
                }
            }
        }

        private string _editortext;
        public string EditorText
        {
            get { return _editortext; }
            set
            {
                if (_editortext != value)
                {
                    _editortext = value;
                }
                GoToAttachScreen.ChangeCanExecute();
            }
        }

        private List<AppealsTable> _listofappeals;
        public List<AppealsTable> ListOfAppeals
        {
            get { return _listofappeals; }
        }

        public string DisplayCurProfile
        {
            get
            {
                if (CurrentProfile == null)
                    return "";
                return $"{CurrentProfile.Sername} {CurrentProfile.Name} {CurrentProfile.MiddleName}";
            }
        }

        public bool IsOrg
        {
            get
            {
                return !CurrentProfile.TypeOfProfile;
            }
        }


        public string FullName
        {
            get { return $"{CurrentProfile.Sername} {CurrentProfile.Name} {CurrentProfile.MiddleName}"; }
        }

        public string Email
        {
            get { return $"{CurrentProfile.Email}"; }
        }

        public string Phone
        {
            get { return $"{CurrentProfile.Phone}"; }
        }
        public string SelectedRegion
        {
            get { return $"{CurrentProfile.SelectedRegion}"; }
        }
        public string SelectedDiv
        {
            get { return $"{CurrentProfile.SelectedDiv}"; }
        }
        public string SelectedRegionOfIncident
        {
            get { return $"{CurrentProfile.SelectedRegionOfIncident}"; }
        }
        public string OrgName
        {
            get { return $"{CurrentProfile.OrgName}"; }
        }

        public string OrgOptionalInformation
        {
            get { return $"{CurrentProfile.OrgOptionalInformation}"; }
        }
        public string OutNumber
        {
            get { return $"{CurrentProfile.OutNumber}"; }
        }
        public string RegistrOrgDate
        {
            get { return $"{CurrentProfile.RegistrOrgDate}"; }
        }
        public string NumberLetter
        {
            get { return $"{CurrentProfile.NumberLetter}"; }
        }



    }
}
