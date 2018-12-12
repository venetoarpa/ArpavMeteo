using System;
using SQLite;

namespace ARPAVTemporali.Models
{
    public class Testo
    {
        public Testo()
        {
        }


		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

        private string _titolo = "";
        public string Titolo
        {
            get => _titolo;
            set
            {
                if (_titolo == value)
                    return;

                _titolo = value;
            }
        }

        private string _descrizione = "";
        public string Descrizione
        {
            get => _descrizione;
            set
            {
                if (_descrizione == value)
                    return;

                _descrizione = value;
            }
        }
              
        private string _ordine = "";
        public string Ordine
        {
            get => _ordine;
            set
            {
                if (_ordine.Equals(value))
                    return;

                _ordine = value;
            }
        }

        private string _slug = "";
        public string Slug
        {
            get => _slug;
            set
            {
                if (_slug.Equals(value))
                    return;

                _slug = value;
            }
        }

        private string _tipo = "";
        public string Tipo
        {
            get => _tipo;
            set
            {
                if (_tipo.Equals(value))
                    return;

                _tipo = value;
            }
        }

		
    }
}



