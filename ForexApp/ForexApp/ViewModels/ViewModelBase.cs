using Prism.Navigation;

using Xamarin.Forms;

namespace ForexApp.ViewModels
{
    public class ViewModelBase : BindableObject, INavigationAware
    {
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}