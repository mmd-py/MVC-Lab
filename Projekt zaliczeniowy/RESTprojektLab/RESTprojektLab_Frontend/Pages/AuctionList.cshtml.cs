using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RESTprojektLab.Models;

namespace RESTprojektLab_Frontend.Pages
{
    public class AuctionListModel : PageModel //model strony razor
    {
        private readonly IHttpClientFactory _clientFactory;
        public List<Auction> Auctions { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SelectedCategory { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }
        public AuctionListModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> OnGetAsync(string? category, int? userId)
        {
            var client = _clientFactory.CreateClient("AuctionAPI");
            var result = await client.GetFromJsonAsync<List<Auction>>("auctions");

            //UserId = userId;

            SelectedCategory = category;
            
            if (string.IsNullOrEmpty(category))
            {
                //domyœlna kategoria
                return RedirectToPage("/AuctionCategories", new { category = "Ksi¹¿ki", userId = userId });
            }

            //filtrowanie: pokazuje tylko aukcje z wybranej kategorii, niezakoñczone, niemoje
            Auctions = result?
                .Where(a => a.Category == category && !a.IsAuctionOver && a.UserID != userId)
                .ToList() ?? new List<Auction>();
            
            return Page();
        }
    }
}
