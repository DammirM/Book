using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Minimal_Api_Book.Data
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        [Required]
        public string GenreName { get; set;}
        [JsonIgnore]
        public ICollection<Book> Books { get; set; }
    }
}
