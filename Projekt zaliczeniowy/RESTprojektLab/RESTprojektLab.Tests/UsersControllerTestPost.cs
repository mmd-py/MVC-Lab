using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    //testowanie metody POST kontrolera Users
    //oczekuję, że doda użytkownika do bazy danych
    public class UsersControllerTestPost
    {
        //tworzy kontekst bd w pamięci, czyli nie używa prawdziwej bd tylko tymczasową
        private AuctionDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AuctionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AuctionDbContext(options);
        }

        //atrubut xUnit; mówi, że to test jednostkowy
        [Fact]
        public async Task Post_AddsUser()
        {
            var context = GetInMemoryDbContext(); //nowy kontekst na potrzeby testów
            var controller = new UsersController(context); //instancja kontrolera z przekazanym kontekstem

            //tworzy nowy obiekt użytkownika z danymi
            var user = new User
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Login = "harrypotter",
                Password = "admin123",
                IsOver18 = true,
                AcceptsRegulations = true
            };

            //wywołuje post, result to odpowiedź http zwrócona przez metodę
            var result = await controller.Post(user);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result); //sprawdza, czy zwróciło 201
            var returnedUser = Assert.IsType<User>(createdResult.Value); //wyciąga obiekt i sprawdza czy jest typu User
            Assert.Equal("harrypotter", returnedUser.Login); //czy właściwość login zwróconego użytkownika ma prawidłowe dane
        }
    }
}
