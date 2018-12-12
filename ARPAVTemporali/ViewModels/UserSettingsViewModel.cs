using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using Xamarin.Forms.Internals;

namespace ARPAVTemporali.ViewModels
{
    public class UserSettingsViewModel : INotifyPropertyChanged
    {


        public UserSettings UserSettings { get => Helpers.Settings.UserSettings; }

        public List<string> AnimationSpeeds { get; }
        public List<string> AnimationDurations { get; }
        public int SelectedAnimationSpeedIndex
        {
            get
            {
                Dictionary<string, int> dictionary = Variables.AnimationSpeeds;
                int needle = UserSettings.AnimationSpeed;
                return dictionary.Values.ToList().IndexOf(needle);
            }
        }
        public int SelectedAnimationDurationIndex
        {
            get
            {
                Dictionary<string, int> dictionary = Variables.AnimationDurations;
                int needle = UserSettings.AnimationDuration;
                return dictionary.Values.ToList().IndexOf(needle);
            }
        }

        public UserSettingsViewModel()
        {
            AnimationSpeeds = Variables.AnimationSpeeds.Keys.ToList();
            AnimationDurations = Variables.AnimationDurations.Keys.ToList();
        }


        public void UpdateSettings<T>(string propertyName, T value)
        {
            PropertyInfo propertyInfo = typeof(Helpers.Settings).GetProperty(propertyName);
            propertyInfo.SetValue(null, Convert.ChangeType(value, propertyInfo.PropertyType), null);

            /*PropertyInfo propertyInfo = UserSettings.GetType().GetProperty(propertyName);
            propertyInfo.SetValue(UserSettings, Convert.ChangeType(value, propertyInfo.PropertyType), null);*/

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
