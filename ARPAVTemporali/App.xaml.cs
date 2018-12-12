using Xamarin.Forms;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using ARPAVTemporali.Views;
using ARPAVTemporali.Views.SettingsPages;
using System.Threading.Tasks;
using System.Linq;
using ARPAVTemporali.Helpers;
using Plugin.Connectivity;
using System;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ARPAVTemporali
{
    public partial class App : Application
    {
        
        public NotificationHelper NotificationManager;

        private int splashDelay = 500;

        public Thickness SafeInsets { get; set; }

		public App()
        {

			InitializeComponent();

            var rootPage = new ARPAVTemporaliPage();
            MainPage = new Xamarin.Forms.NavigationPage(rootPage);

            // iPhone X safe area
            SafeInsets = new Thickness(0, 0, 0, 0);
            rootPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            rootPage.Appearing += SetSafeInset;

            Init();
        }

        private async void Init()
        {
			await DatabaseHelper.CheckUpdates(); //conrtolla se il database ha bisogno di essere aggiornato

			NotificationManager = new NotificationHelper(); //gestisce le notifiche
        }

        private void SetSafeInset(object sender, EventArgs e)
        {
            Xamarin.Forms.Page page = sender as Xamarin.Forms.Page;
            SafeInsets = page.On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();

            if (Device.RuntimePlatform == Device.iOS)
            {
                try
                {
                    // modifica il padding se è un iPhone X
                    if (SafeInsets.Top > 0)
                    {
						Thickness headerPadding = (OnPlatform<Thickness>)Application.Current.Resources["HeaderPadding"];
                        headerPadding.Top = SafeInsets.Top;
						Application.Current.Resources["HeaderPadding"] = headerPadding;

                        Thickness footerPadding = (OnPlatform<Thickness>)Application.Current.Resources["FooterPadding"];
                        footerPadding.Bottom = SafeInsets.Bottom;
                        Application.Current.Resources["FooterPadding"] = footerPadding;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            Application.Current.Resources["SafeInsets"] = SafeInsets;

            page.Appearing -= SetSafeInset; //unsubscribe from event
        }

        protected async override void OnStart()
        {
            // Handle when your app start
            Debug.WriteLine("starting app");
            //controlla le impostazioni
            await CheckSettings();
            Start();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Debug.WriteLine("sleeping app");
            MessagingCenter.Send(this,Events.OnSleep);

        }

        protected async override void OnResume()
        {
            // Handle when your app resumes
            Debug.WriteLine("resuming app");
            await CheckSettings();
        }

		public async Task CheckSettings()
		{
			/*
             * subscribe to messaging center
             * do whatever you want in the modal
             * unsubscribe when the message has been handled
             * NOTA BENE: non mi serve davvero, ma l'ho messo per ricordarmi come funziona
             * per aspettare i risultati uso i task
             */
			//MessagingCenter.Subscribe<Views.Settings.LanguageModalPage, string>(this, Events.LanguageSelectedLabel, OnLanguageSelected);


            Debug.WriteLine("checking language");
            if (Settings.Language.Equals(""))
            {
                //il modal non viene aggiunto allo stack se è già presente
                LanguageModalPage modalPage = MainPage.Navigation.ModalStack.FirstOrDefault((Xamarin.Forms.Page page) => page.GetType() == typeof(LanguageModalPage)) as LanguageModalPage;
                if(modalPage == null)
                {
                    modalPage = new LanguageModalPage();
                    await MainPage.Navigation.PushModalAsync(modalPage);
                }
                await modalPage.LanguageSelectedTask;
            }

            Settings.SetCultureInfo(Settings.Language); //aggiorno la culture di AppResources

            Debug.WriteLine("checking disclaimer");
            if (Settings.DisclaimerRead == false)
            {
                //il modal non viene aggiunto allo stack se è già presente
                DisclaimerPage modalPage = MainPage.Navigation.ModalStack.FirstOrDefault((Xamarin.Forms.Page page) => page.GetType() == typeof(DisclaimerPage)) as DisclaimerPage;
                if (modalPage == null)
                {
                    modalPage = new DisclaimerPage();
                    await MainPage.Navigation.PushModalAsync(modalPage);
                }
                await modalPage.DisclaimerReadTask;
            }
            //controllo se è il primo avvio
            if (!Settings.NotificationPermissionAsked)
            {
                Debug.WriteLine("asking notification permission");
                Settings.NotificationPermissionAsked = true;
                bool answer = await MainPage.DisplayAlert("Notifiche Push", "Vuoi abilitare le notifiche push", "Si", "No");

                Settings.NotificationEnabled = answer;
            }

            /*
             * necessario su android altrimenti resta schermo bianco
             * TODO: cambiare metodo
             */
            await Task.Delay(splashDelay);
        }


        public async void Start()
        {
            //rimpiazzo la root nel navigation stack
            Debug.WriteLine("starting app");

            Debug.WriteLine("checking connection");
            if (!CrossConnectivity.Current.IsConnected)
            {
                await Application.Current.MainPage.DisplayAlert("no internet", "you need an internet connection", "ok");
                //You are offline, notify the user
            }

            Debug.WriteLine("activating notifications");
            if (Settings.NotificationEnabled)
            {
                await NotificationManager.Activate(); //attiva le notifiche
                Debug.WriteLine("fetching notifications");
                await NotificationManager.GetNotificationsTask(); //fetching notifications
            }

            Debug.WriteLine("going to home page");
            if(MainPage.Navigation.NavigationStack.Count>0)
            {
                Xamarin.Forms.Page root = MainPage.Navigation.NavigationStack[0];
                MainPage.Navigation.InsertPageBefore(new HomePage(), root);
            }
            
			await MainPage.Navigation.PopToRootAsync();
        }

    }

}
