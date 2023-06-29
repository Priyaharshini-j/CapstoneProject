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
        public PostController(IConfiguration _configuration, SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
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
                        post.result = new Models.Results();
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
                post.result = new Models.Results();
                post.result.result = false;
                post.result.message = ex.Message;
                postList.Add(post);
                Console.WriteLine(ex.Message);
            }
            return postList;
        }

    }
}
