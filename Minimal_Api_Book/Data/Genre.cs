using System.ComponentModel.DataAnnotations;

namespace Minimal_Api_Book.Data
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        [Required]
        public string GenreName { get; set;}

        public ICollection<Book> Books { get; set; }
    }
}
