using System;
using System.Collections.Generic;
using System.Diagnostics;
using ARPAVTemporali.Models;
using ARPAVTemporali.ViewModels;
using Xamarin.Forms;

namespace ARPAVTemporali.Views.Info
{
    public partial class InfoDetailPage : ContentPage
    {
        private InfoDetailViewModel vm;

        public InfoDetailPage(Testo testo = null)
        {
            InitializeComponent();
			RegisterWebViewEvents();

            vm = new InfoDetailViewModel();
            BindingContext = vm;


            if(testo!=null)
                vm.Info = testo;
        }

        private void RegisterWebViewEvents()
        {
            webview.Navigated += (object sender, WebNavigatedEventArgs e) => 
            {
                Debug.WriteLine(e.Source+"navigated");
            };
            webview.Navigating += (s, e) =>
            {
                if (e.Url.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        var uri = new Uri(e.Url);
                        Device.OpenUri(uri);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    e.Cancel = true;
                }
            };


        }
    }
}
