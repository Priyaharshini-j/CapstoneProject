using Microsoft.AspNetCore.Mvc;
using ReadRate.Models;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Xml.Linq;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReadRate.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        SqlConnection conn;
        private readonly IConfiguration _configuration;
        IHttpContextAccessor Context;
        SupplementaryController supplementaryController;
        public int? UserId;
        public UserController(IConfiguration configuration, IHttpContextAccessor context , SupplementaryController _supplementaryController)
        {
            _configuration = configuration;
            supplementaryController = _supplementaryController;
            Context = context;
        }

        [HttpPost, Route("[action]", Name = "Login")]
        public UserModel Login(UserLogin user)
        {
            UserModel userModel = new UserModel();
            userModel.result = new Models.Result();
            try
            {
                if (user != null && !string.IsNullOrWhiteSpace(user.UserEmail) && !string.IsNullOrWhiteSpace(user.Password))
                {
                    conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                    conn.Open();
                    using (conn)
                    {
                        SqlCommand cmd = new SqlCommand("ValidateLogin", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserEmail", user.UserEmail);
                        cmd.Parameters.AddWithValue("@UserPassword", user.Password);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            userModel.UserName = dt.Rows[0]["UserName"].ToString();
                            userModel.UserId = (int)dt.Rows[0]["UserId"];
                            userModel.UserEmail = dt.Rows[0]["UserEmail"].ToString();
                            userModel.Password = dt.Rows[0]["Password"].ToString();
                            userModel.SecurityQn = dt.Rows[0]["SecurityQn"].ToString();
                            userModel.SecurityAns = dt.Rows[0]["SecurityAns"].ToString();
                            Context.HttpContext.Session.SetString("UserName", userModel.UserName);
                            Context.HttpContext.Session.SetInt32("UserId", userModel.UserId);
                            Context.HttpContext.Session.SetString("UserEmail", userModel.UserEmail);
                            userModel.result.result = true;
                            userModel.result.message = "success";
                            Console.WriteLine("Success");
                        }
                        else
                        {
                            userModel.result.result = false;
                            userModel.result.message = "Invalid userEmail or Passsword";
                        }
                    }
                }
                else
                {
                    userModel.result.result = false;
                    userModel.result.message = "Please enter username and password";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                userModel.result.result = false;
                userModel.result.message = "Please enter username and password";
                ex.Message.ToString();
            }
            return userModel;
        }
        [HttpPost, Route("[action]", Name = "SignUp")]
        public Models.Result SignUp(signUpModel userModel)
        {
            Models.Result result = new Models.Result();
            try
            {
                conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                using (conn)
                {
                    SqlCommand cmd = new SqlCommand("CreateUser", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", userModel.UserName);
                    cmd.Parameters.AddWithValue("@UserEmail", userModel.UserEmail);
                    cmd.Parameters.AddWithValue("@Password", userModel.Password);
                    cmd.Parameters.AddWithValue("@SecurityQn", userModel.SecurityQn);
                    cmd.Parameters.AddWithValue("@SecurityAns", userModel.SecurityAns);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        result.result = true;
                        result.message = "User Account Has been created successfully";

                    }
                    catch (SqlException ex)
                    {
                        result.result = false;
                        result.message = ex.Message;
                        Console.WriteLine(ex.Message);
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = "Error creating the Profile. Try Again Later...";
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        [HttpPut, Route("[action]", Name = "EditProfile")]
        public Result UpdateProfile(UpdateModel userModel)
        {
            Result result = new Result();
            try
            {
                int? convertedUserID = Context.HttpContext.Session.GetInt32("UserId");
                int UserId = convertedUserID.HasValue ? convertedUserID.Value : 0;

                conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                conn.Open();
                if (userModel.UserName != "string" && userModel.UserEmail != "string" && userModel.Password != "string" && userModel.SecurityAns != "string" && userModel.SecurityQn != "string")
                {                    
                    using (conn)
                    {
                        SqlCommand cmd = new SqlCommand("UpdateUser", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", convertedUserID);
                        cmd.Parameters.AddWithValue("@Password", userModel.Password);
                        cmd.Parameters.AddWithValue("@SecurityQn", userModel.SecurityQn);
                        cmd.Parameters.AddWithValue("@SecurityAns", userModel.SecurityAns);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            result.result = true;
                            result.message = "User Account Has been Updated successfully";

                        }
                        else
                        {
                            result.result = false;
                            result.message = "Error in Updating the profile... Try Again";
                        }
                    }                    
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = "Error Updating the Profile. Try Again Later...";
                ex.Message.ToString();
            }
            return result;
        }

        [HttpDelete, Route("[action]", Name = "DeleteProfile")]
        public Models.Result DeleteProfile()
        {
            Models.Result result = new Models.Result();
            int? convertedUserID = Context.HttpContext.Session.GetInt32("UserId");
            int UserId = convertedUserID.HasValue ? convertedUserID.Value : 0;
            try
            {
                conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                conn.Open();
                using (conn)
                {
                    SqlCommand cmd = new SqlCommand("DeleteUser", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        result.result = true;
                        result.message = "User Account Has been Deleted successfully";

                    }
                    else
                    {
                        result.result = false;
                        result.message = "Error in Updating the profile... Try Again";
                    }

                }
                conn.Close();
            }
            catch (SqlException ex)
            {
                result.result = false;
                result.message = ex.Message;
                ex.Message.ToString();
            }
            return result;
        }

        [HttpGet , Route("[action]", Name = "FetchUserName")]
        public string FetchUserName()
        {
            string UserName = "";
            Console.WriteLine("inside the func");
            signUpModel signUpModel = new signUpModel();
            try
            {
                Console.WriteLine("inisde he try");
                conn = new SqlConnection(_configuration["ConnectionStrings:SqlConn"]);
                conn.Open();
                int? convertedUserID = Context.HttpContext.Session.GetInt32("UserId");
                int UserId = convertedUserID.HasValue ? convertedUserID.Value : 0;
                using (conn)
                {
                    Console.WriteLine("inside hte using");
                    Console.WriteLine(UserId);
                    SqlCommand cmd = new SqlCommand("getUserName", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while(dr.Read())
                    {
                        Console.WriteLine("inside hte reading");
                        UserName = dr["UserName"].ToString();
                        Console.WriteLine(signUpModel.UserName);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return UserName;
        }
    }
}