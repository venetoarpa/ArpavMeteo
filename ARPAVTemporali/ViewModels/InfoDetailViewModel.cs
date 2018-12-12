using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ARPAVTemporali.Models;
using Xamarin.Forms;

namespace ARPAVTemporali.ViewModels
{
    public class InfoDetailViewModel: INotifyPropertyChanged
    {
        private Testo _Info = new Testo();
        public Testo Info
        {
            get => _Info;
            set
            {
                _Info = value;
                UpdateHTML(); // aggiorna l'HTML quando cambia la info
                OnPropertyChanged();
            }
        }

        private HtmlWebViewSource _HTMLSource = new HtmlWebViewSource();
        public HtmlWebViewSource HTMLSource
        {
            get => _HTMLSource;
            set
            {
                _HTMLSource = value;
                OnPropertyChanged();
            }
        }

        private bool _Loading = false;
        public bool Loading
        {
            get => _Loading;
            set
            {
                _Loading = value;
                OnPropertyChanged();
            }
        }

        public InfoDetailViewModel()
        {
        }

        // aggiorna l'HTML della WebView
        private void UpdateHTML()
        {
            Loading = true;

            if (Info == null) return;

            var source = new HtmlWebViewSource();

            //var style = Variables.DetailPageStyle;
            var style = Variables.HTMLBaseStyle;

            var html = string.Format(@"<html><body>{0}{1}</body></html>", Info.Descrizione, style);
            source.Html = html;
            HTMLSource = source;

            Loading = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
