using System;
using System.Globalization;
using ForexApp.Localization;

namespace ForexApp.UWP.Localization
{
    public class Localizer : ILocalizer
    {
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            this.OnLocaleChanged(cultureInfo);
        }

        public event EventHandler<CultureInfoChangedEventArgs> CultureInfoChangedEvent;

        protected virtual void OnLocaleChanged(CultureInfo cultureInfo)
        {
            this.CultureInfoChangedEvent?.Invoke(this, new CultureInfoChangedEventArgs(cultureInfo));
        }

        public CultureInfo GetCurrentCulture()
        {
            return CultureInfo.CurrentCulture;
        }
    }
}