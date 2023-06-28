namespace ReadRate.Models
{
    public class DiscussionReply
    {
        public int DiscussionReplyId { get; set; }
        public int DiscussionId { get; set; }
        public int CommunityMemberId { get; set; }
        public string Reply { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual CommunityDiscussion CommunityDiscussion { get; set;}
    }
}
