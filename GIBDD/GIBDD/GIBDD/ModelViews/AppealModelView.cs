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
            GoToAttachScreen = new Command(() => AttachScreen());
            GoToImageViewer = new Command(() => ImageViewer());
            DeleteCurrentImage = new Command(DeleteImage);
            platform.TakePhotoCallBack = PhotoTaken;
        }

        private void DeleteImage()
        {
            TakenImages.Remove(TakenImage);
            TakenImage = null;
            Application.Current.MainPage.Navigation.PopAsync();
        }

        async private void ImageViewer()
        {
            if (TakenImage != null)
                await Application.Current.MainPage.Navigation.PushAsync(new ImageViewer(this));
        }

        async private void AttachScreen()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PhotoAttachPage(this));
        }

        public AppealModelView(ProfilesTable prof)
        {
        }

        private void PhotoTaken(byte[] image)
        {
            ImageSource temp = ImageSource.FromStream(() => new MemoryStream(image));
            TakenImage = new Img(temp);
            _takenimages.Add(TakenImage);

            OnPropertyChanged();
        }
       

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public ImageSource TakenFullScreenImage
        {
            get { return _takenimage.imageSource; }
        }

        private Img _takenimage;
        public Img TakenImage
        {
            get { return _takenimage; }
            set 
            {
                if (_takenimage != value)
                {
                    _takenimage = value;
                }
            }
        }


        private ObservableCollection<Img> _takenimages = new ObservableCollection<Img>();
        public ObservableCollection<Img> TakenImages
        {
            get { return _takenimages; }
            set { }
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
            }
        }
    }
}
