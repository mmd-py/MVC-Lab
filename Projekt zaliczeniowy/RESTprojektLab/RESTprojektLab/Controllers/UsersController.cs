using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTprojektLab.Data;
using RESTprojektLab.Models;
using System;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTprojektLab.Controllers
{
    [Route("api/[controller]")]
    //ścieżka pod jakim adresem fizycznie będzie wystawione api
    [ApiController]
    //klasa to kontroler api
    public class UsersController : ControllerBase
    {
        //obiekt reprezentujący bazę danych do komunikacji z bazą
        private readonly AuctionDbContext _context;

        //konstruktor klasy UsersController
        //kontekst bazy jest wstrzykiwany do kontrolera przez wstrzykiwanie zależności
        public UsersController(AuctionDbContext context)
        {
            //przypisanie kobtekstu do prywatnego pola, żeby kontrloler mógł go używac w metodach
            _context = context;
        }

        // POST api/<UsersController>
        //a. Dodawanie użytkownika
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = user.UserID }, user);
        }

        // PUT api/<UsersController>/5
        //b. Edycja użytkownika 
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            //sprawdza id w url
            if (id != user.UserID)
                return BadRequest();
            else
                //oznacza obiekt jako zmodyfikowany
                _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                //musi zwrócić klientowi info o udanej operacji, bez odpowiedzi w ciele
                return NoContent();
            }
            //żebym nie próbowała edytować wcześniej usuniętego rekordu
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.UserID == id))
                    return NotFound();
                throw;
            }
        }

        // DELETE api/<UsersController>/5
        //c. Usuwanie użytkownika
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            else
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        // GET api/<UsersController>/5
        // d. Pobranie informacji o użytkowniku 
        [HttpGet("{id}")]
        //asynchroniczna metoda kontrolera, zwraca User albo błąd HTTP
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) 
                return NotFound();
            else
                return Ok(user);
        }
    }
}
