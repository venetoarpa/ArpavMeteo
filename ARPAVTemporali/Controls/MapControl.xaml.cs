using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.IO;
using ARPAVTemporali.Models;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ARPAVTemporali.ViewModels;
using ARPAVTemporali.Views;
using ARPAVTemporali.Localization;
using PCLStorage;
using Plugin.Connectivity;
using ARPAVTemporali.Helpers;

namespace ARPAVTemporali.Controls
{
    public partial class MapControl : ContentView
    {
        private MapViewModel MapManager;
        //Map map;
        private double _animationSpeed;
        //private float _overlayOpacity;

        private GroundOverlay _overlayMosaico; //riferimento all'overlay con il mosaico
        private GroundOverlay _overlayFulmini; //riferimento all'overlay con i fulmini
        private int _imagesToLoad = 0;
        private int _overlaysCount = 0;
		private int _counter = 0;
        private DateTime _lastFetchOperation; //memorizza l'ultimo caricamento delle immagini

        private bool _isPlaying = true;

        private string _dataOverlay = "";
        public string DataOverlay {
            get {
                return _dataOverlay;
            }
            set{
                _dataOverlay = value;
                OnPropertyChanged(nameof(DataOverlay)); //notifico il cambiamento alla UI
            }
        }

        public UserSettings UserSettings { get => Settings.UserSettings;  }

        public MapControl()
        {
            /*CancellationToken token = new CancellationToken();
            TimeSpan interval = new TimeSpan(123);
            PeriodicFooAsync(interval, token);*/

            BindingContext = this;

            InitializeComponent();

            _overlayMosaico = new GroundOverlay()
            {
                Bounds = Variables.BoundingBoxMosaico,
                //Icon = BitmapDescriptorFactory.FromBundle("image01.png"),
                //Icon = myImage,
                Transparency = 0.5f,
                Tag = "Radar",
                ZIndex = 1,
            };
            _overlayFulmini = new GroundOverlay()
            {
                Bounds = Variables.BoundingBoxFulmini,
                //Icon = BitmapDescriptorFactory.FromBundle("image01.png"),
                //Icon = myImage,
                Transparency = 0.5f,
                Tag = "Fulmini",
                ZIndex = 2,
            };
            //SetVisibleOverlays(_userSettings.Overlay);
            SetActiveOverlays();
			MapManager = new MapViewModel(map); //gestisce i caricamenti

            /*
             * TODO: fare in modo che se l'animazione era in corso riprenda dopo qualche ms
             * che non avviene un cameraChanged usando una Task
             */
            map.CameraIdled += (object sender, CameraIdledEventArgs e) => {
                if (_isPlaying)
                    Pause();
            };

            //MessagingCenter.Subscribe<MapControl, bool>(this, Events.MapPlayingStatusChangedLabel, OnMapPlayingStatusChanged);
            MessagingCenter.Subscribe<FooterControl>(this, Events.ToggleLayerMenu, Handle_ToggleLayerMenu);
            MessagingCenter.Subscribe<FooterControl>(this, Events.ShareMap, ShareMap);
            MessagingCenter.Subscribe<App>(this, Events.OnSleep, Handle_OnSleep);
            MessagingCenter.Subscribe<LayerMenuControl>(this, Events.ActiveOverlaysValueChangedLabel, Handle_ActiveOverlaysValueChanged);
            MessagingCenter.Subscribe<MapViewModel, bool>(this, Events.FetchOverlaysDone, Handle_FetchOverlaysDone);
        }


        private void Handle_FetchOverlaysDone(MapViewModel obj, bool success)
        {
            if(!success)
                DataOverlay = "impossibile connettersi al server";
        }

        private void Handle_OnSleep(App source)
        {
            Pause(); //pause the map when sleeping
        }


        private async void Handle_ToggleLayerMenu(object source)
        {

            List<string> mapTypes = Variables.MapTypes.Keys.ToList();
            string answer = await Application.Current.MainPage.DisplayActionSheet(
                AppResources.MapControl_ScegliLayer,
                AppResources.MapControl_Annulla,
                null,
                mapTypes.ElementAt(0),
                mapTypes.ElementAt(1),
                mapTypes.ElementAt(2),
                mapTypes.ElementAt(3));
            
            string mapType;
            if(answer != null && Variables.MapTypes.TryGetValue(answer, out mapType))
            {
				UserSettingsViewModel vm = new UserSettingsViewModel();
                vm.UpdateSettings("MapType", mapType);
                MapManager.SetMapType();
			}

        }

        List<OverlayStream> _overlayStreams = new List<OverlayStream>();

		public async Task FetchOverlaysTask(int totalImages)
		{
            if (CrossConnectivity.Current.IsConnected)
            {
                DataOverlay = "loading...";
                _overlayStreams = await MapManager.GetStreamsAsync(totalImages);

                _overlaysCount = _overlayStreams.Count;
                _lastFetchOperation = DateTime.Now; //registro data e ora dell'ultima operazione
                //cleanup
                MapManager.CleanupOverlays();
			}
		}

        private void UpdateButtons()
        {
			var activeTransformation = new TintTransformation
			{
				HexColor = "#ff000000",
				EnableSolidColor = true,
			};
			var disabledTransformation = new TintTransformation
			{
				HexColor = "#66000000",
				EnableSolidColor = true,
			};

            Device.BeginInvokeOnMainThread(() =>
            {
                prevButton.IsEnabled = !_isPlaying;
                nextButton.IsEnabled = !_isPlaying;

                if(_isPlaying)
                {
                    playButtonImage.Source = Xamarin.Forms.ImageSource.FromResource("ARPAVTemporali.Images.Icons.MapControls.stop-128.png");
                    prevButtonImage.Transformations = new List<ITransformation>() { disabledTransformation };
                    nextButtonImage.Transformations = new List<ITransformation>() { disabledTransformation };
                }else {
                    playButtonImage.Source = Xamarin.Forms.ImageSource.FromResource("ARPAVTemporali.Images.Icons.MapControls.play-128.png");
                    prevButtonImage.Transformations = new List<ITransformation>() { activeTransformation };
                    nextButtonImage.Transformations = new List<ITransformation>() { activeTransformation };

                }
			});
        }

		async void Handle_Reload(object sender, System.EventArgs e)
		{
            Pause();
            _counter = 0;
            await FetchOverlaysTask(_imagesToLoad);
            Play();
		}

        public void Play()
        {
            _isPlaying = true;

            StartAnimation();
			UpdateButtons();
        }

        TimerHelper _timer;

        private void StartAnimation()
        {
            SetOverlayImage();

            if (_timer == null) {
				var interval = TimeSpan.FromMilliseconds((int)_animationSpeed);
                _timer = new TimerHelper(interval, Next);
                _timer.Start();
            } else {
                _timer.Stop(); _timer.Start();
            }
        }

        public void Pause()
        {
            _isPlaying = false;
            if (_timer != null)
                _timer.Stop();
			UpdateButtons();
		}

        void Handle_PlayPause(object sender, System.EventArgs e)
        {
            if(_isPlaying)
                Pause();
            else
                Play();
        }

        void Handle_Prev(object sender, System.EventArgs e)
        {
            Prev();
        }

		void Handle_Next(object sender, System.EventArgs e)
		{
            Next();
		}

		private void Prev()
		{
            if (_overlaysCount < 1)
                return;
            
			if (--_counter < 0)
                _counter = _overlaysCount-1;
            
			SetOverlayImage();
            UpdateButtons();
		}

        private void Next()
        {
            if (_overlaysCount < 1)
                return;
            
			if (++_counter >= _overlaysCount)
				_counter = 0;
            
            SetOverlayImage();
            UpdateButtons();
        }


        private float GetPlatformTranparency(float val = 0f)
        {
            //if (Device.RuntimePlatform == Device.Android)
                val = 100.0f - val;

            return val / 100f;
        }

        private float CalculateOverlayOpacity()
        {
            float opacity = (float)UserSettings.Opacity; //imposto l'opacità dell'overlay in base alle preferenze

            return GetPlatformTranparency(opacity);
        }

        private void Handle_ActiveOverlaysValueChanged(object source)
        {
            SetActiveOverlays();
        }

        private void SetActiveOverlays()
        {
            float opacity = CalculateOverlayOpacity(); // l'opacità impostata dall'utente
            float transparency = GetPlatformTranparency(0f); //1 è totalmente trasparente su android


            _overlayMosaico.Transparency = Settings.LayerMosaico ? opacity : transparency;
            _overlayFulmini.Transparency = Settings.LayerFulmini ? opacity : transparency;

            scalaAbsolute.IsVisible = Settings.LayerMosaico; //mostra/nascondi la legenda
        }

        private void SetOverlayImage()
        {

            if (_overlaysCount <= 0) return;

            GC.Collect(); //forza il garbage collector a fare il suo dovere
            var currentOverlay = _overlayStreams.ElementAt(_counter);

            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Stream mosaicoStream = currentOverlay.MosaicoStream;
                    _overlayMosaico.Icon = BitmapDescriptorFactory.FromStream(mosaicoStream);
                    Stream fulminiStream = currentOverlay.FulminiStream;
                    _overlayFulmini.Icon = BitmapDescriptorFactory.FromStream(fulminiStream);


                    map.GroundOverlays.Clear();

                        map.GroundOverlays.Add(_overlayMosaico);

                        map.GroundOverlays.Add(_overlayFulmini);
                    
					DateTime data = currentOverlay.Date;

                    int UtcOffsetHours = TimeZoneInfo.Local.BaseUtcOffset.Hours; //cambio in base a fuso orario
                    if( TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now) )
                        data.AddHours(-1); //cambio in base a ora solare

                    DataOverlay = data.ToString("dd/MM/yyyy HH:mm");
				});
			}
			catch (Exception ex)
			{
                Debug.WriteLine("SetOverlayImage error: " + ex.Message);
			}

			/*_overlay.Clicked += (sender, e) =>
            {
                var overlay = sender as GroundOverlay;
                Debug.WriteLine("Clicked", overlay.Tag as string, "CLOSE");
            };*/
			//overlay_tcs.SetResult("done");
        }

        public async void InitMap()
        {
            SetActiveOverlays();
            //_overlayOpacity = CalculateOverlayOpacity();
            _animationSpeed = UserSettings.AnimationSpeed * 1000;
            _imagesToLoad = UserSettings.AnimationDuration / 10; //1 immagine ogni 10 minuti

            //fine operazioni preliminari

            MapManager.SetMapType(); //imposta il MapType in base alle preferenze dell'utente
            MapManager.PrintRaggiComuni(); //add pins to the map
            await TryFetchOverlays();

            _counter = 0;
            _counter =_overlayStreams.Count-1; //parto dall'ultimo fotogramma

			Play();
            Pause();//metto subito in pausa
        }

        //carica gli overlay se ci sono le condizioni giuste
        private async Task TryFetchOverlays()
        {
            playButton.IsEnabled = false; //disabilita il pulsante mentre carica i dati

            // carica gli overlay solo se sono passati 2 minuti (120 secondi) dall'ultimo caricamento
            DateTime now = DateTime.Now;
            double elapsedSeconds = now.Subtract(_lastFetchOperation).TotalSeconds;
            bool changedTotalOverlays = _overlayStreams.Count != _imagesToLoad; //controlla se l'utente vuole un numero diverso di overlay
            if (changedTotalOverlays || elapsedSeconds > 120d )
                await FetchOverlaysTask(_imagesToLoad);
            
            playButton.IsEnabled = true; //riabilita il pulsante
        }


        public async void ShareMap(FooterControl source)
        {
            /*
             * passo lo il viewmodel nel costruttore della pagina
             * se lo salvo in una variabile e lo passo al costruttore il garbage collector lo elimina
             *
             */
            var vm = new ShareViewModel();

            var cachedImageStream = await map.TakeSnapshot();
            vm.MyCachedImage.Source = Xamarin.Forms.ImageSource.FromStream(() => cachedImageStream);

            var page = new SharePage(vm);
            await Navigation.PushAsync(page);
            source.EnableShareButton(true); //riabilita il pulsante per la condivisione
		}

    }
}
