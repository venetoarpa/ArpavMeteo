using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Localization;
using FFImageLoading.Forms;
using PCLStorage;
using Xamarin.Forms;

namespace ARPAVTemporali.ViewModels
{
    public class ShareViewModel: BaseViewModel
    {
        
        private string _Text { get; set; } = String.Empty;
        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                OnPropertyChanged();
            }
        }

        private CachedImage _MyCachedImage { get; set; }
        public CachedImage MyCachedImage
        {
            get => _MyCachedImage;
            set
            {
                _MyCachedImage = value;
                OnPropertyChanged();
            }
        }

        private bool _IsAndroid { get; set; } = false;
        public bool IsAndroid
        {
            get => _IsAndroid;
            set
            {
                _IsAndroid = value;
                OnPropertyChanged();
            }
        }

        public ShareViewModel()
        {
            MyCachedImage = new CachedImage()
            {
                CacheType = FFImageLoading.Cache.CacheType.Disk,
                CacheDuration = TimeSpan.FromDays(1),
            };

            // condividi appena l'immagine è caricata
            MyCachedImage.Success += (object sender, CachedImageEvents.SuccessEventArgs e) =>
            {
                var cachedImage = sender as CachedImage;
                //Debug.WriteLine($"percorso dell'immagine in cache {e.ImageInformation.FilePath}");
                //byte[] rawJpg = await cachedImage.GetImageAsPngAsync();
            };

            CheckDevice();
        }

        // controllo il tipo di dispositivo
        private void CheckDevice()
        {
            
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    IsAndroid = false;
                    break;
                case Device.Android:
                    IsAndroid = true;
                    break;
                default:
                    IsAndroid = false;
                    break;
            }
        }

        /*
        * https://www.c-sharpcorner.com/article/local-file-storage-using-xamarin-form/
        */
        private async static Task SaveImage(byte[] image,String fileName, IFolder rootFolder = null)  
        {  
            // get hold of the file system  
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
  
            // create a file, overwriting any existing file  
            IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);  
  
            // populate the file with image data  
            using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))  
            {  
                stream.Write(image, 0, image.Length);
            }
        }


        private async Task SaveImageFromStream(Stream stream, string imageName = "image.png")
        {

            IFolder rootFolder = FileSystem.Current.LocalStorage;

            IFolder folder = await rootFolder.CreateFolderAsync("SharedImages", CreationCollisionOption.OpenIfExists);
            IFile image = await folder.CreateFileAsync(imageName, CreationCollisionOption.ReplaceExisting);

            //salvo lo stream nella cartella SharedImages
            using (System.IO.Stream filestream = await image.OpenAsync(FileAccess.ReadAndWrite))
            {
                await stream.CopyToAsync(filestream);
            }

            //ImageFile = image; //imposta un riferimento al file salvato
        }

        public async void Share()
        {
            var shareHelper = DependencyService.Get<IShareHelper>();

            string shareText = Text + " - scarica l'APP ARPAV Temporali: http://www.parallelo.it/arpav_temporali";
            shareText = ""; // imposto un testo vuoto per compatibilità con WhatsApp

            //crea la cartella sharedImages o la apro se esiste
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("SharedImages", CreationCollisionOption.OpenIfExists);

            // ottieni l'array di byte dell'immagine da salvare in formato PNG
            byte[] image = await MyCachedImage.GetImageAsPngAsync();
            string fileName = GetFileName(); //ottengo un nome per l'immagine da salvare
            await SaveImage(image, fileName, folder);

            IFile file = await folder.GetFileAsync(fileName); //riferimento al file salvato

            var imageSource = ImageSource.FromFile(file.Path);
            shareHelper.Share("ARPAV Temporali", "", imageSource);
        }

        /*
         * genera un nome univoco per il file da salvare
         */
        private string GetFileName()
        {
            var d = DateTime.Now;
            var dateString = d.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            return $"ARPAV{dateString}";
        }

        public async Task<string> Save()
        {
            var tcs = new TaskCompletionSource<string>();
            try
            {
                var fileName = GetFileName()+".jpg";
                var shareHelper = DependencyService.Get<IShareHelper>();
                var basePath = shareHelper.GetDownloadPath();

                IFolder rootFolder = FileSystem.Current.LocalStorage;
                var baseFolder = await rootFolder.CreateFolderAsync(basePath, CreationCollisionOption.OpenIfExists);
                var screenshotsFolder = await baseFolder.CreateFolderAsync("Screenshots", CreationCollisionOption.OpenIfExists);
                var image = await MyCachedImage.GetImageAsJpgAsync();
                await SaveImage(image, fileName, screenshotsFolder);

                Debug.WriteLine($"Screenshots Path: {screenshotsFolder.Path} {shareHelper.GetDownloadPath()}");
                //shareHelper.SaveImageToCameraRoll(MapImageSource, $"{dateString}.png");
                var message = $"{AppResources.ImmagineSalvata}: {screenshotsFolder.Path}/{fileName}";
                tcs.SetResult(message);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Problem saving the image: {ex.Message}");
                tcs.SetResult($"Problem saving the image: {ex.Message}");
            }
            return await tcs.Task;
        }

    }
}
