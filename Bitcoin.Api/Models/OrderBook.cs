using System.Collections.Generic;

namespace Bitcoin.Api.Models
{
    public class OrderBook
    {
        public List<List<float>> Bids { get; set; }
        public List<List<float>> Asks { get; set; }
    }
}