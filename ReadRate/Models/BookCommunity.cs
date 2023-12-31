﻿namespace ReadRate.Models
{
    public class BookCommunity
    {
        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
        public string CommunityDesc { get; set; }
        public int CommunityAdmin { get; set; }
        public int BookId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual int CommunityMembers { get; set; }
        public Result result { get; set; }
    }

    public class CreateCommunity
    {

        public string ISBN { get; set; }
        public int userId { get; set; }
        public string CommunityName { get; set; }
        public string CommunityDesc { get; set; }
        

    }
    public class AddMemberCommunity
    {
        public int CommunityId { get; set; }
        public int userId { get; set; }
    }
    public class EditCommunity
    {
        public string CommunityName { get; set; }
        public string CommunityDesc { get; set; }
        public int CommunityAdmin { get; set; }
        public int CommunityId { get; set; }
        public int BookId { get; set; }
    }

    public class CommunityList
    {
        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
        public string CommunityDesc { get; set; }
        public string CommunityAdmin { get; set; }
        public int BookId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual int CommunityMembers { get; set; }
        public Result result { get; set; }
    }
}
