using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ReadRate.Models;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReadRate.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        SqlConnection _conn;
        private readonly SupplementaryController supplementaryController;
        private readonly IConfiguration configuration;
        IHttpContextAccessor Context;
        HttpClient client = new HttpClient();

        public BookController(IConfiguration _configuration, IHttpContextAccessor _context, SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
            Context = _context;
        }

        [HttpPost, Route("[action]", Name = "BookCommunityList")]
        public async Task<List<BookCommunity>> CommunityList(BookModel book)
        {
            List<BookCommunity> communities = new List<BookCommunity>();
            int bookId = await supplementaryController.getBookIdByISBN(book); 
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetCommunityBookId", _conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        foreach (DataRow dr in dt.Rows)
                        {
                            BookCommunity community = new BookCommunity();
                            community.CommunityId = Convert.ToInt32(dr["CommunityId"]);
                            community.CommunityName = dr["CommunityName"].ToString();
                            community.CommunityDesc = dr["CommunityDesc"].ToString();
                            community.CommunityAdmin = Convert.ToInt32(dr["CommunityAdmin"]);
                            community.BookId = Convert.ToInt32(dr["BookId"]);
                            community.CommunityMembers = supplementaryController.NoOfCommMembers(community.CommunityId);
                            community.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            communities.Add(community);
                        }
                    }
                    else
                    {
                        BookCommunity comm = new BookCommunity();
                        comm.result = new Models.Result();
                        comm.result.result = true;
                        comm.result.message = "No Communities found";
                        communities.Add(comm);
                        Console.WriteLine("No Communities Found");
                    }
                }
            }
            catch (SqlException ex)
            {
                BookCommunity comm = new BookCommunity();
                comm.result = new Models.Result();
                comm.result.result = false;
                comm.result.message =ex.Message;
                communities.Add(comm);
            }
            return communities;
        }

        [HttpGet, Route("[action]", Name = "UserCommunity")]
        public List<BookCommunity> UserCommunityList()
        {
            List<BookCommunity> communities = new List<BookCommunity>();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                   // Context.HttpContext.Session.SetInt32("UserId", 1);
                    int? userId = Context.HttpContext.Session.GetInt32("UserId");
                    SqlCommand cmd = new SqlCommand("GetCommunityByUserId", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        foreach (DataRow dr in dt.Rows)
                        {
                            BookCommunity community = new BookCommunity();
                            community.CommunityId = Convert.ToInt32(dr["CommunityId"]);
                            community.CommunityName = dr["CommunityName"].ToString();
                            community.CommunityDesc = dr["CommunityDesc"].ToString();
                            community.CommunityAdmin = Convert.ToInt32(dr["CommunityAdmin"]);
                            community.CommunityMembers = supplementaryController.NoOfCommMembers(community.CommunityId);
                            community.BookId = Convert.ToInt32(dr["BookId"]);
                            community.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            communities.Add(community);
                        }
                    }
                    else
                    {
                        BookCommunity bookCommunity = new BookCommunity();
                        bookCommunity.result = new Models.Result();
                        bookCommunity.result.result = false;
                        bookCommunity.result.message = "No community was created by the User";
                        communities.Add(bookCommunity);
                    }
                    _conn.Close();
                }
            }
            catch (SqlException ex)
            {
                BookCommunity bookCommunity = new BookCommunity();
                bookCommunity.result = new Models.Result();
                bookCommunity.result.result = false;
                bookCommunity.result.message = ex.Message;
                communities.Add(bookCommunity);
                Console.WriteLine(ex.Message);
            }
            
            return communities;
        }

        [HttpPost, Route("[action]", Name = "CreateCommunity")]
        public BookCommunity CreateCommunity(CreateCommunity comm)
        {
            BookCommunity community = new BookCommunity();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                
                int? userId = Context.HttpContext.Session.GetInt32("UserId");
                int convertedUserId = userId.HasValue ? userId.Value : 0;
                int bookId =  supplementaryController.GetBookId(comm.ISBN);
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("CreateCommunity", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CommunityName", comm.CommunityName);
                    cmd.Parameters.AddWithValue("@CommunityDesc", comm.CommunityDesc);
                    cmd.Parameters.AddWithValue("@CommunityAdmin", convertedUserId);
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        foreach (DataRow dr in dt.Rows)
                        {
                            community.CommunityId = Convert.ToInt32(dr["CommunityId"]);
                            community.CommunityName = dr["CommunityName"].ToString();
                            community.CommunityDesc = dr["CommunityDesc"].ToString();
                            community.CommunityAdmin = Convert.ToInt32(dr["CommunityAdmin"]);
                            community.BookId = Convert.ToInt32(dr["BookId"]);
                            community.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            community.result = new Models.Result();
                            community.result.result = true;
                            community.result.message = "Community Created successfully";
                        }
                    }
                    else
                    {
                        BookCommunity bookCommunity = new BookCommunity();
                        bookCommunity.result = new Models.Result();
                        bookCommunity.result.result = true;
                        bookCommunity.result.message = "No community was created ";
                    }
                }
            }
            catch (SqlException ex)
            {
                community.result = new Models.Result();
                community.result.result = false;
                community.result.message = ex.Message;
            }
            return community;
        }

        [HttpDelete, Route("[action]", Name = "DeleteCommunity")]
        public Models.Result DeleteCommunity(int CommunityId)
        {
            Models.Result results = new Models.Result();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("DeleteCommunity", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                   // Context.HttpContext.Session.SetInt32("UserId", 1);
                    int? userId = Context.HttpContext.Session.GetInt32("UserId");
                    int convertedUserId = userId.HasValue ? userId.Value : 0;
                    cmd.Parameters.AddWithValue("@UserId", convertedUserId);
                    cmd.Parameters.AddWithValue("@CommunityId", CommunityId);
                    cmd.ExecuteNonQuery();
                    results.result = true;
                    results.message = "Community Was deleted successfully";
                }
                _conn.Close();
            }
            catch(SqlException ex)
            {
                results.result = false;
                results.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return results;
        }

        [HttpPost, Route("[action]", Name = "AddMember")]

        public Result AddMember(int CommunityId)
        {
            Models.Result results = new Models.Result();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("AddMember", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Context.HttpContext.Session.SetInt32("UserId", 1);
                    int? userId = Context.HttpContext.Session.GetInt32("UserId");
                    int convertedUserId = userId.HasValue ? userId.Value : 0;
                    cmd.Parameters.AddWithValue("@UserId", convertedUserId);
                    cmd.Parameters.AddWithValue("@CommunityId", CommunityId);
                    cmd.ExecuteNonQuery();
                    results.result = true;
                    results.message = "You are added as a Member ";
                }
                _conn.Close();
            }
            catch (SqlException ex)
            {
                results.result = false;
                results.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return results;
        }




        /*                                   

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]
        */

    }

}
