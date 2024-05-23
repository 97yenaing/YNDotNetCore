// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using YNDotNetCore.ConsoleAppHttpClientExample;

HttpClientExample httpClientExample = new HttpClientExample();
await httpClientExample.RunAsync();

Console.ReadLine();




// See https://aka.ms/new-console-template for more information
//ConsoleApp -Client
//ASP.NET Core Web API -Server

/*HttpClient client = new HttpClient();

var response = await client.GetAsync("https://localhost:7236/api/blog"); // when no await you have just one job not do, when await do the job
//task.RunSynchronously();// do the have job

if (response.IsSuccessStatusCode)
{
    string jsonStr = await response.Content.ReadAsStringAsync(); //contant is db table data that have json form in postman body
    Console.WriteLine(jsonStr);
    List<BlogModel> lst= JsonConvert.DeserializeObject<List<BlogModel>>(jsonStr)!;
    foreach (var blog in lst)
    {
        Console.WriteLine(JsonConvert.SerializeObject(blog));
        Console.WriteLine($"Title => {blog.BlogTitle}");
        Console.WriteLine($"Author => {blog.BlogAuthor}");
        Console.WriteLine($"Content => {blog.Con}");

    }
}*/