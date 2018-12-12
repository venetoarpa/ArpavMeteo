// Helpers/Settings.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ARPAVTemporali.Models;
using Newtonsoft.Json;
using Plugin.Multilingual;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ARPAVTemporali.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
                if (CrossSettings.IsSupported)
                    return CrossSettings.Current;

                return null; // or your custom implementation 
			}
		}

        #region Setting Constants

        private static readonly UserSettings UserSettingsDefault = new UserSettings();

        #endregion

        public static string Language
        {
            get => AppSettings.GetValueOrDefault(nameof(Language), string.Empty);
            set {
				AppSettings.AddOrUpdateValue(nameof(Language), value);
                SetCultureInfo(value); //imposta la lingua del dispositivo
            }
        }

		// funzione che imposta la lingua del dispositivo
		public static void SetCultureInfo(string language)
		{
			CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo(language);
			Localization.AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
		}

        /*
        * propietà con impostazione sull'avvenuta lettura del disclaimer
        */
        public static bool DisclaimerRead
        {
            get => AppSettings.GetValueOrDefault(nameof(DisclaimerRead), false);
            set => AppSettings.AddOrUpdateValue(nameof(DisclaimerRead), value);
        }

        /*
        * propietà con impostazione sul livello 
        */
        public static bool LayerFulmini
        {
            get => AppSettings.GetValueOrDefault(nameof(LayerFulmini), true);
            set => AppSettings.AddOrUpdateValue(nameof(LayerFulmini), value);
        }

        /*
        * propietà con impostazione sul livello mosaico
        */
        public static bool LayerMosaico
        {
            get => AppSettings.GetValueOrDefault(nameof(LayerMosaico), true);
            set => AppSettings.AddOrUpdateValue(nameof(LayerMosaico), value);
        }


        public static bool NotificationPermissionAsked
        {
            get => AppSettings.GetValueOrDefault(nameof(NotificationPermissionAsked), false);
            set => AppSettings.AddOrUpdateValue(nameof(NotificationPermissionAsked), value);
        }


        /*
         * propietà con le impostazioni dell'utente
         */
        public static UserSettings UserSettings
        {
            get {
                return new UserSettings
                {
                    Comuni = Helpers.Settings.Comuni,
                    NotificationEnabled = Helpers.Settings.NotificationEnabled,
                    MapType = Helpers.Settings.MapType,
                    Opacity = Helpers.Settings.Opacity,
                    AnimationSpeed = Helpers.Settings.AnimationSpeed,
                    AnimationDuration = Helpers.Settings.AnimationDuration,
                    Interval = Helpers.Settings.Interval,
                    DNDFrom = Helpers.Settings.DNDFrom,
                    DNDTo = Helpers.Settings.DNDTo,
                    DNDEnabled = Helpers.Settings.DNDEnabled,
                    Sound = Helpers.Settings.Sound,
                };
            }
        }

        public static async Task UpdateRemoteSettings()
        {
            var app = Application.Current as App;
            var USER_ID = await app.NotificationManager.UserID();
            string URL = string.Format(Variables.ApplicationServerURL + "/app/config/{0}", USER_ID);
            HttpClient _client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), URL);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string settings = JsonConvert.SerializeObject(UserSettings);

            request.Content = new StringContent(settings, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                HttpContent httpContent = response.Content;
                string responseString = await httpContent.ReadAsStringAsync();
                Debug.WriteLine("updating settings: " + responseString);
            }
            catch (Exception ex)
            {
                string errorType = ex.GetType().ToString();
                string errorMessage = errorType + ": " + ex.Message;
                Debug.WriteLine("errore durante l'aggiornamento dei dati: " + errorMessage);
                //throw new Exception(errorMessage, ex.InnerException);
            }
        }


        /*
         * User Settings
         */


        /*
         * propietà con le impostazioni dei comuni
         */

        private static readonly List<ComuneSettings> ComuniDefault = new List<ComuneSettings>();

        private static string _comuni
        {
            get => AppSettings.GetValueOrDefault(nameof(_comuni), JsonConvert.SerializeObject(ComuniDefault));
            set => AppSettings.AddOrUpdateValue(nameof(_comuni), value);
        }
        public static List<ComuneSettings> Comuni
        {
            get
            {
                return JsonConvert.DeserializeObject<List<ComuneSettings>>(_comuni);
            }
            set
            {
                string json = JsonConvert.SerializeObject(value);
                _comuni = json;
            }
        }

        public static bool NotificationEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(NotificationEnabled), false);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(NotificationEnabled), value);
            }
        }

        public static string MapType
        {
            get => AppSettings.GetValueOrDefault(nameof(MapType), "Terrain");
            set
            {
                AppSettings.AddOrUpdateValue(nameof(MapType), value);
            }
        }

        public static double Opacity
        {
            get => AppSettings.GetValueOrDefault(nameof(Opacity), 60.0d);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Opacity), value);
            }
        }

        public static int AnimationSpeed
        {
            get => AppSettings.GetValueOrDefault(nameof(AnimationSpeed), 2);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(AnimationSpeed), value);
            }
        }

        public static int AnimationDuration
        {
            get => AppSettings.GetValueOrDefault(nameof(AnimationDuration), 120);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(AnimationDuration), value);
            }
        }

        //minuti di intervallo tra le notifiche
        public static int Interval
        {
            get => AppSettings.GetValueOrDefault(nameof(Interval), 30);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Interval), value);
            }
        }

        public static double DNDFrom
        {
            get => AppSettings.GetValueOrDefault(nameof(DNDFrom), 0.0);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(DNDFrom), value);
            }
        }

        public static double DNDTo
        {
            get => AppSettings.GetValueOrDefault(nameof(DNDTo), 0.0);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(DNDTo), value);
            }
        }

        public static bool DNDEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(DNDEnabled), false);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(DNDEnabled), value);
            }
        }

        public static string Sound
        {
            get => AppSettings.GetValueOrDefault(nameof(Sound), "thunder1.mp3");
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Sound), value);
            }
        }


	}
}