using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RESTprojektLab.Models;

namespace RESTprojektLab_Frontend.Pages
{
    public class AccountModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public User User { get; set; }
        public List<Auction> UserAuctions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            var client = _clientFactory.CreateClient("AuctionAPI");

            User = await client.GetFromJsonAsync<User>($"users/{userId}");
            UserAuctions = await client.GetFromJsonAsync<List<Auction>>($"auctions/user/{userId}");
            
            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return Page();
            }
        }
        public async Task<IActionResult> OnPostDeleteAccountAsync(int userId)
        {
            var client = _clientFactory.CreateClient("AuctionAPI");
            
            var response = await client.DeleteAsync($"users/{userId}");

            if (response.IsSuccessStatusCode)
            {
                //przekierowanie po usuniêciu konta
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nie uda³o siê usun¹æ konta.");
                return Page();
            }
        }
    }
}
