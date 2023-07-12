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
        public Result result { get; set; }
    }

    public class CritiqueWithReply
    {
        public UserCritique critique { get; set; }
        public List<CritiqueReply> reply { get; set; }
        public Result Result { get; set; }
    }

    public class CreateCritiqueReply
    {
        public int CritiqueId { get; set; }
        public int userId { get;set ; } 
        public string Reply { get; set; }
    }
    public class ReplyForCritique
    {
        public int CritiqueReplyId { get; set; }
        public int CritiqueId { get; set; }
        public string userName { get; set; }
        public string Reply { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CritiqueReplyList
    {
        public List<ReplyForCritique> Reply { get; set; }
        public Result Result { get; set; }
    }
    public class getReplyByCritiqueId
    {
        public int CritiqueId { get; set; }
    }
}
