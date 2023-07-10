using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadRate.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace ReadRate.Controllers
{

    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CritiqueController : ControllerBase
    {
        SqlConnection _conn;
        private readonly SupplementaryController supplementaryController;
        private readonly IConfiguration configuration;
        IHttpContextAccessor Context;
        HttpClient client = new HttpClient();
        public CritiqueController(IConfiguration _configuration, IHttpContextAccessor _context , SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
            Context = _context;
        }

        [HttpPost, Route("[action]", Name = "BookCritique")]
        public async Task<List<CritiqueList>>CritiqueList(BookDetails book)
        {
            _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
            _conn.Open();
            List<CritiqueList> critiques = new List<CritiqueList>();
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
                            CritiqueList critique = new CritiqueList();
                            int criti_Id = Convert.ToInt32(dr["CritiqueId"]);
                            critique.CritiqueId = Convert.ToInt32(dr["CritiqueId"]);
                            critique.BookId = Convert.ToInt32(dr["BookId"]);
                            int UserId = Convert.ToInt32(dr["UserId"]);
                            critique.UserName = supplementaryController.FetchNameById(UserId);
                            critique.CritiqueDesc = dr["CritiqueDesc"].ToString();
                            critique.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            critique.Like = supplementaryController.getCritqueLikeByCritiqueId(criti_Id)[1]; 
                            critique.Dislike = supplementaryController.getCritqueLikeByCritiqueId(criti_Id)[0];
                            critiques.Add(critique);
                        }
                    }
                    else
                    {
                        CritiqueList critique = new CritiqueList();
                        critique.result = new Models.Result();
                        critique.result.result = true;
                        critique.result.message = "No Critique found for this book";
                        critiques.Add(critique);
                        Console.WriteLine("No Critique found for this book");
                    }
                }
            }
            catch (SqlException ex)
            {
                CritiqueList critique = new CritiqueList();
                critique.result = new Models.Result();
                critique.result.result = false;
                critique.result.message = ex.Message;
                critiques.Add(critique);
                Console.WriteLine(ex.ToString());
            }
            _conn.Close();
            return critiques;
        }

        [HttpGet , Route("[action]", Name = "UserCritique")]
        public List<UserCritique> UsersCritiqueList()
        {
            List<UserCritique> critiqueList = new List<UserCritique>();
           
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("GetUsersCritique", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int? userId = Context.HttpContext.Session.GetInt32("UserId");
                    cmd.Parameters.AddWithValue("UserId", userId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        foreach (DataRow dr in dt.Rows)
                        {
                            UserCritique critique = new UserCritique();
                            critique.CritiqueId = Convert.ToInt32(dr["CritiqueId"]);
                            critique.BookId = Convert.ToInt32(dr["BookId"]);
                            critique.UserId = Convert.ToInt32(dr["UserId"]);
                            critique.CritiqueDesc = dr["CritiqueDesc"].ToString();
                            critique.Like = supplementaryController.getCritqueLikeByCritiqueId(critique.CritiqueId)[1];
                            critique.Dislike = supplementaryController.getCritqueLikeByCritiqueId(critique.CritiqueId)[0];
                            critique.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            critiqueList.Add(critique);
                        }
                    }
                    else
                    {
                        UserCritique critique = new UserCritique();
                        critique.result = new Models.Result();
                        critique.result.result = true;
                        critique.result.message = "No Critique found for this user";
                        critiqueList.Add(critique);
                        Console.WriteLine("No Critique found for this user");
                    }

                }
            }
            catch (SqlException ex)
            {
                UserCritique critique = new UserCritique();
                critique.result = new Models.Result();
                critique.result.result = false;
                critique.result.message = ex.Message;
                critiqueList.Add(critique);
            }
            return critiqueList;
        }

        [HttpPost, Route("[action]", Name = "CreatingCritique")]
        public UserCritique CreatingCritique(CreatingCritique critiqueModel)
        {
            UserCritique critique = new UserCritique();
            int? UserId = critiqueModel.UserId;
            int BookId = supplementaryController.GetBookId(critiqueModel.ISBN);
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("CreateCritique",_conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", BookId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@CritiqueDesc", critiqueModel.CritiqueDesc);
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
                            critique.Like = supplementaryController.getCritqueLikeByCritiqueId(critique.CritiqueId)[1];
                            critique.Dislike = supplementaryController.getCritqueLikeByCritiqueId(critique.CritiqueId)[0];
                            critique.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }
                    }
                    else
                    {
                        critique.result = new Models.Result();
                        critique.result.result = true;
                        critique.result.message = "No Critique found for this book";
                        Console.WriteLine("No Critique found for this book");
                    }
                }
            }
            catch (SqlException ex)
            {
                critique.result.result = false;
                critique.result.message = ex.Message;
            }
            return critique;
        }
        /*
        [HttpDelete, Route("[action]", Name ="DeleteCritique")]
        public Result DeleteCritique(int critiqueId)
        {
            Result result = new Result();
                int? UserId = Context.HttpContext.Session.GetInt32("UserId");
                try
                {
                    _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                    _conn.Open();
                    using (_conn)
                    {
                    Console.WriteLine("3");
                    SqlCommand cmd = new SqlCommand("DeleteCritique", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CritiqueId", critiqueId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    Console.WriteLine("3");
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine("3");
                    if (rowsAffected > 0)
                    {
                        result.result = true;
                        result.message = "Successfully deleted critique.";
                    }
                    else
                    {
                        result.result = false;
                        result.message = "Failed to delete critique.";
                    }
                    Console.WriteLine("3");

                }
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = ex.Message;
            }
            return result;
        }
        */
        [HttpPost, Route("[action]", Name = "CreatingCritiqueReply")]
        public CritiqueWithReply CreatingCritiqueReply(CreateCritiqueReply critiqueReply)
        {
            CritiqueWithReply critiqueWithReply = new CritiqueWithReply();
            try
            {
                int? UserId = Context.HttpContext.Session.GetInt32("UserId");
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("CreateCritiqueReply", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CritiqueId", critiqueReply.CritiqueId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@Reply", critiqueReply.Reply);
                    critiqueWithReply.critique = new UserCritique();
                    critiqueWithReply.critique = supplementaryController.GetCritiqueById(critiqueReply.CritiqueId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        critiqueWithReply.reply = new List<CritiqueReply>(); // Initialize the reply list
                        foreach (DataRow dr in dt.Rows)
                        {

                            CritiqueReply criReply = new CritiqueReply();
                            criReply.CritiqueReplyId = (int)dr["CritiqueReplyId"];
                            criReply.CritiqueId = Convert.ToInt32(dr["CritiqueId"]);
                            criReply.UserId = Convert.ToInt32(dr["UserId"]);
                            criReply.Reply = dr["Reply"].ToString();
                            criReply.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            critiqueWithReply.reply.Add(criReply);
                        }
                    }
                    else
                    {
                        critiqueWithReply.Result = new Models.Result();
                        critiqueWithReply.Result.result = true;
                        critiqueWithReply.Result.message = "Critique reply was not added";
                        Console.WriteLine("Critique reply was not added");
                    }
                }
            }
            catch (Exception ex)
            {
                critiqueWithReply.Result = new Models.Result();
                critiqueWithReply.Result.result = false;
                critiqueWithReply.Result.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return critiqueWithReply;
        }

        /*
        [HttpPost, Route("[action]", Name = "GetCritiqueReply")]
        public CritiqueWithReply GetCritiqueAndReply(int critiqueId)
        {
            CritiqueWithReply critiqueWithReply = new CritiqueWithReply();
            try
            {
                critiqueWithReply.critique = supplementaryController.GetCritiqueById(critiqueId);
                critiqueWithReply.reply = supplementaryController.GetCritiqueReplyById(critiqueId);
                critiqueWithReply.Result.result = true;
                critiqueWithReply.Result.message = "List of reply and critique returned";
            }
            catch (Exception ex)
            {
                critiqueWithReply.Result.result = false;
                critiqueWithReply.Result.message= ex.Message;
            }
            return critiqueWithReply;
        }
        */
        [HttpPost, Route("[action]", Name ="CritiqueLikeDislike")]
        public Result LikeDislikeCritique(CritiqueLikeDislike critiqueLikeDislike )
        {
            Result result = new Result();
            try
            {
                int? UserId = Context.HttpContext.Session.GetInt32("UserId");
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("LikeDislikeCritique", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CritiqueId", critiqueLikeDislike.critiqueId);
                    cmd.Parameters.AddWithValue("@UserId", critiqueLikeDislike.UserId);
                    cmd.Parameters.AddWithValue("@LikeStatus", critiqueLikeDislike.LikeStatus);
                    cmd.ExecuteNonQuery();
                    result.result = true;
                    result.message = "Liked a Critique";
                }
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = ex.Message;
            }
            return result;
        }

        [HttpPut, Route("[action]", Name = "EditCritique")]
        public UserCritique EditCritique(EditCritiqueDesc critiqueModel)
        {
            int? UserId = Context.HttpContext.Session.GetInt32("UserId");
            UserCritique editedCritique = new UserCritique();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("EditCritique", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CritiqueId", critiqueModel.CritiqueId);
                    cmd.Parameters.AddWithValue("@BookId", critiqueModel.BookId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("CritiqueDesc", critiqueModel.CritiqueDesc);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        foreach (DataRow dr in dt.Rows)
                        {
                            editedCritique.CritiqueId = Convert.ToInt32(dr["CritiqueId"]);
                            editedCritique.BookId = Convert.ToInt32(dr["BookId"]);
                            editedCritique.UserId = Convert.ToInt32(dr["UserId"]);
                            editedCritique.CritiqueDesc = dr["CritiqueDesc"].ToString();
                            editedCritique.Like = supplementaryController.getCritqueLikeByCritiqueId(editedCritique.CritiqueId)[1];
                            editedCritique.Dislike = supplementaryController.getCritqueLikeByCritiqueId(editedCritique.CritiqueId)[0];
                            editedCritique.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }
                    }
                    else
                    {
                        editedCritique.result = new Models.Result();
                        editedCritique.result.result = true;
                        editedCritique.result.message = "No Critique found";
                        Console.WriteLine("No Critique found");
                    }
                }
            }
            catch (SqlException ex)
            {
                editedCritique.result.result = false;
                editedCritique.result.message = ex.Message;
            }
            return editedCritique;
        }

        [HttpPut, Route("[action]", Name = "EditedCritiqueReply")]
        public Result EditedCritiqueReply(EditCritiqueReply reply)
        {
            Result result = new Result();
            try
            {
                int? UserId = Context.HttpContext.Session.GetInt32("UserId");
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("EditCritiqueReply", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@CritiqueReplyId", reply.CritiqueReplyId);
                    cmd.Parameters.AddWithValue("@CritiqueId", reply.CritiqueId);
                    cmd.Parameters.AddWithValue("@Reply", reply.Reply);
                    cmd.ExecuteNonQuery();
                    result.result = true;
                    result.message = "Reply edited successfully";
                }
            }
            catch(Exception ex)
            {
               result.result= false;
               result.message = ex.Message;
            }
            return result;
        }




        /*
        [HttpDelete, Route("[action]", Name ="DeleteCritiqueReply")]
        public Result DeleteReply(int CritiqueReplyId)
        {
            Result result = new Result();
            try
            {

                int? UserId = Context.HttpContext.Session.GetInt32("UserId");
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("DeleteCritiqueReply", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CritiqueReplyId", CritiqueReplyId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.ExecuteNonQuery();
                    result.result = true;
                    result.message = "Successfull in deletion";
                }
            }
            catch(Exception ex)
            { 
                result.result = false;
                result.message = ex.Message; 
            }
            return result;
        }
        */
    }
}
