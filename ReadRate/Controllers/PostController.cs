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
        public async Task<List<PostModel>> GetPosts(BookDetails book)
        {
            _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
            _conn.Open();
            List<PostModel> postList = new List<PostModel>();
            int bookId = await supplementaryController.getBookIdByISBN(book);
            try
            {
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetPostByBookId", _conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("Post Found");
                        while (reader.Read())
                        {
                            PostModel post = new PostModel();
                            post.PostId = Convert.ToInt32(reader["PostId"]);
                            post.PostCaption = reader["PostCaption"].ToString();
                            post.UserId = Convert.ToInt32(reader["UserId"]);
                            post.UserName = supplementaryController.FetchNameById(post.UserId);
                            post.BookId = Convert.ToInt32(reader["BookId"]);
                            post.Like = supplementaryController.GetPostLikeDislike(post.PostId)[1];
                            post.DisLike = supplementaryController.GetPostLikeDislike(post.PostId)[0];
                            post.Picture = (byte[])reader["Picture"];
                            post.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
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

                    reader.Close();
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
            int? UserId = Context.HttpContext.Session.GetInt32("UserId");
            List<PostModel> UserPosts = new List<PostModel>();
            try
            {

                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
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

        [HttpPost, Route("[action]" , Name ="AddPostLikeDislike")]
        public Result AddPostLikeDislike(AddPostLikeDislike postLike)
        {
            Result result = new Result();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("CreatePostLikeDislike", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", postLike.UserId);
                    cmd.Parameters.AddWithValue("@PostId", postLike.PostId);
                    cmd.Parameters.AddWithValue("@LikeStatus", postLike.LikeStatus);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        result.result = true;
                        result.message = postLike.LikeStatus == 1 ? "Liked a Post" : "Disliked a Post";
                    }
                    else
                    {
                        result.result = false;
                        result.message = "Cannot Like";
                    }
                    result.result = true;
                   
                    
                }
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        [HttpPost, Route("[action]", Name = "CreatePost")]
        public Result CreatePost([FromForm] CreatePost post)
        {
            Result result = new Result();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                int Bookid = supplementaryController.GetBookId(post.ISBN);
                Console.WriteLine(post.PostCaption);
                Console.WriteLine("UserId", post.UserId);
                Console.WriteLine("This is ISBN",post.ISBN);
                Console.WriteLine("This is BookId", Bookid);
                byte[] pictureData;
                using (var memoryStream = new MemoryStream())
                {
                    post.Picture.CopyTo(memoryStream);
                    pictureData = memoryStream.ToArray();
                }
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("CreatePost", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PostCaption", post.PostCaption);
                    cmd.Parameters.AddWithValue("@BookId", Bookid);
                    cmd.Parameters.AddWithValue("@Picture", pictureData);
                    cmd.Parameters.AddWithValue("@UserId", post.UserId);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        result.result = true;
                        result.message = "Successfully Posted a Picture";
                    }
                    else{
                        result.result = false;
                        result.message = "Cannot Create";
                    }
                }
            }
            catch(Exception ex)
            {
                result.result = false;
                result.message = ex.Message;
            }
            return result;
        }

        [HttpDelete, Route("[action]", Name ="DeletePost")]
        public Result DeletePost (DeletePostModel postModel)
        {
            Result result = new Result();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using(_conn)
                {
                    SqlCommand cmd = new SqlCommand("DeletePost", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PostId", postModel.PostId);
                    cmd.Parameters.AddWithValue("@UserId", postModel.UserId);
                    int rows = cmd.ExecuteNonQuery();
                    if(rows > 0)
                    {
                        result.result = true;
                        result.message = "Deleted Successfully";
                    }
                    else
                    {
                        result.result = false;
                        result.message = "Could not delete the post";
                    }
                }
            }
            catch(Exception ex)
            {
                result.result = false;
                result.message = ex.Message;
            }
            return result;
        }
    }
}
