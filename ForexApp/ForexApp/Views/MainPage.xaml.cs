using ForexApp.Services;
using ForexApp.ViewModels;

using Xamarin.Forms;

namespace ForexApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            //var forexService = new ForexServiceMock();
            var forexService = new ForexService(new ForexServiceConfiguration());
            this.BindingContext = new MainViewModel(forexService);
        }
    }
}