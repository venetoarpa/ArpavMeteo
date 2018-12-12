using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ARPAVTemporali.Views
{
    public partial class SpashScreenPage : ContentPage
    {
        public SpashScreenPage()
        {
            InitializeComponent();

            CheckInitialSettings();
        }

		async void CheckInitialSettings()
		{
			var app = Application.Current as App;

			//Debug.WriteLine("language",app.Language);
			//Debug.WriteLine("disclaimer read?",app.DisclaimerRead);

			/*
             * subscribe to messaging center
             * do whatever you want in the modal
             * unsubscribe when the message has been handled
             * NOTA BENE: non mi serve davvero, ma l'ho messo per ricordarmi come funziona
             * per aspettare i risultati uso i task
             */
			//MessagingCenter.Subscribe<Views.Settings.LanguageModalPage, string>(this, Events.LanguageSelectedLabel, OnLanguageSelected);

			//Debug.WriteLine("checking language");
			if (app.Language.Equals(""))
			{
				Views.Settings.LanguageModalPage langaugePage = new Views.Settings.LanguageModalPage();
				await Navigation.PushModalAsync(langaugePage);
				await langaugePage.LanguageSelectedTask;
			}

			if (app.DisclaimerRead == false)
			{
				Views.DisclaimerPage disclaimerPage = new Views.DisclaimerPage();
				await Navigation.PushModalAsync(disclaimerPage);
				await disclaimerPage.DisclaimerReadTask;
			}

			app.Reload();

			//Debug.WriteLine("language " + app.Language);
			//Debug.WriteLine("disclaimer read " + (app.DisclaimerRead ? "true" : "false"));

		}
    }
}
