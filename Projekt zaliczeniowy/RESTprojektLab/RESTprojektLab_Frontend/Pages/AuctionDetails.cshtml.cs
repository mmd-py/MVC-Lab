using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RESTprojektLab.Models;

namespace RESTprojektLab_Frontend.Pages
{
    public class AuctionDetailsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public AuctionDetailsModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        //w�a�ciwo�ci dla sesji lokalnej
        [BindProperty]
        public Auction? Auction { get; set; }

        [BindProperty]
        public List<string> BiddingSteps { get; set; } = new();

        [BindProperty]
        public bool LatestUser { get; set; } = false;

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, string? category, int? userId)
        {
            //wysy�a zapytanie get do /api/auctions/{id} do kontrolera AuctionsController
            //kt�ry je obs�uguje i zwraca dane z bazy
            var client = _clientFactory.CreateClient("AuctionAPI");
            var result = await client.GetFromJsonAsync<Auction>($"auctions/{id}");

            if (result == null)
            {
                return NotFound();
            }

            Auction = result;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string? category, int? userId)
        {
            var client = _clientFactory.CreateClient("AuctionAPI");
            var result = await client.GetFromJsonAsync<Auction>($"auctions/{id}");

            if (result == null)
            {
                return NotFound();
            }

            Auction = result;

            if (Auction.IsAuctionOver)
            {
                return Page();
            }

            if (Auction.CurrentPrice < Auction.StartingPrice)
            {
                Auction.CurrentPrice = Auction.StartingPrice;
            }

            //pierwsza wersja licytacji
            //var random = new Random();
            //int scenario = random.Next(1, 5);

            //licytacja
            var random = new Random();
            int scenario;
            int[] weightedScenarios = { 1, 1, 1, 2, 2, 2, 2, 3, 4, 4 };
            do
            {
                scenario = weightedScenarios[random.Next(weightedScenarios.Length)];
            }
            while (scenario == 1 && LatestUser == true);   

            switch (scenario)
            {
                case 1:
                    //gdy LatestUser = false
                    //podbicie ceny o 10% od wywo�awczej i zapisanie do 2 mca po przecinku
                    Auction.CurrentPrice = decimal.Round(Auction.CurrentPrice + (Auction.StartingPrice * 0.10m), 2);
                    BiddingSteps.Add($"Podbi�e� cen�. Nowa cena to: {Auction.CurrentPrice:F2} z�");
                    LatestUser = true;
                    break;
                case 2:
                    Auction.CurrentPrice = decimal.Round(Auction.CurrentPrice + (Auction.StartingPrice * 0.10m), 2);
                    BiddingSteps.Add($"Kto� inny podbi� cen�. Nowa cena to: {Auction.CurrentPrice:F2} z�");
                    LatestUser = false;
                    break;
                case 3:
                    Auction.CurrentPrice = decimal.Round(Auction.CurrentPrice + (Auction.StartingPrice * 0.10m), 2);
                    BiddingSteps.Add($"Wygra�e� licytacj�! Cena ko�cowa to: {Auction.CurrentPrice:F2} z�");
                    Auction.IsAuctionOver = true;
                    break;
                case 4:
                    BiddingSteps.Add($"Przegra�e� licytacj�!");
                    Auction.IsAuctionOver = true;
                    break;
            }

            var response = await client.PutAsJsonAsync($"auctions/{id}", Auction);

            if (!response.IsSuccessStatusCode)
            {
                BiddingSteps.Add("B��d zapisu.");
            }
            return Page();
        }
    }
}
