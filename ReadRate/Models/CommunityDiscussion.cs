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

    public class DiscussionList
    {
        public List<CommunityDiscussion> Discussions { get; set; }
        public List<string> UserName { get; set; }
        public Result result { get; set; }
    }
}
