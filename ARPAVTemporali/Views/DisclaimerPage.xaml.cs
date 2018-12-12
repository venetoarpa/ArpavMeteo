using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.ViewModels;
using System.Diagnostics;

namespace ARPAVTemporali.Views
{
    public partial class DisclaimerPage : ContentPage
    {
		// Use this to wait on the page to be finished with/closed/dismissed
		// https://stackoverflow.com/questions/24174241/how-can-i-await-modal-form-dismissal-using-xamarin-forms#answer-38410698
		public Task DisclaimerReadTask { get { return tcs.Task; } }

		private TaskCompletionSource<bool> tcs { get; set; }

        private DisclaimerViewModel vm;

        public DisclaimerPage()
        {
			InitializeComponent();
            vm = new DisclaimerViewModel();
            BindingContext = vm;

			RegisterWebViewEvents();

            RegisterOkPressed();

        }

        /*
         * continua se l'utente preme ok
         */
        private void RegisterOkPressed()
        {
            tcs = new TaskCompletionSource<bool>();

            okButton.Clicked += async (object sender, EventArgs e) =>
            {
                Helpers.Settings.DisclaimerRead = true; // letto il disclaimer
                tcs.SetResult(true);
                await Navigation.PopModalAsync();
            };
        }

		protected async override void OnAppearing()
		{
            await vm.LoadData();
            base.OnAppearing();
		}

        private void RegisterWebViewEvents()
        {
            webview.Navigating += (s, e) =>
            {
                if (e.Url.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        var uri = new Uri(e.Url);
                        Device.OpenUri(uri);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    e.Cancel = true;
                }
            };


        }
	}
}
