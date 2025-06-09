using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RESTprojektLab.Models;

namespace RESTprojektLab_Frontend.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public RegisterModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }
        
        [BindProperty]
        public string Login { get; set; }
        
        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public bool IsOver18 { get; set; }

        [BindProperty]
        public bool AcceptsRegulations { get; set; }

        public string Message { get; set; }


        //metody obs³uguj¹ce ¿¹dania
        //<form method="post">
        public async Task<IActionResult> OnPostAsync()
        {
            var client = _clientFactory.CreateClient("AuctionAPI");

            var newUser = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Login = Login,
                Password = Password,
                IsOver18 = IsOver18,
                AcceptsRegulations = AcceptsRegulations
            };

            var response = await client.PostAsJsonAsync("users", newUser);

            if (response.IsSuccessStatusCode)
            {
                var createdUser = await response.Content.ReadFromJsonAsync<User>();
                //przekierowanie do kolejnego widoku
                return RedirectToPage("/Account", new { userId = createdUser.UserID });
            }
            else
            {
                Message = "Nie uda³o siê utworzyæ konta.";
                return Page();
            }
        }
    }
}
