using System;
using System.Diagnostics;
using ARPAVTemporali.Models;
using Xamarin.Forms;

namespace ARPAVTemporali.DataTemplateSelectors
{
    public class ComuneDataTemplateSelector: DataTemplateSelector
    {
        public ComuneDataTemplateSelector()
        {
        }

        public DataTemplate ValidTemplate { get; set; }
        public DataTemplate InvalidTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var comune = item as Comune;
            var listView = container as ListView;
            return ValidTemplate;
        }
    }
}
