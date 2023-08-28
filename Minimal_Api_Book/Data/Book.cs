using System.ComponentModel.DataAnnotations;

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

    }
}
