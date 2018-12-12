using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ARPAVTemporali.Droid.Helpers;

[assembly: Dependency(typeof(ShareHelper))]
namespace ARPAVTemporali.Droid.Helpers
{
    public class ShareHelper: Activity, IShareHelper
    {
        public ShareHelper()
        {
            
        }

        public async void Share(string subject, string message, ImageSource image)
        {
            Intent intent = new Intent(Intent.ActionSend);
            //intent.PutExtra(Intent.ExtraSubject, subject);
            intent.PutExtra(Intent.ExtraText, message);
            intent.SetType("image/png");

            var handler = GetHandler(image);
            Bitmap bitmap = await handler.LoadImageAsync(image, this);

            Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads
                            + Java.IO.File.Separator + "mapImage.png");

            using (System.IO.FileStream os = new System.IO.FileStream(path.AbsolutePath, System.IO.FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
            }

            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(path));
            Forms.Context.StartActivity(Intent.CreateChooser(intent, message));
        }

        public async void SaveImageToCameraRoll(ImageSource image, string filename = "image.png")
        {

            var handler = GetHandler(image);
            Bitmap bitmap = await handler.LoadImageAsync(image, this);

            Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures
                                                                                         + Java.IO.File.Separator + filename);

            using (System.IO.FileStream os = new System.IO.FileStream(path.AbsolutePath, System.IO.FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
            }

        }

        public string GetDownloadPath()
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            return dir.Path;
        }

        private static IImageSourceHandler GetHandler(ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is UriImageSource)
            {
                returnValue = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                returnValue = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                returnValue = new StreamImagesourceHandler();
            }
            return returnValue;
        }
    }
}
