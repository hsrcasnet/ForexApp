
using ForexApp.ViewModels;
using System;
using System.Globalization;
using ValueConverters;
using Xamarin.Forms;

namespace ForexApp.Converters
{
    public class PriceToColorConverter : ConverterBase
    {
        public static readonly BindableProperty PositiveColorProperty = BindableProperty.Create(
            nameof(PositiveColor),
            typeof(Color),
            typeof(PriceToColorConverter),
            Color.Default);

        public static readonly BindableProperty NegativeColorProperty = BindableProperty.Create(
            nameof(NegativeColor),
            typeof(Color),
            typeof(PriceToColorConverter),
            Color.Default);

        public Color PositiveColor
        {
            get { return (Color)this.GetValue(PositiveColorProperty); }
            set { this.SetValue(PositiveColorProperty, value); }
        }

        public Color NegativeColor
        {
            get { return (Color)this.GetValue(NegativeColorProperty); }
            set { this.SetValue(NegativeColorProperty, value); }
        }

        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var quoteViewModel = value as QuoteViewModel;
            if (quoteViewModel?.LastPrice == null)
            {
                return Color.Default;
            }

            if (quoteViewModel.Price > quoteViewModel.LastPrice)
            {
                return this.PositiveColor;
            }
            if (quoteViewModel.Price < quoteViewModel.LastPrice)
            {
                return this.NegativeColor;
            }

            return Color.Default;
        }
    }
}
