using Microsoft.EntityFrameworkCore;
using RESTprojektLab.Models;

namespace RESTprojektLab.Data
{
    public class AuctionDbContext : DbContext
    {
        //konstruktor
        //parametr przekazuje ustawienia połączenia z bazą danych, przekazuje options do klasy bazowej
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { 
        }

        //właściwości klasy
        //DbSet<T> to representacja tabeli, T to encja
        //User to klasa encji, musi być zgodna z modelem?, Users to właściwość typu DbSet<User>, czyli tabela w bazie
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }

        //kaskadowe usuwanie: usuwanie usera razem z jego aukcjami
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Auctions) //1 user wiele aukcji
                .WithOne(a => a.User) //aukcja ma 1 usera
                .HasForeignKey(async => async.UserID) //fk w Auction to UserID
                .OnDelete(DeleteBehavior.Cascade); //usuń aukcje usera razem z userem
        }
    }
}
