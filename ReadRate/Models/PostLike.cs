namespace ReadRate.Models
{
    public class PostLike
    {
        public int LikeId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public bool LikeStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
