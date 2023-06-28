namespace ReadRate.Models
{
    public class CritiqueModel
    {
        public int CritiqueId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string CritiqueDesc { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual CritiqueLike Likes { get; set; }
    }
}
