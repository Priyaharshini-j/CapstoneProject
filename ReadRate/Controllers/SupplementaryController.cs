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
                    SqlCommand cmd = new SqlCommand("GetBookId", _conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        bookId = (int)dt.Rows[0]["BookId"];
                    }
                    if (bookId == 0)
                    {
                        Console.WriteLine("Not Found");
                        try
                        {
                            cmd = new SqlCommand("InsertBook", _conn);
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
                        book.result = new Models.Results();
                        book.result.result = true;
                        book.result.message = "Unable to fetch the Book Details";
                    }
                }
            }
            catch (Exception ex)
            {
                book.result = new Models.Results();
                book.result.result = false;
                book.result.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return book;
        } 
    }

}
