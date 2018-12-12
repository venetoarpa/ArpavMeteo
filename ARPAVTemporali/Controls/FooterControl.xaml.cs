using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using ARPAVTemporali.ViewModels;
using Xamarin.Forms;

namespace ARPAVTemporali.Controls
{
    public partial class FooterControl : ContentView
    {
        public ICommand TapCommand { get; private set; }
        public int UnreadCount { get; set; }

        public FooterControl()
        {
            UnreadCount = 0;
            BindingContext = this;

            TapCommand = new Command<string>(GoToPagina);

            InitializeComponent();
            MessagingCenter.Subscribe<NotificationHelper, List<Notification>>(this, Events.NewNotificationReceived, Handle_NotificationReceived);
            MessagingCenter.Subscribe<App, Notification>(this, Events.NotificationUpdated, Handle_NotificationUpdated);
            MessagingCenter.Subscribe<App>(this, Events.CheckNotificationCount, CheckNotificationCount);
        }

        private async Task UpdateNotificationsCount()
        {
            await DatabaseHelper.RimuoviVecchieNotifiche();

            UnreadCount = await DatabaseHelper.GetTotalUnreadNotification();
            
            notificationBadge.IsVisible = UnreadCount > 0;
            notificationBadge.Text = UnreadCount.ToString();
        }

        async void CheckNotificationCount(App source)
        {
            await UpdateNotificationsCount();
        }

        async void Handle_NotificationUpdated(App source, Notification notification)
        {
            await UpdateNotificationsCount();
        }

        async void Handle_NotificationReceived(NotificationHelper notificationManager, List<Notification> notifications)
        {
            await UpdateNotificationsCount();
        }

        void Handle_LayerTapped(object sender, System.EventArgs e)
        {
            MessagingCenter.Send(this, Events.ToggleLayerMenu); //send playing false
        }

        public void EnableShareButton(bool enabled)
        {
            shareButton.IsEnabled = enabled;
        }

        void Handle_ShareTapped(object sender, System.EventArgs e)
        {
            EnableShareButton(false); //disabilito il pulsante per evitare tap multipli
            MessagingCenter.Send(this, Events.ShareMap); //share the map and it's overlays
        }

        async void GoToPagina(string typeName)
		{
			ContentPage page;

			switch (typeName)
			{
				default:
					Type elementType = Type.GetType("ARPAVTemporali." + typeName);
					page = (ContentPage)Activator.CreateInstance(elementType);
					break;
			}
			try
			{
				await Navigation.PushAsync(page as ContentPage, true);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}

            /*prevent jump effect on iOS
             * see https://stackoverflow.com/a/38146688/1875109
             * https://bugzilla.xamarin.com/show_bug.cgi?id=32830
             */
            //NavigationPage.SetHasNavigationBar(page, false); //hide the navigation bar to prevent the jump effect
			//NavigationPage.SetHasNavigationBar(page, true); //show the navigation bar after page has changed
		}
    }
}
