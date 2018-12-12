using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using ARPAVTemporali.Views.SettingsPages;
using Xamarin.Forms;

namespace ARPAVTemporali.ViewModels
{
    public class ComuniViewModel: INotifyPropertyChanged
    {
        private int _maxComuni = 3;

        private ObservableCollection<ComuneSettings> _comuni = new ObservableCollection<ComuneSettings>();
        public ObservableCollection<ComuneSettings> Comuni
        {
            get
            {
                return _comuni;
            }
            set
            {
                if (_comuni.Equals(value))
                    return;
                
                _comuni = value;
                OnPropertyChanged(); //omit the name thanks to [CallerMemberName]
            }
        }

        public ComuniViewModel()
        {

            var comuni = Helpers.Settings.Comuni;
            if(comuni != null)
            {
                Comuni = new ObservableCollection<ComuneSettings>(comuni.ToList());
            }

            MessagingCenter.Subscribe<ComuneViewModel>(this, Events.ComuneValueChanged, OnComuneChanged);
        }

        //aggiorno i valori del binding quando modifico le impostazioni
        private void OnComuneChanged(ComuneViewModel sender)
        {
            OnPropertyChanged("Comuni");
            // save remote settings
            //Helpers.Settings.UpdateSettings();
        }

        public bool IsAddButtonButtonEnabled()
        {
            if (Comuni == null) return true;

            return Comuni.Count < _maxComuni;
        }

        public ComuneSettings AddComune(string name)
        {
            // crea un comune con le impostazioni di default
            ComuneSettings comune = new ComuneSettings { Name = name };

            ComuneSettings currentSettings = Comuni.FirstOrDefault<ComuneSettings>(c => c.Name == comune.Name); //cerco se il comune è gi


            if ((currentSettings == null) && (Comuni.Count < _maxComuni))
            {
                Comuni.Add(comune);

                OnPropertyChanged("Comuni");

                return comune;
            }
            return null;

        }

        public void RemoveComune(ComuneSettings comune)
        {

            var removingItem = Comuni.FirstOrDefault(c => c.Name == comune.Name);
            if (removingItem != null)
            {
				
                Comuni.Remove(removingItem);

                OnPropertyChanged("Comuni");
            }


            // DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }

        // save sottings locally
        private void PersistSettings()
        {
            Helpers.Settings.Comuni = Comuni.ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;


        // helper method usato nelle properties per notificare gli aggiornamenti
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //non condition operator ? instead of if (PropertyChanged != null)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            PersistSettings();
            MessagingCenter.Send(this,Events.ComuneValueChanged);
        }
    }
}
