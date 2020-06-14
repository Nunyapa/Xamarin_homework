using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Provider;
using Android.Support.V4.App;
using Android.Graphics;
using System.IO;

namespace GIBDD.Droid
{
    [Activity(Label = "GIBDD", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPhotoPlatform
    {
        private const int CameraPermissionRequest = 1000;
        private const int CameraRequest = 2000;
        private const int ChoosePhotoPermissionRequest = 1001;
        private const int ChoosePhotoRequest = 2001;
        private const int SavePhotoPermissionRequest = 1002;
        private byte[] image;

        public Action<byte[]> TakePhotoCallBack { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var App = new App(this);
            LoadApplication(App);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            for (int i = 0; i < permissions.Length; i++)
            {
                var p = permissions[i];
                var g = i < grantResults.Length ? grantResults[i] : Permission.Denied;
                if (p == Android.Manifest.Permission.Camera && g == Permission.Granted)
                {
                    TakePhotoInternal();
                }
                else if (p == Android.Manifest.Permission.ReadExternalStorage && g == Permission.Granted)
                {
                    ChoosePhotoInternal();
                }
                else if (p == Android.Manifest.Permission.WriteExternalStorage && g == Permission.Granted)
                {
                    SaveImageInternal();
                }
            }
        }

        public void TakePhoto()
        {
            var permission = Android.Manifest.Permission.Camera;
            if (Application.Context.CheckSelfPermission(permission) == Permission.Granted)
            {
                TakePhotoInternal();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new[] { permission }, CameraPermissionRequest);
            }
        }

        private void TakePhotoInternal()
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, CameraRequest);
        }

        public bool IsCameraAvaliable()
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            var availiableActivities = Application.Context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availiableActivities != null && availiableActivities.Count > 0;
        }

        public void ChoosePhoto()
        {
            var permission = Android.Manifest.Permission.ReadExternalStorage;
            if (Application.Context.CheckSelfPermission(permission) == Permission.Granted)
            {
                ChoosePhotoInternal();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new[] { permission }, ChoosePhotoPermissionRequest);
            }
        }

        private void ChoosePhotoInternal()
        {
            var intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), ChoosePhotoRequest);
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            if (requestCode == CameraRequest && resultCode == Result.Ok)
            {
                var bitmap = (Bitmap)data.Extras.Get("data");
                if (bitmap != null)
                {
                    var stream = new MemoryStream();
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 80, stream);
                    image = stream.ToArray();
                    TakePhotoCallBack(image);
                    SaveImage();
                }
            }

            if (requestCode == ChoosePhotoRequest && resultCode == Result.Ok)
            {
                var bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, data.Data);
                if (bitmap != null)
                {
                    var stream = new MemoryStream();
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 80, stream);
                    image = stream.ToArray();
                    TakePhotoCallBack(image);
                }
            }
        }

        public void SaveImage()
        {
            var permission = Android.Manifest.Permission.WriteExternalStorage;
            if (Application.Context.CheckSelfPermission(permission) == Permission.Granted)
            {
                SaveImageInternal();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new[] { permission }, SavePhotoPermissionRequest);
            }

        }

        private void SaveImageInternal()
        {
            var picturesPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures); 
            if (!picturesPath.Exists())
            {
                picturesPath.Mkdirs();
            }
            var file = new Java.IO.File(picturesPath, $"IMG_{Guid.NewGuid()}.jpg");
            File.WriteAllBytes(file.Path, image);
            var intent = new Intent(Intent.ActionMediaScannerScanFile);
            intent.SetData(Android.Net.Uri.FromFile(file));
            SendBroadcast(intent);
        }
    }
}