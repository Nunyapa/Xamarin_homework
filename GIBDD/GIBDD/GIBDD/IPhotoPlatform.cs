using System;
using System.Collections.Generic;
using System.Text;

namespace GIBDD
{
    public interface IPhotoPlatform
    {
        void takePhoto();
        bool cameraIsAvaliable();
        void choosePhoto();
    }
}
