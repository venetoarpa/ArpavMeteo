using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace ARPAVTemporali.Controls
{
	/*
     * custom control con bindable properties
     * per funzionare correttamente quando aggiunti in code behind
     * le proprietà pubbliche (es: LabelText) devono avere get e set con GetValue e SetValue
     */
	public partial class SliderOptionControl : ContentView
    {
        void Handle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        // NAME
        public string LabelText {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

		public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
            propertyName: nameof(LabelText),
			 returnType: typeof(string),
			 declaringType: typeof(SliderOptionControl),
			 defaultValue: "",
			 defaultBindingMode: BindingMode.TwoWay,
			 propertyChanged: LabelTextPropertyChanged);

		private static void LabelTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = (SliderOptionControl)bindable;
            control.label.Text = newValue.ToString();
		}

		// SLIDER VALUE
		public string SliderValue
		{
			get { return (string)GetValue(SliderValueProperty); }
			set { SetValue(SliderValueProperty, value); }
		}

		public static readonly BindableProperty SliderValueProperty = BindableProperty.Create(
			propertyName: nameof(SliderValue),
			 returnType: typeof(string),
			 declaringType: typeof(SliderOptionControl),
			 defaultValue: "",
			 defaultBindingMode: BindingMode.TwoWay,
			 propertyChanged: SliderValuePropertyChanged);

		private static void SliderValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = (SliderOptionControl)bindable;
			control.sliderValue.Text = newValue.ToString();

            double val = Convert.ToDouble(newValue);
            val = Math.Round(val, 2);
            control.slider.Value = Math.Min(control.slider.Maximum, Math.Max(val, control.slider.Minimum));
		}

        public SliderOptionControl()
        {
			InitializeComponent();

            slider.ValueChanged += (sender, e) =>
            {
                double value = ((Slider)sender).Value;
                SetValue(SliderValueProperty, value.ToString());
            };
        }

        //EVENTS
        public event EventHandler<double> SliderValueChanged;

        void Handle_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            SliderValueChanged?.Invoke(this, e.NewValue);
        }
    }
}
