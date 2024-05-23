using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace YNDotNetCore.ConsoleAppHttpClientExample
{
    internal class HttpClientExample
    {   private readonly HttpClient _client = new HttpClient() { BaseAddress = new Uri("https://localhost:7236") };
        private readonly string _blogEndpoint = "api/blog";
        public async Task RunAsync()
        {
            await EditAsync(1);
        }

        public async Task ReadAsync()
        {
            var response = await _client.GetAsync(_blogEndpoint); // when no await you have just one job not do, when await do the job
            //task.RunSynchronously();// do the have job

            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync(); //contant is db table data that have json form in postman body
                List<BlogModel> lst = JsonConvert.DeserializeObject<List<BlogModel>>(jsonStr)!;
                foreach (var blog in lst)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(blog));
                    Console.WriteLine($"Title => {blog.BlogTitle}");
                    Console.WriteLine($"Author => {blog.BlogAuthor}");
                    Console.WriteLine($"Content => {blog.BlogContent}");

                }
            }
        }

        private async Task EditAsync(int blogId)
        {
            var response = await _client.GetAsync($"{_blogEndpoint}/{blogId}"); // when no await you have just one job not do, when await do the job
            //task.RunSynchronously();// do the have job

            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync(); //contant is db table data that have json form in postman body
                var  item = JsonConvert.DeserializeObject<BlogModel>(jsonStr)!;
               
                Console.WriteLine(JsonConvert.SerializeObject(item));
                Console.WriteLine($"Title => {item.BlogTitle}");
                Console.WriteLine($"Author => {item.BlogAuthor}");
                Console.WriteLine($"Content => {item.BlogContent}");


            }
            else
            {
                var message=  await response.Content.ReadAsStringAsync();
                Console.WriteLine(message);
            }
        }

        public async Task DeleteAsync(int blogId)
        {
            var response = await _client.DeleteAsync($"{_blogEndpoint}/{blogId}"); // when no await you have just one job not do, when await do the job
            //task.RunSynchronously();// do the have job

            if (response.IsSuccessStatusCode)
            {
                string message = await response.Content.ReadAsStringAsync(); //contant is db table data that have json form in postman body
                Console.WriteLine(message);
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                Console.WriteLine(message);
            }
        }

        private async Task CreateAsync(string title,string author,string content)
        { 
            BlogModel blogModel = new BlogModel()
            {
                BlogAuthor = author,
                BlogContent = content,
                BlogTitle = title,
            };
            string blogJson=JsonConvert.SerializeObject(blogModel);

            HttpContent httpContent = new StringContent(blogJson,Encoding.UTF8,Application.Json);
            var response = await _client.PostAsync(_blogEndpoint,httpContent);
            if (response.IsSuccessStatusCode)
            {
                string message = await response.Content.ReadAsStringAsync();
                Console.WriteLine(message);
            }else
            {
                Console.WriteLine("No Connnection");
            }
        }

        private async Task UpdateAsync(int id,string title, string author, string content)
        {
            BlogModel blogModel = new BlogModel()
            {
                BlogAuthor = author,
                BlogContent = content,
                BlogTitle = title,
            };
            string blogJson = JsonConvert.SerializeObject(blogModel);

            HttpContent httpContent = new StringContent(blogJson, Encoding.UTF8, Application.Json);
            var response = await _client.PutAsync($"{_blogEndpoint}/{id}", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string message = await response.Content.ReadAsStringAsync();
                Console.WriteLine(message);
            }
        }
    }
}
