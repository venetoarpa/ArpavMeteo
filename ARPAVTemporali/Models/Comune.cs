using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using SQLite;

namespace ARPAVTemporali.Models
{
    [Table("Comuni")]
	public class Comune
	{
        [PrimaryKey, AutoIncrement]
        public int ID { get; }

        public string Provincia { get; set; }
        public string SiglaProvincia { get; set; }

        [Column("Denominazione")]
        [JsonProperty(PropertyName = "Denominazione")]
        public string Name { get; set; }

        [Column("Latitudine")]
        [JsonProperty(PropertyName = "Latitudine")]
        public double lng { get; set; }

        [Column("Longitudine")]
        [JsonProperty(PropertyName = "Longitudine")]
        public double lat { get; set; }

		public Comune()
		{
		}
	}

}

