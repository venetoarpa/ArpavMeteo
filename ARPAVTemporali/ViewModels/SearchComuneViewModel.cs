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
using SQLite;
using Xamarin.Forms;

namespace ARPAVTemporali.ViewModels
{
    public class SearchComuneViewModel: BaseViewModel
    {

        private string _Query = String.Empty; //query string to filter comuni

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

        private ObservableCollection<Comune> _ElencoComuni = new ObservableCollection<Comune>();
        public ObservableCollection<Comune> ElencoComuni
        {
            get
            {
                var comuni = GetComuni()
                    .OrderBy((Comune comune) => comune.Provincia)
                    .ThenBy((Comune comune) => comune.Name).ToList();
                
                return new ObservableCollection<Comune>(comuni);
            }
            set
            {
                _ElencoComuni = value;
                OnPropertyChanged();
            }
        }

        // ottieni la lista di comuni filtrati in base alla query
        private ObservableCollection<Comune> GetComuni()
        {
            if (string.IsNullOrWhiteSpace(_Query))
            {
                return _ElencoComuni;
            }
            else
            {
                var query = _Query.ToLower();

                var results = _ElencoComuni.Where((Comune c) => c.Name.ToLower().Contains(query)).ToList();

                return new ObservableCollection<Comune>(results);
            }
        }

        public SearchComuneViewModel()
        {
            InitElencoComuni();
        }

        private async void InitElencoComuni()
        {
            Loading = true;
            List<Comune> comuni = await ComuniHelper.Fetch();
            ElencoComuni = new ObservableCollection<Comune>(comuni);
            Loading = false;
        }

        public void SetFilter(string query = null)
        {
            _Query = query;
            OnPropertyChanged(nameof(ElencoComuni));
        }

    }
}
