using System.Reflection.Metadata;
using YNDotNetCore.RestApiWithNLayer.Db;

namespace YNDotNetCore.RestApiWithNLayer.Features.Blog
{
    //Data Access
    public class DA_Blog
    {
        private readonly AppDbContext _context;

        public DA_Blog()
        {
            _context = new AppDbContext();
        }
        public List<BlogModel> GetBlogs()
        {
            var lst= _context.Blogs.ToList();
            return lst;
        }

        public BlogModel GetBlog(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x=>x.BlogId == id);
            return item;
        }

        public int CreateBlog(BlogModel requestmodel)
        {
            _context.Blogs.Add(requestmodel);
            var result=_context.SaveChanges();
            return result;
        }

        public int UpdateBlog(int id,BlogModel requestmodel)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return 0;
            }
            item.BlogTitle = requestmodel.BlogTitle;
            item.BlogAuthor = requestmodel.BlogAuthor;
            item.BlogContent = requestmodel.BlogContent;

            var result= _context.SaveChanges();
            return result;
        }

        public int PatchBlog(int id,BlogModel requestmodel)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return 0;
            }
            if (!string.IsNullOrEmpty(requestmodel.BlogTitle))
                item.BlogTitle = requestmodel.BlogTitle;


            if (!string.IsNullOrEmpty(requestmodel.BlogAuthor))
                item.BlogAuthor = requestmodel.BlogAuthor;


            if (!string.IsNullOrEmpty(requestmodel.BlogContent))
                item.BlogContent = requestmodel.BlogContent;

            var result= _context.SaveChanges();
            return result;
        }

        public int DeleteBlog(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.BlogId == id);
            if (item is null)
            {
                return 0;
            }
            _context.Blogs.Remove(item);
            var result= _context.SaveChanges();
            return result;
        }


    }
}
