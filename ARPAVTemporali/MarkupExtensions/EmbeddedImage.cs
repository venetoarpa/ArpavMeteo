using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ARPAVTemporali
{
	[ContentProperty("Source")] //specificando content property si può omettere di scriverlo nel markup
	public class EmbeddedImage : IMarkupExtension
	{
		public string Source { get; set; }
		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (String.IsNullOrWhiteSpace(Source))
				return null;

			return ImageSource.FromResource(Source);
		}
	}
}