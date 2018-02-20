using ForexApp.Localization;
using ForexApp.UWP.Localization;
using Prism;
using Prism.Ioc;

namespace ForexApp.UWP
{
    public class UwpPlatformInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILocalizer, Localizer>();
        }
    }
}