namespace ReadRate.Models
{
    public class Following
    {
        public int FollowingId { get; set; }
        public int UserId { get; set; }
        public int FollowingUserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
