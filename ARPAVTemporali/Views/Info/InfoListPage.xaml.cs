using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using Xamarin.Forms;

namespace ARPAVTemporali.Views.Info
{
    public partial class InfoListPage : ContentPage
    {
        public InfoListPage()
        {
            InitializeComponent();
			BindingContext = this;
		}

		protected async override void OnAppearing()
		{

			base.OnAppearing();

			// get notifications from database
            List<Testo> testi = await DatabaseHelper.GetTesti();
			//List<Testo> testi = await app.DatabaseConnection.Table<Testo>().ToListAsync();

			listview.ItemsSource = new ObservableCollection<Testo>(testi);
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
			var testo = e.Item as Testo;
			//DisplayAlert("Tapped", setting.Title, "Ok");

            Navigation.PushAsync(new InfoDetailPage(testo));
		}
    }
}
