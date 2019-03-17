namespace ForexApp.Model
{
    public class QuoteDto
    {
        //JSON String: {"symbol":"EURUSD","price":1.24308,"bid":1.24288,"ask":1.24328,"timestamp":1517056850}

        public string Symbol { get; set; }

        public decimal Price { get; set; }

        public decimal Bid { get; set; }

        public decimal Ask { get; set; }
    }
}