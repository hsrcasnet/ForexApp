
using Plugin.Settings.Abstractions;

namespace ForexApp.Helpers
{
    public class ForexSettings : IForexSettings
    {
        private readonly ISettings settings;

        public ForexSettings(ISettings settings)
        {
            this.settings = settings;
            if (this.Symbols == null)
            {
                this.Symbols = new[] { "EUR_CHF", "CHF_EUR" };
            }
        }

        public string[] Symbols
        {
            get
            {
                var serialized = this.settings.GetValueOrDefault(nameof(this.Symbols), null);
                return serialized?.Split(',');
            }
            set
            {
                string deserialized = null;
                if (value != null)
                {
                    deserialized = string.Join(",", value);
                }
                this.settings.AddOrUpdateValue(nameof(this.Symbols), deserialized);
            }
        }
    }
}