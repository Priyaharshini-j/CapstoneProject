using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using ReadRate.Models;
using System.Reflection.Metadata.Ecma335;
using System.Data;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReadRate.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        SqlConnection _conn;
        private readonly IConfiguration configuration;
        IHttpContextAccessor Context;
        HttpClient client = new HttpClient();
        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpGet, Route("[action]", Name = "BookCommunityList")]
        public Models.Results CommunityList(int ISBN)
        {
            Models.Results result = new Models.Results();
            int bookId = getBookIdByISBN(ISBN);


            return result;
        }
        public async Task<int> getBookIdByISBN(int ISBN)
        {
            int bookId = 0;
            try
            {
                _conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetBookId",_conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ISBN", ISBN);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        bookId = (int) dt.Rows[0]["BookId"];
                    }
                    if(dt == null && dt.Rows.Count == 0)
                    {
                        // Set the base URL of the third-party API
                        string baseUrl = "https://api.example.com/";

                        // Set the endpoint for retrieving book details
                        string endpoint = $"books/{ISBN}";

                        // Send the GET request to the third-party API
                        HttpResponseMessage response = await client.GetAsync(baseUrl + endpoint);

                        // Check if the request was successful (status code 200)
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the response content as a string
                            string responseContent = await response.Content.ReadAsStringAsync();

                            // Deserialize the response content into a Book object
                            Book book = JsonConvert.DeserializeObject<Book>(responseContent);

                            // Process the book details or save them to the database

                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return bookId;
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

                    [HttpPost, Route("[action]", Name = "")]
        */

    }

}
