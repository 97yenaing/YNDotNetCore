using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YNDotNetCore.ConsoleApp.Dtos;

namespace YNDotNetCore.ConsoleApp.EFCoreExamples
{
    internal class EFCoreExample
    {
        private readonly AppDbContext db = new AppDbContext();
        public void Run()
        {
            /*Read();*/
            /*Edit(3);
            Edit(50);*/
            /*Create("EfTitle", "EfAuthor", "EfContent");*/
            /* Update(6,"EfTitleUpdate", "EfAuthorUpdate", "EfContentUpdate");*/
            Delete(6);
        }
        private void Read()
        {

            var lst = db.Blogs.ToList();
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
            var item = db.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                Console.WriteLine("No Data Found");
                return;
            }
            Console.WriteLine(item.BlogId);
            Console.WriteLine(item.BlogTitle);
            Console.WriteLine(item.BlogAuthor);
            Console.WriteLine(item.BlogContent);
            Console.WriteLine("--------------------------------");
        }

        private void Create(string title, string author, string content)
        {
            var item = new BlogDto
            {
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content,
            };
            db.Blogs.Add(item);
            int result = db.SaveChanges();//Query EXecute EfCore

            string message = result > 0 ? "saving Successful." : "Saving Failed";
            Console.WriteLine(message);
        }

        private void Update(int id, string title, string author, string content)
        {
            var item = db.Blogs.FirstOrDefault(x => x.BlogId == id); // already get real db state 
            if (item is null)
            {
                Console.WriteLine("No Data Found");
                return;
            }
            item.BlogTitle = title; // already prepare update value 
            item.BlogAuthor = author;
            item.BlogContent = content;

            int result = db.SaveChanges();//Query EXecute EfCore

            string message = result > 0 ? "Updating  Successful." : "Updating Failed";
            Console.WriteLine(message);

        }
        private void Delete(int id)
        {
            var item = db.Blogs.FirstOrDefault(x => x.BlogId == id); // already get real db state 
            if (item is null)
            {
                Console.WriteLine("No Data Found");
                return;
            }
            db.Blogs.Remove(item);
            int result = db.SaveChanges();//Query EXecute EfCore

            string message = result > 0 ? "Deleting  Successful." : "Deleting Failed";
            Console.WriteLine(message);
        }

        // for database to C# code changes //efcore command database first serach in google




    }
}
