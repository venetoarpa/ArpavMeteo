using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ARPAVTemporali.Views.SettingsPages
{

    public partial class LanguagePage : ContentPage
    {
		public ICommand TapCommand { get; private set; }

        public LanguagePage()
        {
            BindingContext = this;

            TapCommand = new Command<string>(SetLanguageSetting);

			InitializeComponent();

        }

        public async void SetLanguageSetting(string language) {
			Helpers.Settings.Language = language;

            var app = Application.Current as App;
            app.Start();
            //Navigation.PopAsync();
        }

    }
}
