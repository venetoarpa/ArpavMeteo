using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

using ARPAVTemporali.Controls;

namespace ARPAVTemporali.Views
{

    public partial class SettingsPage1 : ContentPage
    {
        private List<string> settings = new List<string>
        { 
            
        };

        private List<ComuneSettings>comuni = new List<ComuneSettings>
		{
			new ComuneSettings { Name="comune 1", Intensity=2.0d, Range=10.5d },
			new ComuneSettings { Name="comune 2", Intensity=6.0d, Range=15.5d },
			new ComuneSettings { Name="comune 3", Intensity=20.0d, Range=12.5d },
		};
        private List<Strumento> strumenti = new List<Strumento>
		{
            new Strumento { Name="Loncon", Active=false },
    		new Strumento { Name="Teolo", Active=false },
            new Strumento { Name="Valeggio", Active=false },
		};
        public SettingsPage1()
        {
            InitializeComponent();



            printComuni();
            printStrumenti();
            Slider slider = new Slider { Value = 20.5d, Minimum = 0.0d, Maximum = 50.0d };
            comuniLayout.Children.Add(slider);
        }

        private void printComuni()
        {
			Label titleLabel = new Label { Text = "Notifiche", HorizontalOptions = LayoutOptions.Center };
            comuniLayout.Children.Add(titleLabel);
			
			foreach (var comune in comuni)
			{
				StackLayout layout = new StackLayout
				{
					Orientation = StackOrientation.Vertical
				};
                Label comuneLabel = new Label { Text = comune.Name, HorizontalOptions = LayoutOptions.StartAndExpand,  };
				var tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += async (s, e) => {
                    // handle the tap
                    await Navigation.PushAsync(new ComuniPage(), true);
				};
                comuneLabel.GestureRecognizers.Add(tapGestureRecognizer);

                layout.Children.Add(comuneLabel);

                // INTENSITÁ
                SliderOptionControl intensitySlider = new SliderOptionControl { LabelText = "Intensità", SliderValue = comune.Intensity.ToString() };
                intensitySlider.SliderValueChanged += (object sender, double e) => { comune.Intensity = e; }; //aggiorno il valore del comune quando cambia lo slider
				layout.Children.Add(intensitySlider);

                // RAGGIO
                SliderOptionControl rangeSlider = new SliderOptionControl { LabelText = "Raggio", SliderValue = comune.Range.ToString() };
                rangeSlider.SliderValueChanged += (object sender, double e) => { comune.Range = e; }; //aggiorno il valore del comune quando cambia lo slider
				layout.Children.Add(rangeSlider);

				comuniLayout.Children.Add(layout);
			}
        }

		private void printStrumenti()
		{
            Label titleLabel = new Label { Text = "Strumenti", HorizontalOptions = LayoutOptions.Center };
			strumentiLayout.Children.Add(titleLabel);

			foreach (var strumento in strumenti)
			{
				StackLayout layout = new StackLayout
				{
					Orientation = StackOrientation.Horizontal
				};
                layout.Children.Add(new Label { Text = strumento.Name, HorizontalOptions = LayoutOptions.StartAndExpand });
                Switch switchStrumento = new Switch();
                switchStrumento.Toggled += (object sender, ToggledEventArgs e) => { strumento.Active = e.Value; };
				layout.Children.Add(switchStrumento);
				strumentiLayout.Children.Add(layout);
			}
		}

        protected override void OnDisappearing()
        {
            /*foreach (var comune in comuni)
            { 
                Debug.WriteLine(comune.Intensity);
            }
			foreach (var strumento in strumenti)
			{
                Debug.WriteLine(strumento.Active);
			}*/
            base.OnDisappearing();
        }
    }
}
