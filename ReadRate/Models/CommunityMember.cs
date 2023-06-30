namespace ReadRate.Models
{
    public class CommunityMember
    {
        public int CommunityMemberId { get; set; }
        public int CommunityId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual UserModel User { get; set; }
        public Result result { get; set; }
    }
}
