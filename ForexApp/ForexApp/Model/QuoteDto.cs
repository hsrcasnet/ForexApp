using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ForexApp.Model
{ 
    //JSON String: "id": "EUR_CHF", "val": 1.08765, "to": "CHF", "fr": "EUR"

    public class QuoteDto
    {
        [JsonProperty("id")]
        public string Symbol { get; set; }

        [JsonProperty("val")]
        public decimal Price { get; set; }
    }

    internal class ConvertResponseDto
    {
        public ResultsDto Results { get; set; }
    }

    internal class ResultsDto
    {
        public ResultsDto()
        {
            this.CurrencyList = new Dictionary<string, JToken>();
        }

        [JsonExtensionData]
        public IDictionary<string, JToken> CurrencyList { get; set; }
    }
}