using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNDotNetCore.ConsoleApp
{
    internal class DrapperExample
    {
        public void Run() {

            /* Read();
             Edit(2);
             Edit(20);*/
            /* Create("title", "author", "content");*/
            /*Update(5, "title 5", "author 5", "content 5");*/
            Delete(10);

        }
        private void Read() {
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            List<BlogDto> lst = db.Query<BlogDto>("select * from tbl_blog").ToList();

            foreach (BlogDto item in lst)
            {
                Console.WriteLine(item.BlogId);
                Console.WriteLine(item.BlogTitle);
                Console.WriteLine(item.BlogAuthor);
                Console.WriteLine(item.BlogContent);
                Console.WriteLine("--------------------------------");
            }
            
        }

        private void Edit(int id)
        {
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            var item = db.Query<BlogDto>("select * from tbl_blog where BlogId = @BlogId",new BlogDto { BlogId =id}).FirstOrDefault();
            if (item is null) {
                Console.WriteLine("No Data Found");
                return;
            }
            Console.WriteLine(item.BlogId);
            Console.WriteLine(item.BlogTitle);
            Console.WriteLine(item.BlogAuthor);
            Console.WriteLine(item.BlogContent);
            Console.WriteLine("--------------------------------");


        }
        private void Create(String title, string author, string content)
        {
            var item = new BlogDto
            {
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content,
            };
            string query = @"INSERT INTO [dbo].[tbl_blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
           VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            int result=db.Execute(query, item);

            string message = result > 0 ? "saving Successful." : "Saving Failed";
            Console.WriteLine(message);

        }

        private void Update(int id,String title, string author, string content)
        {
            var item = new BlogDto
            {   BlogId = id,
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content,
            };
            string query = @"UPDATE [dbo].[tbl_blog]
             SET [BlogTitle] = @BlogTitle
            ,[BlogAuthor] =@BlogAuthor
            ,[BlogContent] = @BlogContent
             WHERE BlogId = @BlogId";
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, item);

            string message = result > 0 ? "Updating  Successful." : "Updating Failed";
            Console.WriteLine(message);

        }
        private void Delete(int id)
        {
            var item = new BlogDto
            {
                BlogId = id,
            };
            string query = @"DELETE FROM [dbo].[tbl_blog]
            WHERE BlogId = @BlogId";
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, item);

            string message = result > 0 ? "Delete  Successful." : "Delete Failed";
            Console.WriteLine(message);
        }

    }
}
