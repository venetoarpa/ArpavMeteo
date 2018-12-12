using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ARPAVTemporali.Models
{
    /*
    [{
        "_key": "18747665",
        "_id": "notifications/18747665",
        "_rev": "_Vp0LZf6---",
        "device": "1ee4887b-aed1-43ab-b009-02085471f0fa",
        "contents": {
            "it": "Nuovi eventi per i comuni: Bevilacqua",
            "en": "New events for the municipalities: Bevilacqua"

        },
        "maxDate": 1506418710,
        "data": {
            "data": [{
                "comune": "Bevilacqua",
                "minDbz": 50,
                "distanza": 50,
                "data": "2017-09-26 11:38",
                "radars": {
                    "Loncon": {
                        "surface": 2,
                        "maxdbz": 82.5,
                        "count": 1

                    }
                }
            }]
        }
    }]
    */

    public class JSON_Notification
	{
        public class Content
        {
            public string it { get; set; }
            public string en { get; set; }
        }

        public class Detail
        {
            public string comune { get; set; }
            public double minDbz { get; set; }
            public double distanza { get; set; }
            public DateTime data { get; set; }
            //public JSON_RADAR radars { get; set; }
            public int maxDate { get; set; }
        }

		public string test { get; set; }
		public string _key { get; set; }
		public string _id { get; set; }
		public string _rev { get; set; }
		public string device { get; set; }
		public Content contents { get; set; }
		public int maxDate { get; set; }
		public List<Detail> data { get; set; }



		public JSON_Notification()
		{
		}
	}






}
