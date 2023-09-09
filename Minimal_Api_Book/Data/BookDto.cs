namespace Minimal_Api_Book.Data
{
    public class BookDto
    {
        public string Titel { get; set; }
        public string About { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public bool Loan { get; set; }
        public int GenreId { get; set; }
    }
}
