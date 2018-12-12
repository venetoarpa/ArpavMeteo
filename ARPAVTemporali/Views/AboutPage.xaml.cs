using System;
using System.Collections.Generic;

using Xamarin.Forms;
using FFImageLoading.Forms;

namespace ARPAVTemporali.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            /*var cachedImage = new CachedImage()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                //WidthRequest = 300,
                //HeightRequest = 300,
                CacheDuration = TimeSpan.FromDays(30),
                DownsampleToViewSize = true,
                RetryCount = 0,
                RetryDelay = 250,
                //TransparencyEnabled = false, //obsolete
                LoadingPlaceholder = "loading.png",
                ErrorPlaceholder = "error.png",
                Source = ImageSource.FromResource("ARPAVTemporali.Images.logo-arpav.png")
			};*/
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			/*ImageSource myImage = ImageSource.FromResource("ARPAVTemporali.Images.logo-arpav.png"); //carico un file locale come sempre
			myCachedImaged.Source = myImage;*/
		}
    }
}
