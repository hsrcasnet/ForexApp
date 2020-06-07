
using ForexApp.Helpers;
using ForexApp.Localization;
using ForexApp.Services;
using ForexApp.Services.Fakes;
using ForexApp.ViewModels;
using ForexApp.Views;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

using Prism;
using Prism.Autofac;
using Prism.Ioc;
using Strings = ForexApp.Resources.Strings;

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
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>(Pages.Main);
            containerRegistry.RegisterForNavigation<QuoteDetailPage, QuoteDetailViewModel>(Pages.QuoteDetail);

            ResxTranslationProvider.Init(
                Strings.ResourceManager,
                () => base.Container.Resolve<ILocalizer>());

            TranslateExtension.Init(
                () => base.Container.Resolve<ILocalizer>(),
                () => new ResxTranslationProvider());
        }


        protected override void OnInitialized()
        {
            this.InitializeComponent();

            this.NavigationService.NavigateAsync(Pages.Main);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}