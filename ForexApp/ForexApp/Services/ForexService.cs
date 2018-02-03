using ForexApp.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ForexApp.Services
{
    public class ForexService : IForexService
    {
        private readonly IForexServiceConfiguration forexServiceConfiguration;
        private readonly HttpClient httpClient;

        public ForexService(IForexServiceConfiguration forexServiceConfiguration)
        {
            this.forexServiceConfiguration = forexServiceConfiguration;
            this.httpClient = new HttpClient();
        }

        public async Task<QuoteDto> GetQuote(string pair)
        {
            var quotes = await this.GetQuotes(new [] { pair });
            return quotes.SingleOrDefault();
        }

        public async Task<IEnumerable<QuoteDto>> GetQuotes(string[] pairs)
        {
            var apiKey = this.forexServiceConfiguration.ApiKey;
            var pairsString = string.Join(",", pairs);
            var uri = $"https://forex.1forge.com/1.0.3/quotes?pairs={pairsString}&api_key={apiKey}";
            var httpResponseMessage = await this.httpClient.GetAsync(uri);
            httpResponseMessage.EnsureSuccessStatusCode();

            var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            var quoteDtos = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<QuoteDto>>(jsonResponse));
            return quoteDtos ?? Enumerable.Empty<QuoteDto>();
        }
    }
}