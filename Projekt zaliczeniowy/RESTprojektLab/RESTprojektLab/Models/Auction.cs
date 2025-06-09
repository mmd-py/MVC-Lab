namespace RESTprojektLab.Models
{
    public class Auction
    {
        public int AuctionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public string Category { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool IsAuctionOver { get; set; } = false;
        public int UserID { get; set; } //relacja do usera, fk
        public User? User { get; set; } //może być null, dzięki temu przesyła tylko id usera, a nie cały obiekt
    }
}
