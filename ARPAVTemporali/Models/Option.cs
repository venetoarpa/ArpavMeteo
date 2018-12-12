using System;
using Newtonsoft.Json;
using SQLite;

namespace ARPAVTemporali.Models
{
    [Table("Options")]
    public class Option
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        public Option()
        {
        }
    }
}
