using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SQLite;

namespace ARPAVTemporali.Models
{

    public class Radar
    {
		public string Name { get; set; }
        public double Surface { get; set; }
        public double MaxDBZ { get; set; }
        public int Count { get; set; }
    }

	public class Notification : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
        public string notification_ID { get; set; }

        private ObservableCollection<Radar> _radars = new ObservableCollection<Radar>();

        private string _comune = "";
        public string Comune
        {
            get { return _comune; }
            set
            {
                if (_comune == value)
                    return;

                _comune = value;
                OnPropertyChanged();
            }
        }

		private string _key { get; set; }
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key == value)
                    return;

                _key = value;
                OnPropertyChanged();
            }
        }

        private DateTime _data;
        public DateTime Data
        {
            get { return _data; }
            set
            {
                if (_data.Equals(value))
                    return;

                _data = value;
                OnPropertyChanged();
            }
        }

        public string DataFormattata
        {
            get {
                /*string test = _data.ToString("dd/MM/yyyy HH:mm");
                Debug.WriteLine(_data);
                Debug.WriteLine(test);*/
                return _data.ToString("dd/MM/yyyy HH:mm");
            }
        }

        private double _distanza = 0;
        public double Distanza
        {
            get { return _distanza; }
            set
            {
                if (_distanza.Equals(value))
                    return;

                _distanza = value;
                OnPropertyChanged();
            }
        }

        [Ignore]
        public ObservableCollection<Radar> Radars
        {
            get { return _radars; }
            set
            {
                if (_radars.Equals(value))
                    return;

                _radars = value;
                OnPropertyChanged();
            }
        }

		private bool _isRead = false;
        public bool IsRead
        {
            get { return _isRead; }
            set
            {
                if (_isRead == value)
                    return;

				_isRead = value;
				OnPropertyChanged();
			}
		}

        public Notification()
        {
        }

		// helper method usato nelle properties per notificare gli aggiornamenti
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			/*Type myType = typeof(UserSettings);
            var val = myType.GetRuntimeProperty(propertyName).GetValue(this, null);
            Debug.WriteLine("valore: "+val);*/

			//non condition operator ? instead of if (PropertyChanged != null)
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


		}
    }
}
