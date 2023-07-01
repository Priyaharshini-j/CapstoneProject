namespace ReadRate.Models
{
    public class RatingModel
    {
        public int RatingId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public  virtual Result result { get; set; }
    }

    public class UserRatingModel
    {

        public int RatingId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual BookModel BookDetails { get; set; }
    }

    public class UserRating
    {
        public  BookModel BookDetail { get; set; }
        public int userRating { get; set; }
        public Result result { get; set; }
    }
}
