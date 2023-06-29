using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadRate.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace ReadRate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CritiqueController : ControllerBase
    {
        SqlConnection _conn;
        private readonly SupplementaryController supplementaryController;
        private readonly IConfiguration configuration;
        HttpClient client = new HttpClient();
        public CritiqueController(IConfiguration _configuration, SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
        }

        [HttpPost, Route("[action]", Name = "BookCritique")]
        public async Task<List<CritiqueModel>>CritiqueList(BookModel book)
        {
            _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
            _conn.Open();
            List<CritiqueModel> critiques = new List<CritiqueModel>();
            int bookId = await supplementaryController.getBookIdByISBN(book);
            try
            {
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetCritiqueByBookId", _conn);
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
                            CritiqueModel critique = new CritiqueModel();
                            critique.CritiqueId = Convert.ToInt32(dr["CritiqueId"]);
                            critique.BookId = Convert.ToInt32(dr["BookId"]);
                            critique.UserId = Convert.ToInt32(dr["UserId"]);
                            critique.CritiqueDesc = dr["CritiqueDesc"].ToString();
                            critique.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            critiques.Add(critique);
                        }
                    }
                    else
                    {
                        CritiqueModel critique = new CritiqueModel();
                        critique.result = new Models.Results();
                        critique.result.result = true;
                        critique.result.message = "No Critique found for this book";
                        critiques.Add(critique);
                        Console.WriteLine("No Critique found for this book");
                    }
                }
            }
            catch (SqlException ex)
            {
                CritiqueModel critique = new CritiqueModel();
                critique.result = new Models.Results();
                critique.result.result = false;
                critique.result.message = ex.Message;
                critiques.Add(critique);
                Console.WriteLine(ex.ToString);
            }
            _conn.Close();
            return critiques;
        }
       

    }
}
