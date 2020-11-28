namespace Bitcoin.Api.Models
{
    public class Trade
    {
        public int Tid { get; set; }
        public int Date { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public float Amount { get; set; }

    }
}