namespace ReadRate.Models
{
    public class PostModel
    {
        public int PostId { get; set; }
        public string PostCaption { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public byte[] Picture { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
