using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using ARPAVTemporali.Models;
using Plugin.Settings;
using Xamarin.Forms;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.ViewModels;

namespace ARPAVTemporali.Views.SettingsPages
{

    public partial class MapTypePage : ContentPage
    {
        private List<string> _mapTypes;
        UserSettingsViewModel vm;

        public MapTypePage()
        {
            vm = new UserSettingsViewModel();

            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {

            string currentMapType = vm.UserSettings.MapType;
            _mapTypes = new List<string>(Variables.MapTypes.Keys.ToList());

            listview.ItemsSource = _mapTypes;
            listview.SelectedItem = currentMapType;
            listview.SelectedItem = Variables.MapTypes.FirstOrDefault((KeyValuePair<string, string> arg) => arg.Value == currentMapType).Key;

            base.OnAppearing();
        }

        protected override async void OnDisappearing()
        {
            // non need to update remote settings for map type
            //await Helpers.Settings.UpdateRemoteSettings(); //save settings to remote server
            base.OnDisappearing();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var mapType = e.SelectedItem as string;
            //((ListView)sender).SelectedItem = null; //unselect
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            string mapType = e.Item as string;
            if (Variables.MapTypes.ContainsKey(mapType))
            {
                string value = Variables.MapTypes[mapType];
                vm.UpdateSettings("MapType", value);
            }

            Navigation.PopAsync();
        }
    }
}
