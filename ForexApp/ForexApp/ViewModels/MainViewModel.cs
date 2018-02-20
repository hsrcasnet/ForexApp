using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ForexApp.Extensions;
using ForexApp.Helpers;
using ForexApp.Localization;
using ForexApp.Model;
using ForexApp.Services;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ForexApp.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private readonly IForexSettings forexSettings;
        private readonly IForexService forexService;
        private readonly INavigationService navigationService;
        private readonly IPageDialogService pageDialogService;
        private readonly ILocalizer localizer;
        private string newQuoteSymbol;
        private bool isBusy;
        private bool isRefreshing;
        private ObservableCollection<QuoteViewModel> quotes;
        private string currentLanguage;

        public MainViewModel(
            IForexSettings forexSettings,
            IForexService forexService,
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            ILocalizer localizer)
        {
            this.forexSettings = forexSettings;
            this.forexService = forexService;
            this.navigationService = navigationService;
            this.pageDialogService = pageDialogService;
            this.localizer = localizer;
            this.localizer.CultureInfoChangedEvent += OnCurrentLanguageChanged;

            this.Quotes = new ObservableCollection<QuoteViewModel>();
        }

        public ICommand RefreshButtonCommand => new Command(
                                                    async () =>
                                                        {
                                                            this.IsBusy = true;
                                                            await this.LoadData();
                                                            this.IsBusy = false;
                                                        });

        public ICommand RefreshListCommand => new Command(
                                                  async () =>
                                                      {
                                                          this.IsRefreshing = true;
                                                          await this.LoadData();
                                                          this.IsRefreshing = false;
                                                      });

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.localizer.SetCultureInfo(new CultureInfo(this.forexSettings.Language));
            });

            this.RefreshButtonCommand.Execute(null);
        }

        private async Task LoadData()
        {
            if (!this.Quotes.Any())
            {
                var initialPairs = this.forexSettings.Symbols;
                await this.LoadAndUpdateQuotes(initialPairs);
            }
            else
            {
                var pairs = this.Quotes.Select(q => q.Symbol).ToArray();
                await this.LoadAndUpdateQuotes(pairs);
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            set
            {
                this.isBusy = value;
                this.OnPropertyChanged(nameof(this.IsBusy));
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshing;
            }
            set
            {
                this.isRefreshing = value;
                this.OnPropertyChanged(nameof(this.IsRefreshing));
            }
        }

        private async Task LoadAndUpdateQuotes(string[] pairs)
        {
            ICollection<QuoteDto> quotes = (await this.forexService.GetQuotes(pairs)).ToList();

            this.UpdateQuotes(quotes);
        }

        private void UpdateQuotes(ICollection<QuoteDto> quotes)
        {
            foreach (var quoteDto in quotes)
            {
                this.AddOrUpdateQuote(quoteDto);
            }

            var returnedSymbols = quotes.Select(dto => dto.Symbol);
            var unusedViewModels = this.Quotes.Where(vm => !returnedSymbols.Contains(vm.Symbol)).ToList();
            foreach (var quoteViewModel in unusedViewModels)
            {
                this.Quotes.Remove(quoteViewModel);
            }

            this.Quotes = new ObservableCollection<QuoteViewModel>(this.Quotes.OrderBy(q => q.Symbol));
        }

        private void AddOrUpdateQuote(QuoteDto quoteDto)
        {
            var vm = this.Quotes.SingleOrDefault(q => q.Symbol == quoteDto.Symbol);
            if (vm == null)
            {
                this.Quotes.Add(new QuoteViewModel(quoteDto, this.OnQuoteDelete));
            }
            else
            {
                vm.Update(quoteDto);
            }
        }

        private void OnQuoteDelete(QuoteViewModel quoteViewModel)
        {
            this.Quotes.Remove(quoteViewModel);
        }

        public ICommand AddSymbolCommand => new Command(async () => await this.AddSymbol(), () => this.IsNewQuoteSymbolEnabled);

        private async Task AddSymbol()
        {
            var symbol = this.NewQuoteSymbol;

            // Get new market data + update UI
            var quoteDto = await this.forexService.GetQuote(symbol);
            if (quoteDto != null)
            {
                // Add new symbol to settings
                var symbols = this.forexSettings.Symbols.ToList();
                symbols.Add(symbol);
                this.forexSettings.Symbols = symbols.ToArray();

                this.AddOrUpdateQuote(quoteDto);
                this.OnPropertyChanged(nameof(this.Quotes));
            }
            else
            {
                await this.pageDialogService.DisplayAlertAsync("Error", $"Failed to add currency pair '{symbol}'.", "OK");
            }

            this.NewQuoteSymbol = null;
        }

        public string NewQuoteSymbol
        {
            get
            {
                return this.newQuoteSymbol;
            }
            set
            {
                this.newQuoteSymbol = value?.ToUpperInvariant();
                this.OnPropertyChanged(nameof(this.NewQuoteSymbol));
                this.OnPropertyChanged(nameof(this.IsNewQuoteSymbolEnabled));
            }
        }

        public bool IsNewQuoteSymbolEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.NewQuoteSymbol);
            }
        }

        public ObservableCollection<QuoteViewModel> Quotes
        {
            get
            {
                return this.quotes;
            }
            set
            {
                this.quotes = value;
                this.OnPropertyChanged(nameof(this.Quotes));
            }
        }

        public ICommand SelectQuoteCommand => new Command(async () => await this.OnSelectQuote());

        public QuoteViewModel SelectedItem { get; set; }

        private async Task OnSelectQuote()
        {
            var navigationParameter = new NavigationParameters();
            navigationParameter.AddQuoteDetail(this.SelectedItem.Symbol);
            await this.navigationService.NavigateAsync(Pages.QuoteDetail, navigationParameter);
        }

        public string CurrentLanguage
        {
            get { return this.currentLanguage; }
            set
            {
                this.currentLanguage = value;
                this.OnPropertyChanged(nameof(this.CurrentLanguage));
            }
        }

        public ICommand ChangeCurrentLanguageCommand => new Command(this.ToggleCurrentLanguage);

        private void ToggleCurrentLanguage()
        {
            var currentCulture = this.localizer.GetCurrentCulture();
            CultureInfo newCulture;
            if (currentCulture.TwoLetterISOLanguageName == Languages.CultureInfoEnglish.TwoLetterISOLanguageName)
            {
                newCulture = Languages.CultureInfoGerman;
            }
            else
            {
                newCulture = Languages.CultureInfoEnglish;
            }

            this.localizer.SetCultureInfo(newCulture);
            this.forexSettings.Language = newCulture.TwoLetterISOLanguageName;
        }

        private void OnCurrentLanguageChanged(object sender, CultureInfoChangedEventArgs e)
        {
            this.CurrentLanguage = e.CultureInfo.TwoLetterISOLanguageName;
        }

        public void Dispose()
        {
            this.localizer.CultureInfoChangedEvent -= this.OnCurrentLanguageChanged;
        }
    }
}