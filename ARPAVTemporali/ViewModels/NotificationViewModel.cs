using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using Xamarin.Forms.Internals;

namespace ARPAVTemporali.ViewModels
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        public UserSettings UserSettings { get => Helpers.Settings.UserSettings; }
        public List<string> Intervals { get; }
        public List<string> Sounds { get; }
        public TimeSpan DNDFrom { get; set; }
        public TimeSpan DNDTo { get; set; }

        public int SelectedIntervalIndex
        {
            get
            {
                Dictionary<string, int> dictionary = Variables.Intervals;
                int needle = Helpers.Settings.Interval;
                return dictionary.Values.ToList().IndexOf(needle);
            }
        }
        public int SelectedSoundIndex
        {
            get
            {
                Dictionary<string, string> dictionary = Variables.Sounds;
                string needle = Helpers.Settings.Sound;
                return dictionary.Values.ToList().IndexOf(needle);
            }
        }



        public NotificationViewModel()
        {


            Intervals = new List<string>(Variables.Intervals.Keys.ToList());
            Sounds = new List<string>(Variables.Sounds.Keys.ToList());
            DNDFrom = TimeSpan.FromMinutes(Helpers.Settings.DNDFrom);
            DNDTo = TimeSpan.FromMinutes(Helpers.Settings.DNDTo);

        }


        public void UpdateSettings<T>(string propertyName, T value)
        {
            PropertyInfo propertyInfo = typeof(Helpers.Settings).GetProperty(propertyName);
            propertyInfo.SetValue(null, Convert.ChangeType(value, propertyInfo.PropertyType), null);

            OnPropertyChanged(nameof(UserSettings));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // helper method usato nelle properties per notificare gli aggiornamenti
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            /*Type myType = typeof(UserSettings);
            var val = myType.GetRuntimeProperty(propertyName).GetValue(this, null);
            Debug.WriteLine("valore: "+val);*/

            //non condition operator ? instead of if (PropertyChanged != null)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //Debug.WriteLine(propertyName + ": " + value);
        }
    }
}
