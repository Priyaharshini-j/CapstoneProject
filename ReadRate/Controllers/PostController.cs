using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadRate.Models;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace ReadRate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        SqlConnection _conn;
        private readonly SupplementaryController supplementaryController;
        private readonly IConfiguration configuration;
        IHttpContextAccessor Context;
        public PostController(IConfiguration _configuration, IHttpContextAccessor _context , SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
            Context = _context;
        }
        [HttpPost, Route("[action]", Name = "GetPostByBookId")]
        public async Task<List<PostModel>> GetPosts(BookModel book)
        {
            _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
            _conn.Open();
            List<PostModel> postList = new List<PostModel>();
            int bookId = await supplementaryController.getBookIdByISBN(book);
            try
            {
                using(_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetPostByBookId", _conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Post Found");
                        foreach (DataRow dr in dt.Rows)
                        {
                            PostModel post = new PostModel();
                            post.PostId = Convert.ToInt32(dr["CritiqueId"]);
                            post.PostCaption = dr["PostCaption"].ToString();
                            post.UserId = Convert.ToInt32(dr["UserId"]);
                            post.BookId = Convert.ToInt32(dr["BookId"]);
                            post.Picture = (byte[])dr["Picture"];
                            post.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            postList.Add(post);
                        }
                    }
                    else
                    {
                        PostModel post = new PostModel();
                        post.result = new Models.Result();
                        post.result.result = true;
                        post.result.message = "No Post found for this book";
                        postList.Add(post);
                        Console.WriteLine("No Post found for this book");
                    }
                }
            }
            catch (Exception ex)
            {
                PostModel post = new PostModel();
                post.result = new Models.Result();
                post.result.result = false;
                post.result.message = ex.Message;
                postList.Add(post);
                Console.WriteLine(ex.Message);
            }
            return postList;
        }

        [HttpGet, Route("[action]", Name = "UserPost")]
        public List<PostModel> UsersPost()
        {
            _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
            _conn.Open();
            int? UserId = Context.HttpContext.Session.GetInt32("UserId");
            List<PostModel> UserPosts = new List<PostModel>();
            try
            {
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetUsersPost", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;      
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Post Found");
                        foreach (DataRow dr in dt.Rows)
                        {
                            PostModel post = new PostModel();
                            post.PostId = Convert.ToInt32(dr["CritiqueId"]);
                            post.PostCaption = dr["PostCaption"].ToString();
                            post.UserId = Convert.ToInt32(dr["UserId"]);
                            post.BookId = Convert.ToInt32(dr["BookId"]);
                            post.Picture = (byte[])dr["Picture"];
                            post.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            UserPosts.Add(post);
                        }
                    }
                    else
                    {
                        PostModel post = new PostModel();
                        post.result = new Models.Result();
                        post.result.result = true;
                        post.result.message = "No Post found for this book";
                        UserPosts.Add(post);
                        Console.WriteLine("No Post found for this book");
                    }
                }
            }
            catch (Exception ex)
            {
                PostModel post = new PostModel();
                post.result = new Models.Result();
                post.result.result = false;
                post.result.message = ex.Message;
                UserPosts.Add(post);
                Console.WriteLine(ex.Message);
            }
            _conn.Close();
            return UserPosts;
        }
    }
}
