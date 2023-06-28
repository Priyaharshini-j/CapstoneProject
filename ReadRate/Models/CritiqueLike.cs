namespace ReadRate.Models
{
    public class CritiqueLike
    {
        public int CritiqueLikeId { get; set; }
        public int CritiqueId { get; set; }
        public int UserId { get; set; }
        public bool LikeStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
