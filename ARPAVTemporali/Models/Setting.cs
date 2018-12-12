using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ARPAVTemporali.Models
{
    public class Setting: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _title;
		private string _name;
        private string _description;
		private object _value;

		public string Name
		{
			get { return _name; }
			set
			{
				if (_name == value)
					return;

				_name = value;

				//onPropertyChanged(nameof(Name));
				/*
                 * grazie a CallerMemberName nel metodo OnPropertyChanged
                 * posso omettere nameOf(Name)
                 */
				OnPropertyChanged();
			}
		}

		public string Title
		{
			get { return _title; }
			set
			{
				if (_title == value)
					return;

				_title = value;

				OnPropertyChanged();
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if (_description == value)
					return;

				_description = value;

				OnPropertyChanged();
			}
		}


		public object Value
		{
			get { return _value; }
			set
			{
				if (_value == value)
					return;

				_value = value;

				OnPropertyChanged();
			}
		}


        // helper method usato nelle properties per notificare gli aggiornamenti
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            //Debug.WriteLine("property changed:" + propertyName);
            //non condition operator ? instead of if (PropertyChanged != null)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
	}
}
