using System;
using System.Collections.Generic;
using System.Diagnostics;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using Xamarin.Forms;

namespace ARPAVTemporali.Views
{
    public partial class NotificationDetailPage : ContentPage
    {
		//usando una proprietà posso accedere alle nested properties in XAML
		public Notification Notification { get; set; }
        public FormattedString TestoRilevamento { get; set; }

        public NotificationDetailPage(Notification notification)
        {
            Notification = notification;

            TestoRilevamento = new FormattedString();
            TestoRilevamento.Spans.Add(new Span { Text = Localization.AppResources.Notifiche_RilevatoFenomeno });
            TestoRilevamento.Spans.Add(new Span { Text = (" " + Notification.Distanza.ToString() + " "), FontAttributes = FontAttributes.Bold });
            TestoRilevamento.Spans.Add(new Span { Text = Localization.AppResources.Notifiche_ChilometriDalCapoluogo });


            InitializeComponent();

            BindingContext = this;
        }

		//segna la notifica come letta
        private async void UpdateNotificationStatus()
        {
            Notification.IsRead = true;
            await DatabaseHelper.Update(Notification);

            App app = Application.Current as App;
            MessagingCenter.Send(app, Events.NotificationUpdated, Notification);
        }

        protected override void OnAppearing()
        {
            UpdateNotificationStatus();
            base.OnAppearing();
        }
    }
}
