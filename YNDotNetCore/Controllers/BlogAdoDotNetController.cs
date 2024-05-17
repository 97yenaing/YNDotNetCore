using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
/*using Microsoft.Data.SqlClient;*/
using System.Data;
using System.Data.SqlClient;
using YNDotNetCore.RestApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace YNDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNetController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "select * from tbl_blog";
            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            Console.WriteLine("Connection Open");


            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            connection.Close();
            /* List<BlogModel> list = new List<BlogModel>();
             foreach (DataRow dr in dt.Rows)
             {   BlogModel blog = new BlogModel();
                 blog.BlogId =Convert.ToInt32(dr["BlogID"]);
                 blog.BlogAuthor = Convert.ToString(dr["BlogAuthor"]);
                 blog.BlogTitle = Convert.ToString(dr["BlogTitle"]);
                 blog.BlogContent = Convert.ToString(dr["BlogContent"]);
                 list.Add(blog);
             }*/
            List<BlogModel> list = dt.AsEnumerable().Select(dr => new BlogModel
            {
                BlogId = Convert.ToInt32(dr["BlogID"]),
                BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
                BlogTitle = Convert.ToString(dr["BlogTitle"]),
                BlogContent = Convert.ToString(dr["BlogContent"]),
            }).ToList();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            string query = "select * from tbl_blog where BlogId=@BlogId ";
            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            Console.WriteLine("Connection Open");


            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            connection.Close();
            if (dt.Rows.Count == 0)
            {
                return NotFound("No data found.");
            }
            DataRow dr = dt.Rows[0];
            var item = new BlogModel
            {
                BlogId = Convert.ToInt32(dr["BlogID"]),
                BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
                BlogTitle = Convert.ToString(dr["BlogTitle"]),
                BlogContent = Convert.ToString(dr["BlogContent"]),
            };
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBlog(BlogModel blog)
        {
            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = @"INSERT INTO [dbo].[tbl_blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
           VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";
            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);
            int result = cmd.ExecuteNonQuery();

            connection.Close();
            string message = result > 0 ? "saving Successful." : "Saving Failed";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(BlogModel blog, int id)
        {

            string query = @"UPDATE [dbo].[tbl_blog]

             SET [BlogTitle] = @BlogTitle
            ,[BlogAuthor] =@BlogAuthor
            ,[BlogContent] = @BlogContent
             WHERE BlogId = @BlogId";
            string findquery = "select * from tbl_blog where BlogId=@BlogId_find ";
            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand findcmd = new SqlCommand(findquery, connection);
            findcmd.Parameters.AddWithValue("@BlogId_find", id);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(findcmd);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            connection.Close();
            if (dt.Rows.Count == 0)
            {
                return NotFound("No data found.");
            }


            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);
            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? "Update Successful." : "Update Failed";
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public ActionResult PatchBlog(int id,BlogModel blog) {
            
            string findquery = "select * from tbl_blog where BlogId=@BlogId_find ";
            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand findcmd = new SqlCommand(findquery, connection);
            findcmd.Parameters.AddWithValue("@BlogId_find", id);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(findcmd);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            connection.Close();
            if (dt.Rows.Count == 0)
            {
                return NotFound("No data found.");
            }
            string title = string.Empty;
            string author = string.Empty;
            string content = string.Empty;
            string conditions = string.Empty;
            if (!string.IsNullOrEmpty(blog.BlogTitle))
            {
                conditions += "[BlogTitle] = @BlogTitle, ";
                title = "title";

            }
            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            {
                conditions += "[BlogAuthor] =@BlogAuthor, ";
                author = "author";
            }
            if (!string.IsNullOrEmpty(blog.BlogContent))
            {
                conditions += "[BlogContent] = @BlogContent, ";
                content = "content";
            }
            if (conditions.Length == 0)
            {
                return NotFound("No data to update.");
            }
            conditions = conditions.Substring(0, conditions.Length - 2);

            connection.Open();
            string query = $@"UPDATE [dbo].[tbl_blog]
             SET{conditions} WHERE BlogId = @BlogId";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            if (title=="title")
            {
                cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            }
             if (author == "author")
            {
                cmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            }
            if(content == "content") { 
                cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);
            }
            
            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? "Update Successful." : "Update Failed";
            return Ok(message);
        }

        [HttpDelete]
        public IActionResult DeleteBlog(int id)
        {
            string query = @"DELETE FROM [dbo].[tbl_blog]
            WHERE BlogId = @BlogId";
            string findquery = "select * from tbl_blog where BlogId=@BlogId_find ";
            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand findcmd = new SqlCommand(findquery, connection);
            findcmd.Parameters.AddWithValue("@BlogId_find", id);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(findcmd);
            DataTable dt = new DataTable();

            sqlDataAdapter.Fill(dt);

            connection.Close();
            if (dt.Rows.Count == 0)
            {
                return NotFound("No data found.");
            }


            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? "Deleting Successful." : "Deleting Failed";
            return Ok(message);
        }
    }
}
