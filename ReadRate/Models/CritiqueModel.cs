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
        public Result result { get; set; }
    }

    public class UserCritique
    {
        public int CritiqueId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string CritiqueDesc { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public Result result { get; set; }
    }
}
