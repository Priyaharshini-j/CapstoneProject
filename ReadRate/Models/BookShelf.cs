namespace ReadRate.Models
{
    public class BookShelf
    {
        public int BookShelfId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string ReadingStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual BookModel Book { get; set; }
        public Result result { get; set; }
    }

    public class AddBookShelf
    {
        public virtual BookModel Book { get; set; }
        public string BookShelfName { get; set; }
        public Result result { get; set; }
    }
}
