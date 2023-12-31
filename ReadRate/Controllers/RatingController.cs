﻿using Microsoft.AspNetCore.Cors;
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
    public class RatingController : ControllerBase
    {
        SqlConnection _conn;
        private readonly SupplementaryController supplementaryController;
        private readonly IConfiguration configuration;
        IHttpContextAccessor Context;
        public RatingController(IConfiguration _configuration, IHttpContextAccessor _context , SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
            Context = _context;
        }

        [HttpPost, Route("[action]", Name = "GetRatingsForBook")]
        public async Task<float> GetRating(BookDetails book)
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
                        rating = rating + Convert.ToInt32(dr["Rating"]);
                    }
                    rating = rating / dt.Rows.Count;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            _conn.Close();
            return rating;
        }

        [HttpPost, Route("[action]", Name = "UsersRating")]
        public List<UserRatingModel> UsersRating(UserDetail userDetail)
        {
            _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
            _conn.Open();
            List<UserRatingModel> ratings = new List<UserRatingModel>();
            try
            {
                SqlCommand cmd = new SqlCommand("getUsersRating", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userDetail.userId);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        UserRatingModel userRatingModel = new UserRatingModel();
                        userRatingModel.RatingId = Convert.ToInt32(dr["RatingId"]);
                        userRatingModel.BookId = Convert.ToInt32(dr["BookId"]);
                        userRatingModel.UserId = Convert.ToInt32(dr["UserId"]);
                        userRatingModel.Rating = Convert.ToInt32(dr["Rating"]);
                        userRatingModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        userRatingModel.BookDetails = supplementaryController.GetBookByBookId(userRatingModel.BookId);
                        ratings.Add(userRatingModel);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
           return ratings;
        }

        [HttpPost, Route("[action]", Name = "AddRating")]
        public async Task<UserRating> AddRating(AddRatingModel ratingModel)
        {
            UserRating rating = new UserRating();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                int bookId = supplementaryController.GetBookId(ratingModel.ISBN);
                SqlCommand cmd = new SqlCommand("AddRating", _conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userRatings", ratingModel.Rating);
                cmd.Parameters.AddWithValue("@UserId", ratingModel.userId);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                if (dr.Read())
                {
                    rating.userRating = ratingModel.Rating;
                    rating.BookDetail = new BookModel();
                    rating.BookDetail.BookId = Convert.ToInt32(dr["BookId"]);
                    rating.BookDetail.BookName = dr["BookName"].ToString();
                    rating.BookDetail.ISBN = dr["ISBN"].ToString();
                    rating.BookDetail.Genre = dr["Genre"].ToString();
                    rating.BookDetail.Author = dr["Author"].ToString();
                    rating.BookDetail.Publisher = dr["Publisher"].ToString();
                    rating.BookDetail.PublishedDate = dr["PublishedDate"].ToString();
                    rating.BookDetail.CoverUrl = dr["CoverUrl"].ToString();
                    rating.BookDetail.BookDesc = dr["BookDesc"].ToString();
                    rating.result = new Result();
                    rating.result.result = true;
                    rating.result.message = "Rating created successfully";
                }
            }
            catch (SqlException ex)
            {
                rating.result = new Result();
                rating.result.result = false;
                rating.result.message = "Rating creation failed";
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _conn.Close();
            }
            return rating;
        }


        [HttpDelete, Route("[action]" , Name ="DeleteRating")]
        public Result DeleteRating(DeleteRating deleteRating)
        {
            Result result = new Result();
            try
            {
                _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("DeleteRating", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", deleteRating.BookId);
                    cmd.Parameters.AddWithValue("UserId", deleteRating.UserId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        result.result = true;
                        result.message = "Rating for the book is deleted";
                    }
                    else
                    {
                        result.result = false;
                    }
                }

            }
            catch(SqlException ex)
            {
                result.result = false;
                result.message = ex.ToString();
            }
            return result;
        }
    } 
}
