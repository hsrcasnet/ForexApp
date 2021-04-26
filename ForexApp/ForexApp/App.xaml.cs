
using ForexApp.Helpers;
using ForexApp.Services;
using ForexApp.Services.Fakes;
using ForexApp.ViewModels;
using ForexApp.Views;

using Plugin.Settings;
using Plugin.Settings.Abstractions;

using Prism;
using Prism.Autofac;
using Prism.Ioc;
using Xamarin.Forms;

namespace ForexApp
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register services
            containerRegistry.RegisterSingleton(typeof(IForexServiceConfiguration), typeof(ForexServiceConfiguration));
            containerRegistry.RegisterSingleton(typeof(IForexService), typeof(ForexServiceMock));
            containerRegistry.RegisterInstance(typeof(ISettings), CrossSettings.Current);
            containerRegistry.RegisterSingleton(typeof(IForexSettings), typeof(ForexSettings));

            // Register views and view models
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>(Pages.Main);
            containerRegistry.RegisterForNavigation<QuoteDetailPage, QuoteDetailViewModel>(Pages.QuoteDetail);
        }

        protected override void OnInitialized()
        {
            this.InitializeComponent();

            this.NavigationService.NavigateAsync($"NavigationPage/{Pages.Main}");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}