using System;
using System.Collections.Generic;
using System.Text;

namespace GIBDD
{
    public interface PhotoPlatform
    {
        void takePhoto();
        bool cameraIsAvaliable();
    }
}
