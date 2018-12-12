using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ARPAVTemporali.Models;
using System.Linq;
using System.Diagnostics;
using ARPAVTemporali.ViewModels;

namespace ARPAVTemporali.Views.SettingsPages
{
    public partial class SearchComunePage : ContentPage
    {
        private SearchComuneViewModel vm;

		public SearchComunePage()
		{
			InitializeComponent();
            vm = new SearchComuneViewModel();
            BindingContext = vm;

		}

        protected override void OnAppearing()
        {
            listview.ItemsSource = vm.ElencoComuni;
            listview.SetBinding(ListView.ItemsSourceProperty, nameof(vm.ElencoComuni));
            listview.BindingContext = vm;
        }

        void Handle_UndoClicked(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var comune = e.SelectedItem as Comune;
            ((ListView)sender).SelectedItem = null; //unselect
        }

        async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var comune = e.Item as Comune;
            // DisplayAlert("Tapped", comune.Name, "Ok");
            await Navigation.PopModalAsync();
            MessagingCenter.Send(this, Events.AddComuneLabel, comune.Name);
        }


        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            string filter = e.NewTextValue;
            vm.SetFilter(filter);
        }


    }
}
