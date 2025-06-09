using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RESTprojektLab.Models;

namespace RESTprojektLab_Frontend.Pages
{
    public class EditItemModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public EditItemModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        //Bind znaczy, ¿e dane przes³ane z formularza maj¹ byæ automatycznie powi¹zane z w³aœciwoœci¹
        [BindProperty]
        public Auction Auction { get; set; }

        //bez: tylko do komunikacji z widokiem
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int auctionId, int userId)
        {
            var client = _clientFactory.CreateClient("AuctionAPI");
            var auction = await client.GetFromJsonAsync<Auction>($"auctions/{auctionId}");

            if (auction == null || auction.UserID != userId)
            {
                return NotFound();
            }
            //gdy warunek false, program przechodzi dalej
            Auction = auction;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var client = _clientFactory.CreateClient("AuctionAPI");

            if (action == "delete")
            {
                var response = await client.DeleteAsync($"auctions/{Auction.AuctionID}");
                if (response.IsSuccessStatusCode)
                {
                    //przekierowanie po usuniêciu przedmiotu: z parametrem usera
                    return RedirectToPage("/Account", new { userId = Auction.UserID });
                }
                else
                {
                    Message = "Nie uda³o siê usun¹æ przedmiotu.";
                    return Page();
                }
            }

            //¿eby nie nadpisywaæ 0 w StartingPrice po edycji przedmiotu
            var originalAuction = await client.GetFromJsonAsync<Auction>($"auctions/{Auction.AuctionID}");
            if (originalAuction == null)
            {
                Message = "Nie znaleziono oryginalnej aukcji.";
                return Page();
            }

            Auction.StartingPrice = originalAuction.StartingPrice;

            //aktualizacja przedmiotu
            var updateResponse = await client.PutAsJsonAsync($"auctions/{Auction.AuctionID}", Auction);
            
            if (updateResponse.IsSuccessStatusCode)
            {
                Message = "Przedmiot zosta³ zaktualizowany.";
            }
            else
            {
                //var error = await response.Content.ReadAsStringAsync();
                //Message = $"Nie uda³o siê edytowaæ przedmiotu. B³¹d: {error}";
                Message = "Nie uda³o siê edytowaæ przedmiotu.";

            }
            return Page();
        }
    }
}
