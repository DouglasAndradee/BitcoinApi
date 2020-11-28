namespace Bitcoin.Api.Models
{
    public class Result
    {
        public Ticker Ticker { get; set; }
    }

    public class Ticker
    {
        public float High { get; set; }
        public float Low { get; set; }
        public float Vol { get; set; }
        public float Last { get; set; }
        public float Buy { get; set; }
        public float Sell { get; set; }
        public float Open { get; set; }
        public int Date { get; set; }
    }
}