
using ForexApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForexApp.Services
{
    public interface IForexService
    {
        Task<QuoteDto> GetQuote(string pair);

        Task<IEnumerable<QuoteDto>> GetQuotes(string[] pairs);
    }
}