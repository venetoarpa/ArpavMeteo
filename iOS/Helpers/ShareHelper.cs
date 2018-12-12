using System;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.iOS.Helpers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(ShareHelper))]
namespace ARPAVTemporali.iOS.Helpers
{
    public class ShareHelper : IShareHelper
    {
        public ShareHelper()
        {
        }

        public async void Share(string subject, string message, ImageSource image)
        {
            var handler = GetHandler(image);

            try
            {
                using(UIImage uiImage = await handler.LoadImageAsync(image))
                {
                    var img = NSObject.FromObject(uiImage);
                    var mess = NSObject.FromObject(message);

                    var activityItems = new[] { mess, img };
                    var activityController = new UIActivityViewController(activityItems, null);

                    var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

                    while (topController.PresentedViewController != null)
                    {
                        topController = topController.PresentedViewController;
                    }

                    topController.PresentViewController(activityController, true, () => { });
                }
				

            }
            catch (Exception ex)
            {
                Console.WriteLine($"error sharing data: {ex.Message}");
            }


        }

        public async void SaveImageToCameraRoll(ImageSource image, string filename = "image.png")
        {


        }

        public string GetDownloadPath()
        {
            return Environment.SpecialFolder.MyPictures.ToString();
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
