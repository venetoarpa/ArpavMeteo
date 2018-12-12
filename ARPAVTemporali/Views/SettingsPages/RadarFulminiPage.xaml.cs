using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plugin.Settings;
using Xamarin.Forms;
using ARPAVTemporali.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

using ARPAVTemporali.Localization;
using System.Reflection;
using Plugin.Multilingual;
using ARPAVTemporali.ViewModels;

namespace ARPAVTemporali.Views.SettingsPages
{

    public partial class RadarFulminiPage : ContentPage, INotifyPropertyChanged
    {
        UserSettingsViewModel vm;

        public RadarFulminiPage()
        {
            vm = new UserSettingsViewModel();
            //System.Resources.ResourceManager resmgr = new System.Resources.ResourceManager("ARPAVTemporali.Localization.AppResources", typeof(AppResources).GetTypeInfo().Assembly);
            //var ci = CrossMultilingual.Current.CurrentCultureInfo;
            //string test = resmgr.GetString("Variables.MapTypes.Rilievo",ci);

			BindingContext = vm;
			
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
		
            //gestione tap su android (in caso l'utente non trascini il cursore)
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                // handle the tap
                Debug.WriteLine("tapped");
                Slider slider = s as Slider;
                double opacity = Convert.ToDouble(slider.Value);
                vm.UpdateSettings("Opacity", opacity);
            };
            opacitySlider.GestureRecognizers.Add(tapGestureRecognizer);

            base.OnAppearing();

        }

        protected override async void OnDisappearing()
        {
            var opacity = opacitySlider.Value;
            vm.UpdateSettings("Opacity", opacity);
         
            await Helpers.Settings.UpdateRemoteSettings(); //save settings to remote server

            base.OnDisappearing();
        }

        void Handle_AnimationSpeedSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var index = picker.SelectedIndex;
            int value = Variables.AnimationSpeeds.Values.ElementAt(index);

            if (!vm.UserSettings.AnimationSpeed.Equals(value))
                vm.UpdateSettings("AnimationSpeed", value);
        }

		void Handle_AnimationDurationSelectedIndexChanged(object sender, EventArgs e)
		{
            var picker = sender as Picker;
            var index = picker.SelectedIndex;
            int value = Variables.AnimationDurations.Values.ElementAt(index);

            if (!vm.UserSettings.AnimationDuration.Equals(value))
                vm.UpdateSettings("AnimationDuration", value);
		}

        void Handle_OpacityValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
			var value = Convert.ToDouble(e.NewValue);
            //userSettings.Opacity = opacity;
            if (!vm.UserSettings.Opacity.Equals(value))
                vm.UpdateSettings("Opacity", value);
        }

        void Handle_OpacityUnfocused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Debug.WriteLine("unfocusing");
            Slider slider = sender as Slider;
            var value = slider.Value;

            if (!vm.UserSettings.Opacity.Equals(value))
                vm.UpdateSettings("Opacity", slider.Value);
        }

    }
}
