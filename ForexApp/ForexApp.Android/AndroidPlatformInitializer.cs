using ForexApp.Droid.Localization;
using ForexApp.Localization;
using Prism;
using Prism.Ioc;

namespace ForexApp.Droid
{
    public class AndroidPlatformInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILocalizer, Localizer>();
        }
    }
}