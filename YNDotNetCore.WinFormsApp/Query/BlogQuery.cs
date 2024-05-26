using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNDotNetCore.WinFormsApp.Query
{
    internal class BlogQuery
    {
        public static string BlogCreate { get; } = @"INSERT INTO [dbo].[tbl_blog]
           ([BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
           VALUES
           (@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";
    }
}
