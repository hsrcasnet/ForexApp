

using ForexApp.ViewModels;
using System;
using System.Globalization;
using ValueConverters;
using Xamarin.Forms;

namespace ForexApp.Converters
{
    public class PriceToImageConverter : ConverterBase
    {
        public static readonly BindableProperty PositiveImageProperty = BindableProperty.Create(
            nameof(PositiveImage),
            typeof(ImageSource),
            typeof(PriceToImageConverter),
            null);

        public static readonly BindableProperty NegativeImageProperty = BindableProperty.Create(
            nameof(NegativeImage),
            typeof(ImageSource),
            typeof(PriceToImageConverter),
            null);

        public ImageSource PositiveImage
        {
            get { return (ImageSource)this.GetValue(PositiveImageProperty); }
            set { this.SetValue(PositiveImageProperty, value); }
        }

        public ImageSource NegativeImage
        {
            get { return (ImageSource)this.GetValue(NegativeImageProperty); }
            set { this.SetValue(NegativeImageProperty, value); }
        }

        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var quoteViewModel = value as QuoteViewModel;
            if (quoteViewModel?.LastPrice == null)
            {
                return null;
            }

            if (quoteViewModel.Price > quoteViewModel.LastPrice)
            {
                return this.PositiveImage;
            }
            if (quoteViewModel.Price < quoteViewModel.LastPrice)
            {
                return this.NegativeImage;
            }

            return null;
        }
    }
}
