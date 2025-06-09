using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTprojektLab.Data;
using RESTprojektLab.Models;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTprojektLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        //inicjaluzje kontroler z dostępem do bazy danych przez AuctionDbContext
        private readonly AuctionDbContext _context;
        public AuctionsController(AuctionDbContext context)
        {
            _context = context;
        }

        // POST api/<AuctionsController>
        //a. Dodawanie przedmiotu do licytacji 
        [HttpPost]
        public async Task<ActionResult<Auction>> Post([FromBody] Auction auction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //ustawia currentprice na równe starting price, gdy current = 0
            if (auction.CurrentPrice == 0)
            {
                auction.CurrentPrice = auction.StartingPrice;
            }

            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = auction.AuctionID }, auction);
        }

        // PUT api/<AuctionsController>/5
        //b. Edycja przedmiotu do licytacji
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Auction auction)
        {
            if (id != auction.AuctionID)
                return BadRequest();
            else
                _context.Entry(auction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Auctions.Any(e => e.AuctionID == id))
                    return NotFound();
                throw;
            }
        }

        // DELETE api/<AuctionsController>/5
        //c. Usuwanie przedmiotu z licytacji 
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
                return NotFound();
            else
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        // GET api/<AuctionsController>/5
        //d. Pobranie informacji o przedmiocie na licytacji
        [HttpGet("{id}")]
        public async Task<ActionResult<Auction>> Get(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
                return NotFound();
            else
                return Ok(auction);
        }
        // GET api/<AuctionsController>/5
        //d. Pobranie wszystkich licytacji
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAll()
        {
            return await _context.Auctions.ToListAsync();
        }

        //GET
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctionsByUser(int userId)
        {
            var userAuctions = await _context.Auctions
                .Where(a => a.UserID == userId)
                .ToListAsync();
            if (userAuctions == null || !userAuctions.Any())
            {
                //return NotFound("Brak aukcji dla użytkownika");
                return Ok(userAuctions ?? new List<Auction>());
            }
            else
            {
                return Ok(userAuctions);
            }
        }
    }
}

