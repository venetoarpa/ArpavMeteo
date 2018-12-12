using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace ARPAVTemporali
{
	public class ComuneSettings : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string _name;
		private int _intensity = 0;
		private double _range = 30;
		private bool _showRange = true;

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

		public int Intensity
		{
			get { return _intensity; }
			set
			{
				if (_intensity.Equals(value))
					return;

				_intensity = value;
				OnPropertyChanged();
			}
		}

		public double Range
		{
			get { return _range; }
			set
			{
				if (_range.Equals(value))
					return;

				_range = value;
				OnPropertyChanged();
			}
		}

		public bool ShowRange
		{
			get { return _showRange; }
			set
			{
				if (_showRange == value)
					return;

				_showRange = value;
				OnPropertyChanged();
			}
		}

		// helper method usato nelle properties per notificare gli aggiornamenti
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			//non condition operator ? instead of if (PropertyChanged != null)
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
