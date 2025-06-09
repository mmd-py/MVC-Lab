using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RESTprojektLab.Models;

namespace RESTprojektLab_Frontend.Pages
{
    public class EditProfileModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public EditProfileModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public User User { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            var client = _clientFactory.CreateClient("AuctionAPI");
            var user = await client.GetFromJsonAsync<User>($"users/{userId}");

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _clientFactory.CreateClient("AuctionAPI");
            var response = await client.PutAsJsonAsync($"users/{User.UserID}", User);
            
            if (response.IsSuccessStatusCode)
            {
                Message = "Profil zosta� zaktualizowany.";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                //Message = $"Nie uda�o si� edytowa� konta. B��d: {error}";
                Message = "Nie uda�o si� edytowa� konta.";
            }
            return Page();
        }
    }
}
