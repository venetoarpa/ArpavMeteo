using System;

using Xamarin.Forms;

namespace ARPAVTemporali.Views.Settings
{
    public class ComunePage : ContentPage
    {
        public ComunePage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

