using System;
using System.Globalization;

using ValueConverters;

using Xamarin.Forms;

namespace ForexApp.Converters
{
    /// <summary>
    /// Converts given currency pair to currency-xyz.png image source.
    /// </summary>
    public class SymbolToImageConverter : ConverterBase
    {
        protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var symbol = value as string;
            if (symbol != null)
            {
                var index = System.Convert.ToInt32(parameter);
                var currency = symbol.Substring(index == 0 ? 0 : 3, 3).ToLowerInvariant();
                var imageSource = ImageSource.FromResource($"ForexApp.Resources.Images.currency-{currency}.png");
                return imageSource;
            }

            return UnsetValue;
        }
    }
}
