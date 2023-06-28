namespace ReadRate.Models
{
    public class ClommunityMember
    {
        public int CommunityMemberId { get; set; }
        public int CommunityId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
