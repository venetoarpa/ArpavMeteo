using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ARPAVTemporali.ViewModels
{
    public class ComuneViewModel: BaseViewModel
    {
        public UserSettings UserSettings { get => Helpers.Settings.UserSettings; }

        // uso una property per accedere alle nested properties nello XAML
        public ComuneSettings ComuneSettings { get; set; }
        public List<double> Ranges { get; set; }
        public List<string> Intensities { get; set; }
        public int SelectedRangeIndex
        {
            get
            {
                List<double> ranges = Variables.Ranges;
                double needle = ComuneSettings.Range;
                return ranges.IndexOf(needle);
            }
        }
        public int SelectedIntensityIndex
        {
            get
            {
                Dictionary<string, int> dictionary = Variables.Intensities;
                int needle = ComuneSettings.Intensity;
                return dictionary.Values.ToList().IndexOf(needle);
            }
        }

        public ComuneViewModel(ComuneSettings settings)
        {
            ComuneSettings = settings;

            Ranges = new List<double>(Variables.Ranges);
            Intensities = new List<string>(Variables.Intensities.Keys.ToList());
        }

        public void UpdateSettings<T>(string propertyName, T value)
        {
            PropertyInfo propertyInfo = ComuneSettings.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(ComuneSettings, Convert.ChangeType(value, propertyInfo.PropertyType), null);

            OnPropertyChanged(nameof(ComuneSettings));
			MessagingCenter.Send(this, Events.ComuneValueChanged);

        }

    }
}
