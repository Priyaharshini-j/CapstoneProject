namespace ReadRate.Models
{
    public class Following
    {
        public int FollowingId { get; set; }
        public int UserId { get; set; }
        public int FollowingUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual UserModel followerDetail { get; set; }
        public virtual UserModel userDetail { get; set; }
        public Result result { get; set; }
    }
}
