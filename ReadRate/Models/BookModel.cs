namespace ReadRate.Models
{
    public class BookModel
    {
        public int BookId { get; set; } 
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string BookVol { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public string CoverUrl { get; set; }
        public string BookDesc { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public virtual RatingModel Rating { get; set; }
        public Results result { get; set; }

    }

}
