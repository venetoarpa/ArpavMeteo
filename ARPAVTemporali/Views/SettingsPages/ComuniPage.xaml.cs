using System;
using System.Diagnostics;
using System.Linq;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using ARPAVTemporali.ViewModels;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace ARPAVTemporali.Views.SettingsPages
{
    public partial class ComuniPage : ContentPage
    {

        public bool isButtonAggiungiComuneEnabled { get; set; }
        ComuniViewModel vm;

        public ComuniPage()
        {
            vm = new ComuniViewModel();
			
            BindingContext = vm;

            
            InitializeComponent();


            MessagingCenter.Subscribe<SearchComunePage, string>(this, Events.AddComuneLabel, OnAddComuneAsync);

        }

        protected override void OnAppearing()
        {
            updateButtonEnabled();

            listview.ItemsSource = vm.Comuni;
            //listview.SetBinding(ListView.ItemsSourceProperty, "Comuni");
            //listview.BindingContext = vm;
			base.OnAppearing();
		}

        protected async override void OnDisappearing()
        {
            await Helpers.Settings.UpdateRemoteSettings(); //save settings to remote server
            base.OnDisappearing();
        }

        private void updateButtonEnabled()
        {
            addComuneButton.IsEnabled = vm.IsAddButtonButtonEnabled();
        }


        private async void OnAddComuneAsync(SearchComunePage source, string name)
        {
            var comune = vm.AddComune(name);
            if(comune!=null)
                await Navigation.PushAsync(new ComunePage(comune)); //apri la pagina di impostazione del comune

        }



        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new SearchComunePage());
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null; //unselect
			/*
			 * attenzione: con custom renderers e.SelectedItem potrebbe risultare null
			 */
		}

		void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
            ComuneSettings settings = e.Item as ComuneSettings;
			//DisplayAlert("Tapped", setting.Title, "Ok");

			if (settings != null)
                Navigation.PushAsync(new ComunePage(settings));
		}

		// Context Actions
		public void OnMore(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
		}

		public void OnDelete(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
            ComuneSettings comune = mi.CommandParameter as ComuneSettings;
            vm.RemoveComune(comune);

            updateButtonEnabled();

			// DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
		}
    }
}
