using System;
using System.Globalization;
using System.Threading;
using ForexApp.Localization;

namespace ForexApp.Droid.Localization
{
    public class Localizer : ILocalizer
    {
        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            this.OnLocaleChanged(cultureInfo);
        }

        public event EventHandler<CultureInfoChangedEventArgs> CultureInfoChangedEvent;

        protected virtual void OnLocaleChanged(CultureInfo cultureInfo)
        {
            this.CultureInfoChangedEvent?.Invoke(this, new CultureInfoChangedEventArgs(cultureInfo));
        }

        public CultureInfo GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture;
        }
    }
}