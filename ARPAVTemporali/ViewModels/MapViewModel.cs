using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using FFImageLoading;
using Newtonsoft.Json;
using PCLStorage;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace ARPAVTemporali.ViewModels
{
    public class MapViewModel
    {
        private List<string> _availableOverlays = new List<string> { "mosaico","fulmini" };
        private Map _map;

        public MapViewModel(Map map = null)
        {
            _map = map;
            SetupMap();
        }

        private void SetupMap()
        {
            /*
            * Map settings
            */
            _map.UiSettings.MyLocationButtonEnabled = false;
            //map.IsShowingUser = true; // obsolete, but working on iOS
            _map.UiSettings.TiltGesturesEnabled = true;
            _map.UiSettings.ZoomGesturesEnabled = true;
            //map.UiSettings.ZoomControlsEnabled = false;
            _map.UiSettings.CompassEnabled = true;
            _map.MyLocationEnabled = true;

            //posizione iniziale
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(
                Variables.InitialPosition,  // latlng
                Variables.InitialMapZoom, // zoom
                0d, // rotation
                0d)); // tilt

            _map.InitialCameraUpdate = cameraUpdate;

        }

        public async void PrintRaggiComuni()
        {
            ObservableCollection<ComuneSettings> settingsComuni = new ObservableCollection<ComuneSettings>(Settings.Comuni);
            _map.Pins.Clear();
            _map.Circles.Clear();

            List<Comune> comuni = await DatabaseHelper.GetComuni();

            foreach (ComuneSettings settings in settingsComuni)
            {
                Comune comune = comuni.FirstOrDefault(_comune => _comune.Name == settings.Name);
                if (comune != null)
                {
                    /*
                     * aggiungi i pin
                     */
                    Position comunePosition = new Position(comune.lat, comune.lng);
                    var pin = new Pin()
                    {
                        Type = PinType.Place,
                        Label = comune.Name,
                        Address = settings.Range.ToString(),
                        Position = comunePosition,
                        Rotation = 0.0f,
                        Tag = "id_" + comune.Name,
                        ZIndex = 10,
                        IsVisible = true
                    };
                    _map.Pins.Add(pin);

                    /*
                     * aggiungi i raggi
                     */
                    if (settings.ShowRange) //check showRange
                    {
                        double range = settings.Range * 1000;
                        Circle circle = new Circle()
                        {
                            IsClickable = true,
                            Center = comunePosition,
                            Radius = Distance.FromMeters(range),
							StrokeColor = Color.FromHex("ff007267"),
							StrokeWidth = 3f,
							FillColor = Color.FromRgba(0, 0, 255, 32),
							Tag = "CIRCLE", // Can set any object
                            ZIndex = 10,
                        };



                        circle.Clicked += (sender, e) => Debug.WriteLine("Clicked");

                        _map.Circles.Add(circle);
                    }
                }
            }
        }

        public void SetMapType()
        {

            string mapType = Settings.MapType;

            Xamarin.Forms.GoogleMaps.MapType type;
            switch (mapType)
            {
                case "Hybrid":
                    type = Xamarin.Forms.GoogleMaps.MapType.Hybrid;
                    break;
                case "Street":
                    type = Xamarin.Forms.GoogleMaps.MapType.Street;
                    break;
                case "Satellite":
                    type = Xamarin.Forms.GoogleMaps.MapType.Satellite;
                    break;
                case "Terrain":
                    type = Xamarin.Forms.GoogleMaps.MapType.Terrain;
                    break;
                default:
                    type = Xamarin.Forms.GoogleMaps.MapType.Street;
                    break;
            }
            _map.MapType = type;
        }

        // scarica dal server l'elenco degli overlay
        private async Task<List<JSON_Overlay>> FetchOverlaysAsync(int totalImages)
        {
            //_fetchOverlaysTaskCompletionSource = new TaskCompletionSource<Dictionary<string, List<Stream>>>();

            List<JSON_Overlay> overlays = new List<JSON_Overlay>();

            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClient _client = new HttpClient();
                string URL = string.Format(Variables.ApplicationServerURL + "/public/overlays/{0}", totalImages);
                var request = new HttpRequestMessage(HttpMethod.Get, URL);
				try
				{
                    var response = await _client.SendAsync(request); //assicurarsi di abilitare il permesso a usare internet in android->options->android application->required permissions

    				response.EnsureSuccessStatusCode();
    				var jsonString = await response.Content.ReadAsStringAsync();
    				
    				overlays = JsonConvert.DeserializeObject<List<JSON_Overlay>>(jsonString);
                }
                catch (Exception ex)
                {
                    MessagingCenter.Send(this, Events.FetchOverlaysDone, false);
                    Debug.WriteLine("Error fetching overlay images: " + ex.Message);
                }
			}
            MessagingCenter.Send(this, Events.FetchOverlaysDone, true);
            return overlays;
        }

        public async Task<List<OverlayStream>> GetStreamsAsync(int total)
        {
            var overlays = await FetchOverlaysAsync(total);
            List<OverlayStream> streams = new List<OverlayStream>();

            foreach (var overlay in overlays)
            {
                Uri MosaicoImageURI = new Uri(Variables.ApplicationServerURL + overlay.mosaico);
                Uri FulminiImageURI = new Uri(Variables.ApplicationServerURL + overlay.fulmini);
                var streamMosaico = await GetStreamAsync(MosaicoImageURI);
                var streamFulmini = await GetStreamAsync(FulminiImageURI);
                var overlayStream = new OverlayStream()
                {
                    Date = Convert.ToDateTime(overlay.data),
                    MosaicoStream = streamMosaico,
                    FulminiStream = streamFulmini,
                };
                streams.Add(overlayStream);
            }
            return streams;
        }

        // trasforma l'immagine remota in uno stream
        public async Task<Stream> GetStreamAsync(Uri imageUri)
        {
            Stream stream = null;
            try
            {
                //Debug.WriteLine("url mosaico: "+MosaicoImageURI.AbsoluteUri);
                stream = await ImageService.Instance.LoadUrl(imageUri.AbsoluteUri).AsPNGStreamAsync();
                return stream;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Stream creation error: " + ex.Message);
                return stream;
            }
        }

        public async void CleanupOverlays()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("overlays", CreationCollisionOption.OpenIfExists);
            string pattern = @".*_([\d]{4})([\d]{2})([\d]{2})([\d]{2})([\d]{2})_.*";
			Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);

            foreach (IFile file in await folder.GetFilesAsync())
            {

                foreach (Match match in Regex.Matches(file.Name, pattern))
                {
                    int year = Int32.Parse(match.Groups[1].ToString());
                    int month = Int32.Parse(match.Groups[2].ToString());
                    int day = Int32.Parse(match.Groups[3].ToString());
                    int hour = Int32.Parse(match.Groups[4].ToString());
                    int minute = Int32.Parse(match.Groups[5].ToString());
                    int second = 0;
                    //Debug.WriteLine("nome file " + file.Name);
                    //Debug.WriteLine("year: " + year + ": " + "month: " + month + ": " + "day: " + day + ": " + "hour: " + hour + ": " + "minute: " + minute + ": " + "second: " + second);
                    DateTime date = new DateTime(year, month, day, hour, minute, second);

                    DateTime now = DateTime.Now;
                    double elapsedTime = now.Subtract(date).TotalHours;
                    //delete images older than 2 days
                    if (elapsedTime >= 48)
                    {
						await file.DeleteAsync();
                    }

                }
            }
        }
    }
}
