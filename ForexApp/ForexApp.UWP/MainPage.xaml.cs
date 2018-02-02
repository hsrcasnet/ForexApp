namespace ForexApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            var platformInitializer = new UwpPlatformInitializer();
            this.LoadApplication(new ForexApp.App(platformInitializer));
        }
    }
}