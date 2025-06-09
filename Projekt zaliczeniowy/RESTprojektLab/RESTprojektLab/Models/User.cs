namespace RESTprojektLab.Models
{
    public class User //reprezentuje użytkownika
    {
        //właściwości do pobierania i ustawiania wartości
        public int UserID { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsOver18 { get; set; }
        public bool AcceptsRegulations { get; set; }
        //1 user, wiele aukcji
        public List<Auction> Auctions { get; set; } = new();
    }
}
