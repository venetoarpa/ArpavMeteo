using Xamarin.Forms;

namespace ARPAVTemporali.Controls
{
    public partial class CircleView : BoxView
    {
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(CircleView), 0.0);
     
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
     
    }
}