using Prism.Navigation;

namespace ForexApp.Extensions
{
    public static class NavigationParametersExtensions
    {
        public static void AddQuoteDetail(this NavigationParameters navigationParameters, string symbol)
        {
            navigationParameters.Add("AddQuoteDetail", symbol);
        }

        public static string GetQuoteDetail(this NavigationParameters navigationParameters)
        {
            return navigationParameters["AddQuoteDetail"] as string;
        }
    }
}