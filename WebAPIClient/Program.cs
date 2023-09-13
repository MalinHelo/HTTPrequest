// använder denna gudiden: https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;


namespace httprequest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var repositories = await ProcessRepositoriesAsync(client);

            foreach (var repo in repositories)
            {
                Console.WriteLine($"Name: {repo.Name}");
                Console.WriteLine($"Homepage: {repo.Homepage}");
                Console.WriteLine($"GitHub: {repo.GitHubHomeUrl}");
                Console.WriteLine($"Description: {repo.Description}");
                Console.WriteLine($"Watchers: {repo.Watchers:#,0}");
                Console.WriteLine($"{repo.LastPush}");
                Console.WriteLine();
            }
        }
        static async Task<List<Repository>> ProcessRepositoriesAsync(HttpClient client)
        {
            await using Stream stream = await client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(stream);
            return repositories ?? new();

        }

    }


}

