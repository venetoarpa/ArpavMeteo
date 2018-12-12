using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using Xamarin.Forms;

namespace ARPAVTemporali.Views
{
    public partial class NotificationListPage : ContentPage
    {
        ObservableCollection<Notification> _notifications;

        App app;

        public NotificationListPage()
        {

            InitializeComponent();

			//MessagingCenter.Subscribe<App, Notification>(this, Events.NewNotificationReceivedLabel, Handle_NewNotificationReceived);

            BindingContext = this;
        }

		protected async override void OnAppearing()
		{

            // get notifications from database
            app = Application.Current as App;
            await app.NotificationManager.GetNotificationsTask();

            /*Debug.WriteLine("before fetching");
			await app.NotificationManager.GetNotificationsAsync();
            Debug.WriteLine("after fetching");*/
            List<Notification> notifications = await DatabaseHelper.GetNotifiche();
            _notifications= new ObservableCollection<Notification>(notifications);

            listview.ItemsSource = _notifications;
			base.OnAppearing();


		}


        void Handle_NewNotificationReceived(App source, Notification notification)
        {
            _notifications.Add(notification);
        }

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var setting = e.SelectedItem as SettingPage;
			((ListView)sender).SelectedItem = null; //unselect
    		/*
    		 * attenzione: con custom renderers e.SelectedItem potrebbe risultare null
    		 */
		}

		void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
            var notification = e.Item as Notification;
			//DisplayAlert("Tapped", setting.Title, "Ok");

            Navigation.PushAsync(new NotificationDetailPage(notification));
		}

        /*
         * TODO: far funzionare il remove anche dopo aver filtrato la lista
         */
        public async void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            Notification notification = mi.CommandParameter as Notification;

			_notifications.Remove(notification); //optimistic update
            await DatabaseHelper.Delete(notification); //remove from local database
            await app.NotificationManager.DeleteNotificationAsync(notification.Key); //remove from server

        }
    }
}
