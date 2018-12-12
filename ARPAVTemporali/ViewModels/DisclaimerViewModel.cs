using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ARPAVTemporali.Helpers;
using ARPAVTemporali.Models;
using Xamarin.Forms;

namespace ARPAVTemporali.ViewModels
{
    //helper class to retrieve data in dictionary by reference
    sealed class VariableReference
    {
        public Func<object> Get { get; private set; }
        public Action<object> Set { get; private set; }
        public VariableReference(Func<object> getter, Action<object> setter)
        {
            Get = getter;
            Set = setter;
        }
    }

    public class DisclaimerViewModel: INotifyPropertyChanged
    {

        private Testo _Disclaimer = new Testo();
        public Testo Disclaimer
        {
            get => _Disclaimer;
            set{
                _Disclaimer = value;
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



        //private Dictionary<string, VariableReference> slugsReference = new Dictionary<string, VariableReference>();

        public DisclaimerViewModel()
        {


        }



        /*
         * carica i dati dal database e assegnali alle proprietà
         */
        public async Task LoadData()
        {
            Loading = true;

            Testo testo = await DatabaseHelper.GetTestoBySlug("disclaimer");
            if (testo == null) return;
            Disclaimer = testo;

            var source = new HtmlWebViewSource();
            //var style = Variables.DetailPageStyle;
            var style = Variables.HTMLBaseStyle;

            var html = string.Format(@"<html><body>{0}{1}</body></html>", Disclaimer.Descrizione, style);
            source.Html = html;
            HTMLSource = source;

            Loading = false;
            /*var keys = slugsReference.Keys.ToList<string>(); 
            foreach (var key in keys)
            {
                Debug.WriteLine("key: " + key);
                var testo = await GetTextBySlug(key);
                if(testo!=null)
                {
                    Debug.WriteLine(testo.Titolo);
                    slugsReference[key].Set(testo);
                }
            }*/
        }


        public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
