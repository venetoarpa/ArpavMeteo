using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using FFImageLoading.Forms;
using Plugin.Multilingual;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace ARPAVTemporali.Views
{
    public partial class HomePage : ContentPage
    {

        public ICommand TapCommand { get; private set; }

        public HomePage()
        {
            BindingContext = this;
            TapCommand = new Command<string>(GoToPagina);

			//CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo("it");
			//Localization.AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;

            InitializeComponent();

            Title = "ARPAV Temporali";

            //CheckInitialSettings();

		}

        protected async override void OnAppearing()
        {
            
            // Debug.WriteLine("updating settings");
            // await Helpers.Settings.UpdateSettings();

            Debug.WriteLine("appearing home");
            map.InitMap();
            App app = Application.Current as App;
            MessagingCenter.Send(app, Events.CheckNotificationCount);

            base.OnAppearing();

        }

        protected override void OnDisappearing()
        {
            map.Pause();
            base.OnDisappearing();
		}


		async void GoToPagina(string typeName)
		{
            ContentPage page;

			switch (typeName)
			{
				case "MapPage":
                    page = new MapPage();
					break;
				default:
					Type elementType = Type.GetType("ARPAVTemporali." + typeName);
                    page = (ContentPage) Activator.CreateInstance(elementType);
                    break;
			}

			/*prevent jump effect on iOS
             * see https://stackoverflow.com/a/38146688/1875109
             * https://bugzilla.xamarin.com/show_bug.cgi?id=32830
             */
        	//NavigationPage.SetHasNavigationBar(page, false); //hide the navigation bar to prevent the jump effect
            try
            {
				await Navigation.PushAsync(page as ContentPage, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
			//NavigationPage.SetHasNavigationBar(page, true); //show the navigation bar after page has changed
		}
    }
}
