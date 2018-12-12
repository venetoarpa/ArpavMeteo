using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;

namespace ARPAVTemporali.Controls
{
    public partial class LayerMenuControl : ContentView
    {
        public ICommand TapCommand { get; private set; }

        private TintTransformation _activeTransformation = new TintTransformation
        {
            HexColor = "#ffffffff",
            EnableSolidColor = true,
        };
        private TintTransformation _normalTransformation = new TintTransformation
        {
            HexColor = "#ff000000",
            EnableSolidColor = true,
        };

        public LayerMenuControl()
        {
            App app = Application.Current as App;

            TapCommand = new Command<string>(Handle_LayerButtonTapped);

            BindingContext = this;
            InitializeComponent();

			UpdateButtonsOpacity();
        }


        private  void Handle_LayerButtonTapped(string val)
        {
            switch(val)
            {
                case "fulmini":
                    Settings.LayerFulmini = !Settings.LayerFulmini;
                    break;
                case "mosaico":
                    Settings.LayerMosaico = !Settings.LayerMosaico;
                    break;
            }

            UpdateButtonsOpacity();
            MessagingCenter.Send(this, Events.ActiveOverlaysValueChangedLabel);
            //_userSettings.Overlay = value;
            //SetVisibleOverlays(value);
        }

        private void UpdateButtonsOpacity()
        {
            if(Settings.LayerMosaico)
            {
				mosaicoLayerButton.Transformations = new List<ITransformation>() { _activeTransformation };
            }else
            {
                mosaicoLayerButton.Transformations = new List<ITransformation>() { _normalTransformation };
            }

            if(Settings.LayerFulmini)
            {
                fulminiLayerButton.Transformations = new List<ITransformation>() { _activeTransformation };
            }
            else
            {
                fulminiLayerButton.Transformations = new List<ITransformation>() { _normalTransformation };
            }
        }
    }
}
