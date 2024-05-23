using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace YNDotNetCoreConsoleAppRestClientExample
{
    internal class RestClientExample
    {
        private readonly RestClient _client = new RestClient(new Uri("https://localhost:7236"));
        private readonly string _blogEndpoint = "api/blog";
        public async Task RunAsync()
        {
            await ReadAsync();
        }

        public async Task ReadAsync()
        {
            RestRequest restRequest = new RestRequest(_blogEndpoint, Method.Get);
            var response = await _client.ExecuteAsync(restRequest);
            //var response = await _client.GetAsync(restRequeest); // when no await you have just one job not do, when await do the job
            //task.RunSynchronously();// do the have job

            if (response.IsSuccessStatusCode)
            {
                string jsonStr = response.Content!; //contant is db table data that have json form in postman body
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
            RestRequest restRequest = new RestRequest($"{_blogEndpoint}/{blogId}", Method.Get);
            var response = await _client.ExecuteAsync(restRequest);// when no await you have just one job not do, when await do the job
            //task.RunSynchronously();// do the have job

            if (response.IsSuccessStatusCode)
            {
                string jsonStr = response.Content!;//contant is db table data that have json form in postman body
                var item = JsonConvert.DeserializeObject<BlogModel>(jsonStr)!;

                Console.WriteLine(JsonConvert.SerializeObject(item));
                Console.WriteLine($"Title => {item.BlogTitle}");
                Console.WriteLine($"Author => {item.BlogAuthor}");
                Console.WriteLine($"Content => {item.BlogContent}");


            }
            else
            {
                var message = response.Content!;
                Console.WriteLine(message);
            }
        }

        public async Task DeleteAsync(int blogId)
        {
            RestRequest restRequest = new RestRequest($"{_blogEndpoint}/{blogId}", Method.Delete);
            var response = await _client.ExecuteAsync(restRequest);

            // when no await you have just one job not do, when await do the job
            //task.RunSynchronously();// do the have job

            if (response.IsSuccessStatusCode)
            {
                string message = response.Content!; //contant is db table data that have json form in postman body
                Console.WriteLine(message);
            }
            else
            {
                var message = response.Content!;
                Console.WriteLine(message);
            }
        }

        private async Task CreateAsync(string title, string author, string content)
        {
            BlogModel blogModel = new BlogModel()
            {
                BlogAuthor = author,
                BlogContent = content,
                BlogTitle = title,
            };
            var restRequest = new RestRequest(_blogEndpoint, Method.Post);
            restRequest.AddJsonBody(blogModel);
            var response = await _client.ExecuteAsync(restRequest);
            if (response.IsSuccessStatusCode)
            {
                string message = response.Content!;
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("No Connnection");
            }
        }

        private async Task UpdateAsync(int id, string title, string author, string content)
        {
            BlogModel blogModel = new BlogModel()
            {
                BlogAuthor = author,
                BlogContent = content,
                BlogTitle = title,
            };
            var restRequest = new RestRequest($"{_blogEndpoint}/{id}", Method.Put);
            restRequest.AddJsonBody(blogModel);
            var response = await _client.ExecuteAsync(restRequest);
            if (response.IsSuccessStatusCode)
            {
                string message = response.Content!;
                Console.WriteLine(message);
            }
        }
    }
}
