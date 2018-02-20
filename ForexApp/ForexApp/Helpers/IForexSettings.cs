namespace ForexApp.Helpers
{
    public interface IForexSettings
    {
        string[] Symbols { get; set; }

        string Language { get; set; }
    }
}