using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
/*using Microsoft.Data.SqlClient;*/
using System.Data;
using System.Data.SqlClient;
using YNDotNetCore.RestApi.Models;
using YNDotNetCore.Shared;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace YNDotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNet2Controller : ControllerBase
    {   private readonly AdoDotNetService _adoDotNetService=new AdoDotNetService(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
        [HttpGet]
        public IActionResult GetBlogs()
        {
           string query = "select * from tbl_blog";
           var list = _adoDotNetService.Query<BlogModel>(query);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
           /* AdoDotNetParameter[] parameters= new AdoDotNetParameter[1];
            parameters[0]=new AdoDotNetParameter("@BlogId", id); without param in AdoDotNetservice calss */
            string query = "select * from tbl_blog where BlogId=@BlogId ";
            var item = _adoDotNetService.QueryFirstOrDefault<BlogModel>(query, new AdoDotNetParameter("@BlogId", id));
            if (item is null)
            {
                return NotFound("No data found.");
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBlog(BlogModel blog)
        {
            
            string query = @"INSERT INTO [dbo].[tbl_blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
           VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";
            int result = _adoDotNetService.Execute(query, new AdoDotNetParameter("@BlogTitle", blog.BlogTitle),
                new AdoDotNetParameter("@BlogAuthor", blog.BlogAuthor),
                new AdoDotNetParameter("@BlogContent", blog.BlogContent));
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
            string conditions = string.Empty;
            string target = string.Empty;
            if (!string.IsNullOrEmpty(blog.BlogTitle))
            {
                conditions += "[BlogTitle] = @BlogTitle, ";
                target = "title";

            }
            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            {
                conditions += "[BlogAuthor] =@BlogAuthor, ";
                target = "author";
            }
            if (!string.IsNullOrEmpty(blog.BlogContent))
            {
                conditions += "[BlogContent] = @BlogContent, ";
                target = "content";
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
            if (target=="title")
            {
                cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            }
            else if (target == "author")
            {
                cmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            }
            else
            {
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
