using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Localization;
using ARPAVTemporali.Persistance;
using ARPAVTemporali.ViewModels;
using FFImageLoading;
using PCLStorage;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace ARPAVTemporali.Views
{
    public partial class SharePage : ContentPage
    {
        private ShareViewModel vm;

        public SharePage(ShareViewModel viewModel)
        {
            InitializeComponent();

            vm = viewModel;
            BindingContext = vm;

            imageContainer.Content = vm.MyCachedImage;
        }

        protected override void OnAppearing()
        {
            //await vm.SaveImageFromStream(vm.MapStream);
            base.OnAppearing();
        }

        void Handle_ShareClicked(object sender, System.EventArgs e)
        {
            vm.Share();
        }

        async void Handle_SaveClicked(object sender, System.EventArgs e)
        {
            string message = await vm.Save();
            await DisplayAlert(AppResources.OperazioneCompletata, message, "OK");
            await Navigation.PopAsync();
        }
    }
}
