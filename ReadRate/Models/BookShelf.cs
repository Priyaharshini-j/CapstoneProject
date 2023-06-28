namespace ReadRate.Models
{
    public class BookShelf
    {
        public int BookShelfId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string ReadingStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
