using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using System.Reflection;
using ARPAVTemporali.Helpers;

namespace ARPAVTemporali.Models
{
    public class UserSettings
    {
        /*
         * imposto _notificationEnabled come Nullable bool per verificare se è il primo avvio
         */
        private bool _notificationEnabled = false; //oppure bool? _notificationEnabled = null;
        private string _mapType = "Terrain";
        private double _opacity = 60.0d;
        private int _animationSpeed = 2;
        private int _animationDuration = 120;
		private int _interval = 30; //minuti di intervallo
        private double _dndFrom = 0;
        private double _dndTo = 0;
        private bool _dndEnabled = false;
        private string _sound = "thunder1.mp3";

        /*
         * TODO: gestire overlay con List
         */
        private List<string> _overlayList = new List<string>();

        private List<ComuneSettings> _comuni = new List<ComuneSettings>();


        public List<ComuneSettings> Comuni
        {
            get { return _comuni; }
            set
            {
                if ( _comuni.Equals(value) )
                    return;

                _comuni = value;
			}
		}

        //[JsonIgnore]
        public bool NotificationEnabled
        {
            get { return _notificationEnabled; }
            set
            {
                if (_notificationEnabled == value)
                    return;

                _notificationEnabled = value;

                // aggiorno lo stato di sottoscrizione alle notifiche
                MessagingCenter.Send(this,Events.NotificationEnabledValueChanged, value);
            }
        }

        public string MapType
        {
            get { return _mapType; }
            set
            {
                if (_mapType == value)
                    return;

                _mapType = value;
            }
        }

        public double Opacity
        {
            get { return _opacity; }
            set
            {
                if (_opacity.Equals(value))
                    return;

                _opacity = value;
            }
        }

        public int AnimationSpeed
        {
            get { return _animationSpeed; }
            set
            {
                if (_animationSpeed.Equals(value))
                    return;

                _animationSpeed = value;
            }
        }

		public int AnimationDuration
		{
			get { return _animationDuration; }
			set
			{
                if (_animationDuration.Equals(value) )
					return;

				_animationDuration = value;
			}
		}

		public int Interval
		{
			get { return _interval; }
			set
			{
				if (_interval.Equals(value))
					return;

				_interval = value;
			}
		}

		public double DNDFrom
		{
            get { return _dndFrom; }
			set
			{
				if (_dndFrom.Equals(value))
					return;

				_dndFrom = value;
			}
		}

		public double DNDTo
		{
            get { return _dndTo; }
			set
			{
				if (_dndTo.Equals(value))
					return;

				_dndTo = value;
			}
		}


		[JsonProperty(PropertyName = "Dndenabled")]
		public bool DNDEnabled
		{
			get { return _dndEnabled; }
			set
			{
				if (_dndEnabled == value)
					return;

				_dndEnabled = value;
			}
		}

        [JsonProperty(PropertyName = "Dnd")]
		public List<int> DND
		{
            get
            {
                if (_dndEnabled)
                    return new List<int> { Convert.ToInt32(_dndFrom), Convert.ToInt32(_dndTo) };
                else
                    return new List<int> { -1, -1 };
            }
		}

		public string Sound
		{
			get { return _sound; }
			set
			{
				if (_sound == value)
					return;

				_sound = value;

			}
		}


	}
}