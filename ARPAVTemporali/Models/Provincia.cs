using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ARPAVTemporali.Models
{
    public class Provincia: List<Comune>
    {
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public bool Visible { get => this.Count > 0; }

        public Provincia(string name = "", string code = "")
        {
            Name = name;
            Code = code;

        }

        public Provincia(string name, string code, IEnumerable<Comune> items)
        {
            Name = name;
            Code = code;

            foreach (var item in items)
                this.Add(item);
        }

    }
}
