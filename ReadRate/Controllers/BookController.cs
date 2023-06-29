using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using ReadRate.Models;
using System.Reflection.Metadata.Ecma335;
using System.Data;
using Newtonsoft.Json;
using ReadRate.Controllers;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

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
        public BookController(IConfiguration _configuration, SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
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
                            community.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            communities.Add(community);
                        }
                    }
                    else
                    {
                        BookCommunity comm = new BookCommunity();
                        comm.result = new Models.Results();
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
                comm.result = new Models.Results();
                comm.result.result = false;
                comm.result.message =ex.Message;
                communities.Add(comm);
            }
            return communities;
        }
        





        /*


                    

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]

                    [HttpPost, Route("[action]", Name = "")]
        */

    }

}
