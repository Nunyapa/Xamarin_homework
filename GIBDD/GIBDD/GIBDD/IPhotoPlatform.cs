using System;
using System.Collections.Generic;
using System.Text;

namespace GIBDD
{
    public interface IPhotoPlatform
    {
        void TakePhoto();
        bool IsCameraAvaliable();
        void ChoosePhoto();
        Action<byte[]> TakePhotoCallBack { get; set;}
    }
}
