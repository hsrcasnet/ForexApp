using ForexApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForexApp.Services.Fakes
{
    public class FakeForexService : IForexService
    {
        private static readonly Random Rng = new Random();

        public async Task<QuoteDto> GetQuote(string pair)
        {
            var quotes = await this.GetQuotes(new[] { pair });
            return quotes.SingleOrDefault();
        }

        public async Task<IEnumerable<QuoteDto>> GetQuotes(string[] pairs)
        {
            var quoteDtos = new List<QuoteDto>();
            foreach (var symbol in pairs)
            {
                var dto = new QuoteDto
                {
                    Symbol = symbol,
                    Price = (decimal)Rng.NextDouble() * Rng.Next(1, 100),
                };
                quoteDtos.Add(dto);
            }

            await Task.Delay(1000);

            return quoteDtos;
        }
    }
}