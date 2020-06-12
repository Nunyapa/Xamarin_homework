using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace GIBDD.ModelViews
{
    class AppealModelView : INotifyPropertyChanged
    {
        public Command CommandBtn { get; set; }


        public AppealModelView()
        {
            CommandBtn = new Command(ChoosePhoto);
        }

        public AppealModelView(ProfilesTable prof)
        {
        }

        async void ChoosePhoto()
        {
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
