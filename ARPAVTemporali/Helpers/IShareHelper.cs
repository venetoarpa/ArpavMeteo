using System;
using Xamarin.Forms;

namespace ARPAVTemporali.Helpers
{
    public interface IShareHelper
    {
        void Share(string subject, string message, ImageSource image);

        void SaveImageToCameraRoll(ImageSource image, string filename = "image.png");

        string GetDownloadPath();
    }
}
