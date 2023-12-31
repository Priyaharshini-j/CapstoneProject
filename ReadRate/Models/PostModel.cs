﻿namespace ReadRate.Models
{
    public class PostModel
    {
        public int PostId { get; set; }
        public string PostCaption { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BookId { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public byte[] Picture { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Result result { get; set; }
    }
    public class CreatePost
    {
        public string PostCaption { get; set; }
        public IFormFile Picture { get; set; }
        public string ISBN { get; set; }
        public int UserId { get; set; }

    }

    public class DeletePostModel
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
