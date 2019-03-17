namespace ForexApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.LoadApplication(new ForexApp.App(new UwpPlatformInitializer()));
        }
    }
}