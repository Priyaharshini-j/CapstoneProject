﻿using Microsoft.AspNetCore.Http;
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
        IHttpContextAccessor Context;
        HttpClient client = new HttpClient();
        public CritiqueController(IConfiguration _configuration, IHttpContextAccessor _context , SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
            Context = _context;
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
                CritiqueModel critique = new CritiqueModel();
                critique.result = new Models.Result();
                critique.result.result = false;
                critique.result.message = ex.Message;
                critiques.Add(critique);
                Console.WriteLine(ex.ToString);
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
                            critique.Like = supplementaryController.getCritqueLikeByCritiqieId(critique.CritiqueId)[1];
                            critique.Dislike = supplementaryController.getCritqueLikeByCritiqieId(critique.CritiqueId)[0];
                            critique.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            critiqueList.Add(critique);
                        }
                    }
                    else
                    {
                        UserCritique critique = new UserCritique();
                        critique.result = new Models.Result();
                        critique.result.result = true;
                        critique.result.message = "No Critique found for this book";
                        critiqueList.Add(critique);
                        Console.WriteLine("No Critique found for this book");
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
        public UserCritique CreattingCritique(CritiqueModel critiqueModel)
        {
            UserCritique critique = new UserCritique();
            int? UserId = Context.HttpContext.Session.GetInt32("UserId");
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("CreateCritique");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId",critiqueModel.BookId);
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
                            critique.CritiqueId = Convert.ToInt32(dr["CritiqueId"]);
                            critique.BookId = Convert.ToInt32(dr["BookId"]);
                            critique.UserId = Convert.ToInt32(dr["UserId"]);
                            critique.CritiqueDesc = dr["CritiqueDesc"].ToString();
                            critique.Like = supplementaryController.getCritqueLikeByCritiqieId(critique.CritiqueId)[1];
                            critique.Dislike = supplementaryController.getCritqueLikeByCritiqieId(critique.CritiqueId)[0];
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

        [HttpDelete, Route("[action]", Name ="DeleteCritique")]
        public Result DeleteCritique(int critiqueId)
        {
            Result result = new Result();
            try
            {
                int? UserId = Context.HttpContext.Session.GetInt32("UserId");
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("DeleteCritique");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CritiqueId", critiqueId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.ExecuteNonQuery();
                    result.result = true;
                    result.message = "Successfully Deleted Critique ";
                }
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = ex.Message;
            }
            return result;
        }
    }
}
