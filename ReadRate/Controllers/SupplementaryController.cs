using Microsoft.AspNetCore.Mvc;
using ReadRate.Models;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;

using Microsoft.AspNetCore.Http;

namespace ReadRate.Controllers
{
    public class SupplementaryController : Controller
    {
        SqlConnection _conn;
        private readonly IConfiguration configuration;
        public SupplementaryController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<int> getBookIdByISBN(BookModel book)
        {
            int bookId = 0;
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    bookId = GetBookId(book.ISBN.ToString());
                    if (bookId == 0)
                    {
                        Console.WriteLine("Not Found");
                        try
                        {
                            SqlCommand cmd = new SqlCommand("InsertBook", _conn);
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                            cmd.Parameters.AddWithValue("@BookName", book.BookName);
                            cmd.Parameters.AddWithValue("@BookVol", book.BookVol);
                            cmd.Parameters.AddWithValue("@Genre", book.Genre);
                            cmd.Parameters.AddWithValue("@Author", book.Author);
                            cmd.Parameters.AddWithValue("@CoverUrl", book.CoverUrl);
                            cmd.Parameters.AddWithValue("@BookDesc", book.BookDesc);
                            cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
                            cmd.Parameters.AddWithValue("@PublishedDate", book.PublishedDate);
                            cmd.ExecuteNonQuery();
                            bookId = await getBookIdByISBN(book);
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return bookId;
        }

        public BookModel GetBookByBookId(int bookId)
        {
            BookModel book = new BookModel();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();

                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetBookByBookId", _conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            book.BookId = Convert.ToInt32(dr["BookId"]);
                            book.ISBN = dr["ISBN"].ToString();
                            book.BookName = dr["BookName"].ToString();
                            book.BookVol = dr["BookVol"].ToString();
                            book.Genre = dr["Genre"].ToString();
                            book.Author = dr["Author"].ToString();
                            book.CoverUrl = dr["CoverUrl"].ToString();
                            book.BookDesc = dr["BookDesc"].ToString();
                            book.Publisher = dr["Publisher"].ToString();
                            book.PublishedDate = dr["PublishedDate"].ToString();
                        }
                    }
                    else
                    {
                        book.result = new Models.Result();
                        book.result.result = true;
                        book.result.message = "Unable to fetch the Book Details";
                    }
                }
            }
            catch (Exception ex)
            {
                book.result = new Models.Result();
                book.result.result = false;
                book.result.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return book;
        }

        public int[] getCritqueLikeByCritiqieId(int critqueId)
        {

            int[] critiqueLike = new int[2];
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("CritiqueLikeDislike", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CritiqueId", critqueId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            int likeStatus = Convert.ToInt32(dr["LikeStatus"]);

                            if (likeStatus == -1)
                            {
                                critiqueLike[0] = critiqueLike[0] + 1;
                            }
                            else if (likeStatus == 1)
                            {
                                critiqueLike[1] = critiqueLike[1] + 1;
                            }
                        }
                    }
                    else
                    {
                        critiqueLike[0] = 0;
                        critiqueLike[1] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return critiqueLike;
        }

        public UserModel GetUser(int UserId)
        {
            UserModel user = new UserModel();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetUserById", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            user.UserId = Convert.ToInt32(dr["UserId"]);
                            user.UserName = dr["UserName"].ToString();
                            user.UserEmail = dr["UserEmail"].ToString();
                            user.Password = dr["Password"].ToString();
                            user.SecurityQn = dr["SecurityQn"].ToString();
                            user.SecurityAns = dr["SecurityAns"].ToString();
                            user.result = new Models.Result();
                            user.result.result = true;
                            user.result.message = "User Details are retrieved";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                user.result = new Models.Result();
                user.result.result = false;
                user.result.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return user;
        }

        public int GetBookId(string ISBN)
        {
            int bookId = 0;
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetBookId", _conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ISBN", ISBN);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        bookId = (int)dt.Rows[0]["BookId"];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return bookId;
        }

        public int NoOfCommMembers(int CommunityId)
        {
            int memberCount = 0;
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetCommunityMembersCount", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CommunityId", CommunityId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            memberCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                memberCount = 0;
            }
            return memberCount;
        }
        public int[] GetPostLikeDislike(int PostId)
        {
            int[] LikeDislike = new int[2];
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("PostLikeDislike", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PostId", PostId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            int likeStatus = Convert.ToInt32(dr["LikeStatus"]);

                            if (likeStatus == -1)
                            {
                                LikeDislike[0] = LikeDislike[0] + 1;
                            }
                            else if (likeStatus == 1)
                            {
                                LikeDislike[1] = LikeDislike[1] + 1;
                            }
                        }
                    }
                    else
                    {
                        LikeDislike[0] = 0;
                        LikeDislike[1] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return LikeDislike;
        }
        public BookCommunity GetCommunitybyId(int communityId)
        {
            BookCommunity community = new BookCommunity();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetCommunitybyId", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CommunityId", communityId);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return community;
        }

        public UserCritique GetCritiqueById(int CritiqueId)
        {
            UserCritique critique = new UserCritique();
            try
            {
                SqlCommand cmd = new SqlCommand("GetCritiqueById", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CritiqueId", CritiqueId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Console.WriteLine("Found");
                    foreach (DataRow dr in dt.Rows)
                    {
                        critique.CritiqueId = Convert.ToInt32(dr["CritiqueId"]);
                        critique.BookId = Convert.ToInt32(dr["BookId"]);
                        critique.UserId = Convert.ToInt32(dr["UserId"]);
                        critique.CritiqueDesc = dr["CritiqueDesc"].ToString();
                        critique.Like = getCritqueLikeByCritiqieId(critique.CritiqueId)[1];
                        critique.Dislike = getCritqueLikeByCritiqieId(critique.CritiqueId)[0];
                        critique.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        critique.result.result = true;
                        critique.result.message = "Found thwe critique";
                    }
                }
                else
                {
                    critique.result.result = false;
                    critique.result.message = "No Critique found on this ID";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return critique;
        }
        public List<CritiqueReply> GetCritiqueReplyById(int CritiqueId)
        {
            List<CritiqueReply> critiqueWithReply = new List<CritiqueReply>();
            try
            {
                SqlCommand cmd = new SqlCommand("GetCritiqueReplyById", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CritiqueId", CritiqueId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Console.WriteLine("Found");
                    foreach (DataRow dr in dt.Rows)
                    {
                        CritiqueReply criReply = new CritiqueReply();
                        criReply.CritiqueReplyId = (int)dr["CritiqueReplyId"];
                        criReply.CritiqueId = Convert.ToInt32(dr["CritiqueId"]);
                        criReply.UserId = Convert.ToInt32(dr["UserId"]);
                        criReply.Reply = dr["Reply"].ToString();
                        criReply.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        criReply.result.result = true;
                        criReply.result.message = "Found";
                        critiqueWithReply.Add(criReply);
                    }
                }
                else
                {
                    CritiqueReply reply = new CritiqueReply();
                    reply.result.result = false;
                    reply.result.message = "Not found";
                    Console.WriteLine("No Critique reply found");
                    critiqueWithReply.Add(reply);

                }
            }
            catch (Exception ex)
            {
                CritiqueReply reply = new CritiqueReply();
                reply.result.result = false;
                reply.result.message = ex.Message;
                Console.WriteLine(ex.Message);
                critiqueWithReply.Add(reply);
            }
            return critiqueWithReply;
        }
    }

}
