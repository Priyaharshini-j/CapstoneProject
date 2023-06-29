namespace ReadRate.Models
{
    public class CritiqueReply
    {
        public int CritiqueReplyId { get; set; }
        public int CritiqueId { get; set; }
        public int UserId { get; set; }
        public string Reply { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual CritiqueModel CritiqueModel { get; set; }
        public Results result { get; set; }
    }
}
