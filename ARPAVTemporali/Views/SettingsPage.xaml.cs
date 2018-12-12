using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ARPAVTemporali.Models;
using ARPAVTemporali.Views.SettingsPages;
using System.Diagnostics;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Linq;
using ARPAVTemporali.Localization;
using Plugin.Multilingual;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ARPAVTemporali.Views
{
    public class SettingPage
    {
        public object Title { get; set; }
        public Type PageType { get; set; }
    }

    public partial class SettingsPage : ContentPage
    {

        private ObservableCollection<SettingPage> _settingPages;


        public SettingsPage()
        {
			var app = Application.Current as App;

            InitializeComponent();

			BindingContext = this;
        }

		protected override void OnAppearing()
		{
            string path = "ARPAVTemporali.Views.SettingsPages.";

            _settingPages = new ObservableCollection<SettingPage>
            {
                new SettingPage{Title=AppResources.Impostazioni_Mappa_Titolo, PageType =  Type.GetType(path+"MapTypePage")},
                new SettingPage{Title=AppResources.Impostazioni_RadarFulmini_Titolo, PageType = Type.GetType(path+"RadarFulminiPage")},
                new SettingPage{Title=AppResources.Impostazioni_SelezionaLocalita_Titolo,PageType = Type.GetType(path+"ComuniPage")},
                new SettingPage{Title=AppResources.Impostazioni_Notifiche_Titolo, PageType = Type.GetType(path+"NotificationPage")},
				new SettingPage{Title=AppResources.Impostazioni_Lingua_Titolo, PageType = Type.GetType(path+"LanguagePage")},
            };

			listview.ItemsSource = _settingPages;

			base.OnAppearing();
		}

        void Handle_ClearButtonClicked(object sender, System.EventArgs e)
        {
            App app = Application.Current as App;
            Helpers.Settings.Language = "";
            Helpers.Settings.DisclaimerRead = false;
        }

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var setting = e.SelectedItem as SettingPage;
			((ListView)sender).SelectedItem = null; //unselect
            /*
             * attenzione: con custom renderers e.SelectedItem potrebbe risultare null
             */
		}

		async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			var setting = e.Item as SettingPage;
			//DisplayAlert("Tapped", setting.Title, "Ok");
			if(setting.PageType != null)
            {
                ContentPage page = (ContentPage)Activator.CreateInstance(setting.PageType);
                await Navigation.PushAsync(page as ContentPage, true);
            }
		}
    }
}
