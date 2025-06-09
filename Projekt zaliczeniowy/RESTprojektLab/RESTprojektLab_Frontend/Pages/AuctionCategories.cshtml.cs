using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RESTprojektLab_Frontend.Pages
{
    public class AuctionCategoriesModel : PageModel
    {
        public int UserId { get; set; }
        public void OnGet(int userId)
        {
            //userId to parametr, a UserId w³aœciwoœæ klasy
            UserId = userId;
        }
    }
}
