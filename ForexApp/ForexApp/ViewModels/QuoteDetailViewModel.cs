using ForexApp.Extensions;
using ForexApp.Localization;
using ForexApp.Resources;
using Prism.Navigation;

namespace ForexApp.ViewModels
{
    public class QuoteDetailViewModel : ViewModelBase
    {
        private readonly ITranslationProvider translationProvider;

        private string symbol;
        private string title;

        public QuoteDetailViewModel(ITranslationProvider translationProvider)
        {
            // Demo: ITranslationProvider can also be injected in order to translate strings at runtime.
            this.translationProvider = translationProvider;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            this.Symbol = parameters.GetQuoteDetail();

            // Demo: Fill placeholders of translated string
            this.Title = string.Format(this.translationProvider.Translate(nameof(Strings.QuoteDetailPageTitle)), this.Symbol);
        }

        public string Title
        {
            get => this.title;
            private set => this.SetProperty(ref this.title, value);
        }

        public string Symbol
        {
            get => this.symbol;
            private set => this.SetProperty(ref this.symbol, value);
        }
    }
}
