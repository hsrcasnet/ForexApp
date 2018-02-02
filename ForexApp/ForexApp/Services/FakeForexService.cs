using ForexApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForexApp.Services
{
    public class FakeForexService : IForexService
    {
        private static readonly Random Rng = new Random();

        public Task<IEnumerable<QuoteDto>> GetQuotes(string[] pairs)
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

            return Task.FromResult<IEnumerable<QuoteDto>>(quoteDtos);
        }
    }
}