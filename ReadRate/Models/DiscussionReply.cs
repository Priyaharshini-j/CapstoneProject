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
        public Result result { get; set; }
    }

    public class ListDiscussionReply
    {
        public CommunityDiscussion communityDiscussion { get; set; }
        public List<DiscussionReply> discussionReply { get; set;}
        public List<string> UserName { get; set; }
        public Result result { get; set; }
    }


}
