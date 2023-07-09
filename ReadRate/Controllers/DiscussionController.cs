using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadRate.Models;
using System.Data;
using System.Data.SqlClient;

namespace ReadRate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private SqlConnection _conn;
        private readonly SupplementaryController supplementaryController;
        private readonly IConfiguration configuration;
        IHttpContextAccessor Context;
        HttpClient client = new HttpClient();
        public int? UserId;
        public DiscussionController(IConfiguration _configuration, IHttpContextAccessor _context, SupplementaryController _controller)
        {
            supplementaryController = _controller;
            configuration = _configuration;
            Context = _context;
            UserId = Context.HttpContext.Session.GetInt32("UserId");
            _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]);
            _conn.Open();
        }
        [HttpGet, Route("[action]", Name = "ListDiscussion")]
        public DiscussionList GetDiscussionList(int communityId)
        {
            DiscussionList discussionList = new DiscussionList();
            try
            {
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("ListDiscussion", _conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@communityId", communityId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        discussionList.Discussions = new List<CommunityDiscussion>(); // Initialize the Discussions list
                        discussionList.UserName = new List<string>(); // Initialize the UserName list
                        foreach (DataRow dr in dt.Rows)
                        {
                            CommunityDiscussion communityDiscussion = new CommunityDiscussion();
                            communityDiscussion.DiscussionId = Convert.ToInt32(dr["DiscussionId"]);
                            communityDiscussion.CommunityMemberId = Convert.ToInt32(dr["CommunityMemberId"]);
                            communityDiscussion.CommunityId = Convert.ToInt32(dr["CommunityId"]);
                            communityDiscussion.Discussion = dr["Discussion"].ToString();
                            discussionList.Discussions.Add(communityDiscussion);
                            discussionList.UserName.Add(supplementaryController.GetUserNameByMemberId(communityDiscussion.CommunityMemberId));
                        }
                    }
                    discussionList.result = new Models.Result(); // Create a new instance of Result object
                    discussionList.result.result = true;
                    discussionList.result.message = "List of discussions";
                }
            }
            catch (Exception ex)
            {
                discussionList.result = new Models.Result(); // Create a new instance of Result object
                discussionList.result.result = false;
                discussionList.result.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return discussionList;
        }

        [HttpGet, Route("[action]", Name ="ListDiscussionReply")]
        public ListDiscussionReply GetDiscussionReply(int discussionId)
        {
            ListDiscussionReply listDiscussionReply = new ListDiscussionReply();
            try
            {
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("ListDiscussionReply", _conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiscussionId", discussionId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Console.WriteLine("Found");
                        listDiscussionReply.communityDiscussion = supplementaryController.GetDiscussionByDiscussionId(discussionId);
                        foreach (DataRow dr in dt.Rows)
                        {
                            DiscussionReply reply = new DiscussionReply();
                            reply.DiscussionReplyId = Convert.ToInt32(dr["DiscussionReplyId"]);
                            reply.DiscussionId = Convert.ToInt32(dr["DiscussionId"]);
                            reply.CommunityMemberId = Convert.ToInt32(dr["CommunityMemberId"]);
                            reply.Reply = dr["Reply"].ToString();
                            reply.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            listDiscussionReply.UserName.Add(supplementaryController.GetUserNameByMemberId(reply.CommunityMemberId));
                            listDiscussionReply.discussionReply.Add(reply);
                            listDiscussionReply.result.result = true;
                            listDiscussionReply.result.message = "Listed the replies";
                        }
                    }
                    else
                    {
                        listDiscussionReply.result.result = true;
                        listDiscussionReply.result.message = "No replies to the Discussion";
                    }
                }
            }
            catch (Exception ex)
            {
                listDiscussionReply.result.result = false;
                listDiscussionReply.result.message= ex.Message;
                Console.WriteLine (ex.Message);
            }
            return listDiscussionReply;
        }
        [HttpDelete, Route("[action]", Name ="DeleteDiscussionReply")]
        public Result DeleteDiscussionReply(int DiscussionReplyId)
        {
            Result result = new Result();
            try
            {
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("DeleteDiscussionReply", _conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiscussionReplyId", DiscussionReplyId);
                    cmd.ExecuteNonQuery();
                    result.result = true;
                    result.message = "Deleted the replies successfully";
                }
            }
            catch (SqlException ex)
            {
                result.result = false;
                result.message = ex.Message;
                Console.WriteLine (ex.Message);
            }

            return result;
        }

        [HttpPost, Route("[action]", Name = "CreateDiscussion")]
        public Result CreateDiscussion(CommunityDiscussion communityDiscussion)
        {
            Result res = new Result();
            try
            {
                using (SqlConnection _conn = new SqlConnection(configuration["ConnectionStrings:SqlConn"]))
                {
                    _conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CreateDiscussion", _conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("CommunityId", communityDiscussion.CommunityId);
                        int memId = supplementaryController.GetMemberId(communityDiscussion.CommunityId);
                        Console.WriteLine(memId);

                        cmd.Parameters.AddWithValue("CommunityMemberId", memId);
                        cmd.Parameters.AddWithValue("Discussion", communityDiscussion.Discussion);
                        cmd.ExecuteNonQuery();
                        res.result = true;
                        res.message = "Successful in creation";
                    }
                }
            }
            catch (SqlException ex)
            {
                res.result = false;
                res.message = ex.Message;
            }
            return res;
        }

        [HttpPost, Route("[action]", Name = "CreateDiscussionReply")]
        public ListDiscussionReply CreateDiscussionReply(DiscussionReply discussionReply)
        {
            ListDiscussionReply listDiscussionReply = new ListDiscussionReply();
            try
            {
                _conn.Open();
                using (_conn)
                {
                    SqlCommand cmd = new SqlCommand("CreateDiscussionReply", _conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiscussionId", discussionReply.DiscussionId);
                    int? userId = Context.HttpContext.Session.GetInt32("UserId");
                    int convertedUserId = userId.HasValue ? userId.Value : 0;
                    cmd.Parameters.AddWithValue("@MemberId", supplementaryController.GetMemberId(convertedUserId));
                    cmd.Parameters.AddWithValue("@Reply", discussionReply.Reply);
                    cmd.ExecuteNonQuery();
                    listDiscussionReply = supplementaryController.GetDiscussionReplyByDiscussionId(discussionReply.DiscussionId);

                }
            }
            catch(Exception ex)
            {
                listDiscussionReply.result.result = false;
                listDiscussionReply.result.message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return listDiscussionReply;
        }

    }
}
