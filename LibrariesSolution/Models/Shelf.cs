using System.ComponentModel.DataAnnotations;

namespace LibrariesSolution.Models
{
    public class Shelf
    {
        public Shelf()
        {
            BookList = new List<Book>();
        }

        [Key]
        public int Id { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public Library Library { get; set; }

        public List<Book> BookList { get; set; }


    }
}
