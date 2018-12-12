using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Touch;

using Foundation;
using UIKit;


namespace ARPAVTemporali.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

			Xamarin.FormsGoogleMaps.Init("AIzaSyAY4MjZQ1k5nH8V5Mtpk1M6bxkEwqt-XRo"); //Google Maps

            LoadApplication(new App());

            CachedImageRenderer.Init(); //FFImageLoading

            return base.FinishedLaunching(app, options);
        }
    }
}
