using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace ARPAVTemporali.Controls
{
    public partial class HeaderControl : ContentView
    {

        public HeaderControl()
        {
            InitializeComponent();

            BindingContext = this; //importantissimo!!!

        }

		// IsHome
		public bool IsHome
		{
			get {
                if (GetValue(IsHomeProperty) == null)
                    return false;

                return (bool)GetValue(IsHomeProperty);
            }
			set { SetValue(IsHomeProperty, value); }
		}

		public static readonly BindableProperty IsHomeProperty = BindableProperty.Create(
            propertyName: nameof(IsHome),
             returnType: typeof(bool),
             declaringType: typeof(HeaderControl),
             defaultValue: false,
			 defaultBindingMode: BindingMode.TwoWay
        );

		public async void OnSettingsButtonPressed(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new Views.SettingsPage());
		}

		public async void OnBackButtonPressed(object sender, EventArgs e)
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}

		public void OnLogoRegioneButtonPressed(object sender, EventArgs e)
        {
            GoToAbout();
		}

		public async void OnLogoARPAVButtonPressed(object sender, EventArgs e)
		{
            await Navigation.PopToRootAsync();
		}

        private async void GoToAbout()
        {
            /*
             * vai alla pagina about se non ci sei già
             */
			int index = Navigation.NavigationStack.Count - 1;
			Page currentPage = Navigation.NavigationStack[index];

			if (!currentPage.GetType().Equals(typeof(Views.AboutPage)))
				await Navigation.PushAsync(new Views.AboutPage());
        }
	}
}
