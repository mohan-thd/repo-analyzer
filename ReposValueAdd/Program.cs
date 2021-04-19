using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace repoanalyzer
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            await ProcessRepositories();

        }




        private static async Task ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer bde73a6c6c059424d9de0934f5383c0e3721108e");

            var stringTask =  client.GetStringAsync("https://api.github.com/repos/quotecenter/catalog-productcatalogsuggest-icp-svc");

            var msg =  await stringTask;
            Console.Write(msg);
        }
    }
}
