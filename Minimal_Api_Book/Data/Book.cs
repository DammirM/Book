using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Minimal_Api_Book.Data
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        
        public string Titel { get; set; }
        public string About { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public bool Loan { get; set; }
        public int GenreId { get; set; }
        [JsonIgnore]
        public Genre Genre { get; set; }

    }
}
