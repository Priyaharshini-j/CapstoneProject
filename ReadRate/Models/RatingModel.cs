namespace ReadRate.Models
{
    public class RatingModel
    {
        public int RatingId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
