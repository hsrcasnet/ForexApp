using Prism.Navigation;

using Xamarin.Forms;

namespace ForexApp.ViewModels
{
    public class ViewModelBase : BindableObject, INavigationAware
    {
        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}