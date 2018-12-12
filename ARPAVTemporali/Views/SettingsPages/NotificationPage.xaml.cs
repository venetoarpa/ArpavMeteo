using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using ARPAVTemporali.ViewModels;
using Xamarin.Forms;

namespace ARPAVTemporali.Views.SettingsPages
{
    public partial class NotificationPage : ContentPage
    {
        NotificationViewModel vm;

        public NotificationPage()
        {
            vm = new NotificationViewModel();

            BindingContext = vm;
            InitializeComponent();
        }

        protected override async void OnDisappearing()
        {
            await Helpers.Settings.UpdateRemoteSettings(); //save settings to remote server
            base.OnDisappearing();
        }

        void Handle_IntervalSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var index = picker.SelectedIndex;
            int value = Variables.Intervals.Values.ElementAt(index);

            if (!vm.UserSettings.Interval.Equals(value) )
                vm.UpdateSettings("Interval", value);
        }

        void Handle_SoundSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var index = picker.SelectedIndex;
            string value = Variables.Sounds.Values.ElementAt(index);

            if(!vm.UserSettings.Sound.Equals(value))
                vm.UpdateSettings("Sound", value);
        }

        void Handle_NotificationEnabledToggled(object sender, EventArgs e)
        {
            Switch _switch = sender as Switch;

            if (vm.UserSettings.NotificationEnabled != _switch.IsToggled)
            {
                vm.UpdateSettings("NotificationEnabled", _switch.IsToggled);
            }

        }

        void Handle_DNDEnabledToggled(object sender, EventArgs e)
        {
            Switch dnd_switch = sender as Switch;

            if (vm.UserSettings.DNDEnabled != dnd_switch.IsToggled)
                vm.UpdateSettings("DNDEnabled", dnd_switch.IsToggled);

        }

        void Handle_TimeChanged(object sender, PropertyChangingEventArgs e)
        {
			TimePicker picker = sender as TimePicker;

            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
				int hours = picker.Time.Hours;
				int minutes = picker.Time.Minutes;
				double totalMinutes = picker.Time.TotalMinutes;

                if(picker.Equals(DND_fromPicker))
                {

                    vm.UpdateSettings("DNDFrom", totalMinutes);

                }
                if(picker.Equals(DND_toPicker))
                {

                    vm.UpdateSettings("DNDTo", totalMinutes);
                }

            }

        }
    }
}
