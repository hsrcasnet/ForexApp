using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ForexApp.Localization
{
    [ContentProperty("Text")]
    public class TranslateExtension : BindableObject, IMarkupExtension<BindingBase>
    {
        private static Lazy<ILocalizer> localizer;
        private static Lazy<ITranslationProvider> translationProvider;

        public static void Init(Func<ILocalizer> localizerFunction, Func<ITranslationProvider> translationProviderFunction)
        {
            localizer = new Lazy<ILocalizer>(localizerFunction, LazyThreadSafetyMode.PublicationOnly);
            translationProvider = new Lazy<ITranslationProvider>(translationProviderFunction, LazyThreadSafetyMode.PublicationOnly);
        }

        public string Text { get; set; }

        BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding(nameof(TranslationData.Value))
            {
                Source = new TranslationData(this.Text, localizer.Value, translationProvider.Value),
            };

            return binding;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return ((IMarkupExtension<BindingBase>)this).ProvideValue(serviceProvider);
        }
    }
}
