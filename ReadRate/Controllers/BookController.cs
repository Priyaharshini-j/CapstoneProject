using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ReadRate.Models;
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReadRate.Controllers
{

    [EnableCors("AllowSpecificOrigin")]
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
        public async Task<List<BookCommunity>> CommunityList(BookDetails book)
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
                        community.result = new Models.Result();
                        community.result.result = true;
                        community.result.message = "No community was created ";
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

        [HttpPost, Route("[action]" , Name ="AddToShelf")]
        public async Task<Result> AddingBook (AddBookShelf bookShelf)
        {
            Result addingBook = new Result();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using(_conn)
                {
                    SqlCommand cmd = new SqlCommand("AddBookShelf", _conn);
                    Console.WriteLine("1");
                    cmd.CommandType= CommandType.StoredProcedure;
                    int? userId = Context.HttpContext.Session.GetInt32("UserId");
                    int bookId = await supplementaryController.getBookIdByISBN(bookShelf.Book);
                    cmd.Parameters.AddWithValue("@UserId",userId);
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    cmd.Parameters.AddWithValue("@ReadingStatus", bookShelf.BookShelfName);
                    Console.WriteLine("1");
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("1");
                    addingBook.result = true;
                    addingBook.message = "Book Added to the Shelf";
                    Console.WriteLine("1");

                }
                _conn.Close();

            }
            catch (SqlException ex)
            {
                addingBook.result = false;
                addingBook.message = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return addingBook;
        }

        [HttpDelete, Route("[action]", Name = "DeleteMember")]
        public Result DeletingMember (BookCommunity community)
        {
            Result result = new Result();
            int? userId = Context.HttpContext.Session.GetInt32("UserId");
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("DeleteMember", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CommunityId", community.CommunityId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                    result.result = true;
                    result.message = "Unfollwed the community";
                }
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = ex.Message;
            }
            return result;
        }

        [HttpGet, Route("[action]", Name = "BookInShelf")]
        public List<BookInShelf> ListBookInShelf()
        {
            List<BookInShelf> shelf = new List<BookInShelf>();
            int? userId = Context.HttpContext.Session.GetInt32("UserId");
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("ListBookInShelf", _conn);
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
                            BookInShelf shelfBook = new BookInShelf();
                            shelfBook.BookShelfName = dr["ReadingStatus"].ToString();
                            int bookId = (int)dr["BookId"];
                            shelfBook.UserId = (int)dr["UserId"];
                            shelfBook.BookShelfId = (int)dr["BookShelfId"];
                            shelfBook.Book = supplementaryController.GetBookByBookId(bookId);
                            
                            shelf.Add(shelfBook);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BookInShelf shelfBook = new BookInShelf();
                shelfBook.result = new Result();
                shelfBook.result.result = false;
                shelfBook.result.message = ex.Message;
                Console.WriteLine(ex.Message.ToString());
                shelf.Add(shelfBook);
            }
            return shelf;
        }

        
        [HttpPost, Route("[action]", Name ="EditCommunity")]
        public Result EditCommunity(EditCommunity comm)
        {
           Result res = new Result();
            try
            {
                Console.WriteLine("1");
                int? userId = Context.HttpContext.Session.GetInt32("UserId");
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                Console.WriteLine("1");
                if (userId == comm.CommunityAdmin)
                {
                    Console.WriteLine("1");
                    using (_conn)
                    {
                        Console.WriteLine("1");
                        SqlCommand cmd = new SqlCommand("EditCommunity", _conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CommunityId", comm.CommunityId);
                        cmd.Parameters.AddWithValue("@CommunityName", comm.CommunityName);
                        cmd.Parameters.AddWithValue("@CommunityDesc", comm.CommunityDesc);
                        cmd.Parameters.AddWithValue("@CommunityAdmin", userId);
                        cmd.Parameters.AddWithValue("@BookId", comm.BookId);
                        Console.WriteLine("1");
                        cmd.ExecuteNonQuery();
                        res.result = true;
                        res.message = "Updated the community";
                    }
                }
                Console.WriteLine("1");

            }
            catch(Exception ex)
            {
                Console.WriteLine("1");
                res.result = false;
                res.message = ex.Message;
                Console.WriteLine(ex.ToString());
                Console.WriteLine("1");
            }
            return res;
        }


        [HttpDelete, Route("[action]", Name = "RemoveBook")]
        public Result RemoveBook(DeleteBookInShelf bookInShelf)
        {
            Result result = new Result();
            int? userId = Context.HttpContext.Session.GetInt32("UserId");
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("RemoveBook", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookShelfId", bookInShelf.BookShelfId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@BookId", bookInShelf.BookId);
                    cmd.Parameters.AddWithValue("@ReadingStatus", bookInShelf.ReadingStatus);
                    cmd.ExecuteNonQuery();
                    result.result = true;
                    result.message = "Successfully removed the book from the shelf";
                }
            }
            catch(Exception ex)
            {
                result.result = false;
                result.message = ex.Message;
                Console.WriteLine(ex.Message.ToString());
            }
            return result;
        }

        




        /*                                   

                   


                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]
        */

    }

}
