using Xamarin.Forms;

namespace ARPAVTemporali.Views
{
    public partial class ARPAVTemporaliPage : ContentPage
    {

        public ARPAVTemporaliPage()
        {
            InitializeComponent();
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();

			//ImageSource myImage = ImageSource.FromResource("ARPAVTemporali.Images.logo-arpav.png"); //carico un file locale come sempre
			//myCachedImaged.Source = myImage;
		}
    }
}
