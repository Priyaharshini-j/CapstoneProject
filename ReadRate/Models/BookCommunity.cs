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
        public Results result { get; set; }
    }

    public class CreateCommunity
    {
        public string CommunityName { get; set; }
        public string CommunityDesc { get; set; }
        public string ISBN { get; set; }

    }
}
