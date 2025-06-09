using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTprojektLab.Controllers;
using RESTprojektLab.Data;
using RESTprojektLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTprojektLab.Tests
{
    public class UsersControllerTestGet
    {
        private AuctionDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AuctionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AuctionDbContext(options);
        }

        //gdy id istnieje
        //Do kontekstu tymczasowej bd tworzy i dodaje testowego użytkownika, wywołuje metodę GET(1), sprawdza czy wynik to OkObjectResult (HTTP 200), sprawdza, czy zwrócony obiekt to typ User i czy login zwróconego użytkownika się zgadza.
        [Fact]
        public async Task GetUserById()
        {
            var context = GetInMemoryDbContext(); 
            var controller = new UsersController(context);
            
            //dodaje testowego użytkownika
            var user = new User 
            {
                UserID = 1,
                FirstName = "testuser",
                LastName = "testuser",
                Login = "testuser",
                Password = "testuser",
                IsOver18 = true,
                AcceptsRegulations = true
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            //wywołuje get z parametrem
            var result = await controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result); 
            var returnedUser = Assert.IsType<User>(okResult.Value); 
            Assert.Equal("testuser", returnedUser.Login); 
        }

        //gdy id nie istnieje
        //tworzy pusty kontekst, wywołuje GET(100), sprawdza czy odpowiedź to 404 NotFound
        [Fact]
        public async Task GetWhenUserIdDoesNoExist()
        {
            var context = GetInMemoryDbContext();
            var controller = new UsersController(context);

            //wywołuje get z parametrem
            var result = await controller.Get(100);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
