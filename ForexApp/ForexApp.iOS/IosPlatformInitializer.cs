using ForexApp.iOS.Localization;
using ForexApp.Localization;
using Prism;
using Prism.Ioc;

namespace ForexApp.iOS
{
    public class IosPlatformInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILocalizer, Localizer>();
        }
    }
}