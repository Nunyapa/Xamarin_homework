using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace GIBDD.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IPhotoPlatform
    {
        public Action<byte[]> TakePhotoCallBack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ChoosePhoto()
        {
            throw new NotImplementedException();
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            var App = new App(this);
            LoadApplication(App);
            return base.FinishedLaunching(app, options);
        }

        public bool IsCameraAvaliable()
        {
            throw new NotImplementedException();
        }

        public void TakePhoto()
        {
            throw new NotImplementedException();
        }
    }
}
