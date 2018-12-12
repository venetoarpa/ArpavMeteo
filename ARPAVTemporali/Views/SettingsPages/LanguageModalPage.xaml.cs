using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ARPAVTemporali.Views.SettingsPages
{
	public partial class LanguageModalPage : ContentPage
	{
		// Use this to wait on the page to be finished with/closed/dismissed
		// https://stackoverflow.com/questions/24174241/how-can-i-await-modal-form-dismissal-using-xamarin-forms#answer-38410698
		public Task LanguageSelectedTask { get { return tcs.Task; } }

		private TaskCompletionSource<string> tcs { get; set; }

        public ICommand TapCommand { get; private set; }

		public LanguageModalPage()
		{

			BindingContext = this;

            TapCommand = new Command<string>(async (language)=> await SetLanguageSetting(language));

			tcs = new System.Threading.Tasks.TaskCompletionSource<string>();

			InitializeComponent();
		}

		private async Task SetLanguageSetting(string language)
		{
			Helpers.Settings.Language = language;
			await Navigation.PopModalAsync();
			tcs.SetResult(language);
		}
	}
}
