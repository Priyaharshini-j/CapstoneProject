namespace ReadRate.Models
{
    public class CommunityDiscussion
    {
        public int DiscussionId { get; set; }
        public int CommunityId { get; set; }
        public int CommunityMemberId { get; set; }
        public string Discussion { get; set; }
        public DateTime CreatedDate { get; set; }
        public Result result { get; set; }
    }
}
