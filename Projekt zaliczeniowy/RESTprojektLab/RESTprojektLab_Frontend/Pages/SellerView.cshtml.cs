using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RESTprojektLab.Models;

namespace RESTprojektLab_Frontend.Pages
{
    public class SellerViewModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public SellerViewModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        [BindProperty]
        public Auction Auction { get; set; } = new();
        public string? Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _clientFactory.CreateClient("AuctionAPI");

            Auction.IsAuctionOver = false;
            Auction.CurrentPrice = Auction.StartingPrice;
            Auction.UserID = UserId;

            var response = await client.PostAsJsonAsync("auctions", Auction);
            if (response.IsSuccessStatusCode)
            {
                Message = "Przedmiot zosta³ wystawiony.";
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Message = "Wystawianie przedmiotu nie powiod³o siê.";
                //Message = $"Wystawianie przedmiotu nie powiod³o siê. ({response.StatusCode}): {errorContent}";
            }
            return Page();
        }
    }
}
