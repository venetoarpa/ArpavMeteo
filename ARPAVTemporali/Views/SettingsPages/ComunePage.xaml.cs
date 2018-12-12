using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ARPAVTemporali.ViewModels;
using Xamarin.Forms;

namespace ARPAVTemporali.Views.SettingsPages
{
    public partial class ComunePage : ContentPage
    {


        private ComuneViewModel vm;

        public ComunePage(ComuneSettings settings=null)
        {
            vm = new ComuneViewModel(settings);

            BindingContext = vm;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public void Handle_RaggioIndexChanged(object sender, EventArgs e)
		{
            var picker = sender as Picker;
            var index = picker.SelectedIndex;
            double value = Variables.Ranges.ElementAt(index);

            if(!vm.ComuneSettings.Range.Equals(value))
                vm.UpdateSettings("Range", value);

		}

		public void Handle_IntensityIndexChanged(object sender, EventArgs e)
		{
            var picker = sender as Picker;
            var index = picker.SelectedIndex;
            int value = Variables.Intensities.Values.ElementAt(index);

            if (!vm.ComuneSettings.Intensity.Equals(value))
                vm.UpdateSettings("Intensity", value);

		}


        public void Handle_ShowRangeToggled(object sender, ToggledEventArgs e)
        {
			var toggled = (bool)e.Value;
            vm.UpdateSettings("ShowRange", toggled);

        }

        protected override void OnDisappearing()
        {
			Debug.WriteLine(vm.ComuneSettings.Intensity);
            base.OnDisappearing();
		}

    }
}
