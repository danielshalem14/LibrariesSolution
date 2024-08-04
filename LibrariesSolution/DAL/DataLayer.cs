using LibrariesSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrariesSolution.DAL
{
    public class DataLayer : DbContext
    {
        public DataLayer(string cs) : base(GetOptions(cs))
        {
            Database.EnsureCreated();

            Seed();
        }

        private void Seed()
        {
            if (!Libraries.Any())
            {
                Library library = new Library { Genre = "fantazy" };
                library.ShelfList = CreateDefaultShelfList(library);
                Libraries.Add(library);
                SaveChanges();
            }

        }

        private List<Shelf> CreateDefaultShelfList(Library library)
        {
            List<Shelf> shelfList = new List<Shelf>();

            Shelf shelf = new Shelf { Height = 50, Width = 100, Library = library };

            shelf.BookList = CreateDefaultBookList(shelf);

            shelfList.Add(shelf);
            Shelves.Add(shelf);
            return shelfList;
        }

        private List<Book> CreateDefaultBookList(Shelf shelf)
        {
            List<Book> bookList = new List<Book>();

            Book book = new Book { Height = 10, Width = 10, Title = "Harry Potter", Shelf = shelf };

            bookList.Add(book);
            Books.Add(book);
            return bookList;
        }


        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Shelf> Shelves { get; set; }


        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), connectionString)
                .Options;
        }
    }
}
