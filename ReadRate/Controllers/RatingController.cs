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
    public class RatingController : ControllerBase
    {
        SqlConnection _conn;
        private readonly SupplementaryController supplementaryController;
        private readonly IConfiguration configuration;
        public RatingController(IConfiguration _configuration, SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
        }

        [HttpPost, Route("[action]", Name = "GetRatingsForBook")]
        public async Task<float> GetRating(BookModel book)
        {
            float rating = 0.0f;
            _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
            _conn.Open();
            int bookId = await supplementaryController.getBookIdByISBN(book);
            try
            {
                SqlCommand cmd = new SqlCommand("getRatingsByBookId", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookId", bookId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        rating = rating+ Convert.ToInt32(dr["Rating"]);
                    }
                    rating = rating/dt.Rows.Count;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            _conn.Close();
            return rating;
        }

    }
}
