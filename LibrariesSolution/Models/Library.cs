using System.ComponentModel.DataAnnotations;

namespace LibrariesSolution.Models
{
    public class Library
    {

        public Library()
        {
            ShelfList = new List<Shelf>();
        }

        [Key]
        public int Id { get; set; }

        public string Genre { get; set; } = string.Empty;

        public List<Shelf> ShelfList { get; set; }
    }
}
