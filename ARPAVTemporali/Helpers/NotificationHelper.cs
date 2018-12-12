using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ARPAVTemporali.Models;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Newtonsoft.Json;
using Plugin.Connectivity;
using SQLite;
using Xamarin.Forms;

namespace ARPAVTemporali.Helpers
{
    public class NotificationHelper
    {

        private string _userID;
        private bool initializationDone = false;

        public NotificationHelper()
        {
            MessagingCenter.Subscribe<UserSettings,bool>(this, Events.NotificationEnabledValueChanged, OnNotificationEnabledValueChanged);
        }


        public async Task Activate()
        {
            OneSignal.Current.StartInit("8ad1c4ec-bf0d-422a-bcfe-429c4cac7544")
                    .HandleNotificationReceived(HandleNotificationReceived)
                    .HandleNotificationOpened(HandleNotificationOpened)
                    .EndInit();
            initializationDone = true;
            
            await RegisterUser();
        }

        private async void OnNotificationEnabledValueChanged(UserSettings sender, bool active)
        {
            if (!initializationDone && active)
                await Activate();
            
            OneSignal.Current.SetSubscription(active);
        }



        //registra l'utente sull'application server
        private async Task RegisterUser()
        {
            string USER_ID = await UserID();
            Debug.WriteLine("utente registrato con ID: "+ USER_ID);
            string UtcOffsetHours = TimeZoneInfo.Local.BaseUtcOffset.Hours.ToString();
            //TODO: mandare anche questi due come json
            var platform = Device.RuntimePlatform; //iOS, android...
            var idiom = Device.Idiom; //phone, tablet,...


            string URL = string.Format(Variables.ApplicationServerURL + "/app/register/{0}", USER_ID);
            HttpClient _client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), URL);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            List<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>>();
            nameValueCollection.Add(new KeyValuePair<string, string>("Timezone", UtcOffsetHours));
            nameValueCollection.Add(new KeyValuePair<string, string>("Platform", platform));
            nameValueCollection.Add(new KeyValuePair<string, string>("Idiom", idiom.ToString()));
            request.Content = new FormUrlEncodedContent(nameValueCollection);
            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                HttpContent httpContent = response.Content;
                string responseString = await httpContent.ReadAsStringAsync();
                Debug.WriteLine("utente registrato: " + responseString);
            }
            catch (Exception ex)
            {
                string errorType = ex.GetType().ToString();
                string errorMessage = errorType + ": " + ex.Message;
                Debug.WriteLine("errore durante la registrazione dell'utente: " + errorMessage);
                //throw new Exception(errorMessage, ex.InnerException);
            }
        }

        public Task<string> UserID()
        {
            TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
            if (_userID != null)
            {
                taskCompletionSource.SetResult(_userID);
            }
            else
            {
                OneSignal.Current.IdsAvailable((string playerID, string pushToken) =>
                {
                    _userID = playerID;
                    taskCompletionSource.SetResult(_userID);
                });
            }
            return taskCompletionSource.Task;
        }

        public Task<List<Notification>> GetNotificationsTask()
        {
            var tcs = new TaskCompletionSource<List<Notification>>();

            OneSignal.Current.IdsAvailable(async (string userID, string pushToken) => {
                List<Notification> list = await GetNotificationsAsync();
                tcs.SetResult(list);
            });
            return tcs.Task;
        }

        private async Task<List<Notification>> GetNotificationsAsync()
        {
            List<Notification> notifications = await DatabaseHelper.GetNotifiche();

            if (CrossConnectivity.Current.IsConnected)
            {
                string USER_ID = await UserID();
                //USER_ID = "0921e648-b44d-40cb-b10e-f6df8683eb28";
                HttpClient _client = new HttpClient();
                string URL = string.Format(Variables.ApplicationServerURL + "/public/notifications/{0}", USER_ID);

                try
                {
                    Debug.WriteLine("start fetching notifications");
                    var request = new HttpRequestMessage(HttpMethod.Get, URL);
                    var response = await _client.SendAsync(request); //assicurarsi di abilitare il permesso a usare internet in android->options->android application->required permissions

                    // scarico le notifiche dal server
                    response.EnsureSuccessStatusCode();
                    var jsonString = await response.Content.ReadAsStringAsync();
                    List<JSON_Notification> JSON_notification_list = JsonConvert.DeserializeObject<List<JSON_Notification>>(jsonString);

                    /*
                     * controllo ogni notifica scaricata
                     * ogni notifica contiene una lista di dettagli
                     */
                    foreach (JSON_Notification JSON_notification in JSON_notification_list)
                    {

                        // scorro tra la lista di dettagli dennla notifica
                        List<JSON_Notification.Detail> notificationList = JSON_notification.data;
                        foreach (var detail in notificationList)
                        {
                            // usando l'ID controllo se la notifica è già presente nel database
                            string id = string.Join("_", new List<string> { JSON_notification._key, detail.comune });
                            Notification match = notifications.FirstOrDefault(_notification => _notification.notification_ID == id);
                            if (match == null)
                            {
                                //cancello le notifiche superiori al amssimo consentito
                                /*if (notifications.Count() < _maxNotitifactions)
                                {
                                }else {
                                    await DeleteNotificationAsync(JSON_notification._key); //cancello le notifiche superiori al amssimo consentito
                                }*/

                                //aggiungo la notifica nel database
                                Notification newNotification = new Notification
                                {
                                    notification_ID = id,
                                    Comune = detail.comune,
                                    Data = detail.data, //Convert.ToDateTime(detail.data),
                                    Distanza = detail.distanza,
                                    Key = JSON_notification._key,
                                    IsRead = false

                                };
                                notifications.Add(newNotification);
                                await DatabaseHelper.Insert(newNotification);
                            }
                            else
                            {
                                // aggiorno l'identificativo della notifica
                                match.Key = JSON_notification._key;
                            }

                        }
                    }
                    //await DatabaseConnection.InsertAsync(newNotification);
                    Debug.WriteLine("notifications fetched");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error fetching notifications: " + ex.Message);
                    string errorType = ex.GetType().ToString();
                    string errorMessage = errorType + ": " + ex.Message;
                    //throw new Exception(errorMessage, ex.InnerException);
                }

            }
            // prendo dal database le ultime 20 notifiche
            notifications = notifications.OrderByDescending(n => n.Data).Take(20).ToList();
            // amndo una notifica al sistema con le ultime 20 notifiche
            MessagingCenter.Send(this, Events.NewNotificationReceived, notifications);
            //TODO: cancellare le notitiche superiori alla numero 20
            return notifications;
        }

        public async Task DeleteNotificationAsync(string key)
        {
            string URL = string.Format(Variables.ApplicationServerURL + "/public/notifications/{0}", key);
            HttpClient _client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("DELETE"), URL);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = JsonConvert.SerializeObject(null); //non passo niente

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                HttpContent httpContent = response.Content;
                string responseString = await httpContent.ReadAsStringAsync();
                Debug.WriteLine("dati aggiornati: " + responseString);
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
         * notifica ricevuta quando l'app è aperta
         */
        private void HandleNotificationReceived(OSNotification notification)
        {
            GetNotificationsTask();
        }

        /*
         * utente ha premuto sull'avviso della notifica
         */
        private void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            GetNotificationsTask();
        }
    }
}
