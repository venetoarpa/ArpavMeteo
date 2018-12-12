using System;
using System.IO;

namespace ARPAVTemporali.Models
{
    public class OverlayStream
    {
        public DateTime Date { get; set; }
        public Stream MosaicoStream { get; set; }
        public Stream FulminiStream { get; set; }
        public string MosaicoImagePath { get; set; }
        public string FulminiImagePath { get; set; }

        public OverlayStream()
        {
        }
    }
}