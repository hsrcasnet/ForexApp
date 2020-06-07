using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ForexApp.Model;
using Newtonsoft.Json;

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

        public async Task<IEnumerable<QuoteDto>> GetQuotes(string[] pairs)
        {
            // Send API request
            var apiKey = this.forexServiceConfiguration.ApiKey;
            var pairsString = string.Join(",", pairs);
            var uri = $"https://free.currconv.com/api/v7/convert?q={pairsString}&apiKey={apiKey}";
            var httpResponseMessage = await this.httpClient.GetAsync(uri);
            httpResponseMessage.EnsureSuccessStatusCode();

            // Read request payload and transform JSON to DTO object
            var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            var rootObject = await Task.Run(() => JsonConvert.DeserializeObject<ConvertResponseDto>(jsonResponse));

            var quoteDtos = rootObject.Results.CurrencyList.Select(c =>
            {
                var quoteDto = c.Value.ToObject<QuoteDto>();
                return quoteDto;
            }).ToList();
            return quoteDtos;
        }
    }
}