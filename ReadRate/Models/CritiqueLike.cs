namespace ReadRate.Models
{
    public class CritiqueLike
    {
        public int CritiqueLikeId { get; set; }
        public int CritiqueId { get; set; }
        public int UserId { get; set; }
        public int LikeStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public Results result { get; set; }
    }
}
