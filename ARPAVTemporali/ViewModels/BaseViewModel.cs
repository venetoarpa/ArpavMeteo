using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ARPAVTemporali.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // helper method usato nelle properties per notificare gli aggiornamenti
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //non condition operator ? instead of if (PropertyChanged != null)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
