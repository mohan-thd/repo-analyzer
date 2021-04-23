using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml;
using System.Text;

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
            client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");

            var stringTask =  client.GetStringAsync("https://api.github.com/repos/quotecenter/catalog-productcatalogsuggest-icp-svc/contents/src/Catalog.ProductCatalogSuggest.Icp.Service/Catalog.ProductCatalogSuggest.Icp.Service.csproj?ref=master");

            var msg =  await stringTask;
            dynamic convertMsg = Newtonsoft.Json.JsonConvert.DeserializeObject(msg);
            string prjFileContent = convertMsg["content"];
            byte[] newBytes = Convert.FromBase64String(prjFileContent);
            var base64EncodedBytes = System.Convert.FromBase64String(prjFileContent);
            string finalXMLContent= System.Text.Encoding.UTF8.GetString(base64EncodedBytes).TrimStart('?');
            string byteOrderMark = System.Text.Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            finalXMLContent= finalXMLContent.Replace('\r',' ');
            finalXMLContent = finalXMLContent.Replace('\n', ' ');
            //finalXMLContent = finalXMLContent.Replace(@"Sdk=\","Sdk=");
            //finalXMLContent = "<?xml version='1.0' encoding='utf-16'?>" + finalXMLContent.Trim();
            //finalXMLContent = new System.Text.RegularExpressions.Regex("\\<\\?xml.*\\?>").Replace(finalXMLContent, "").Trim();
            finalXMLContent = finalXMLContent.Trim(byteOrderMark[0]);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(finalXMLContent);
            string xPath = "Project/PropertyGroup/TargetFramework";
            var frameworkNode = xmlDoc.SelectSingleNode(xPath);
            xPath = "Project/ItemGroup/PackageReference";
            var nodes = xmlDoc.SelectNodes(xPath);
            foreach (XmlNode childNode in nodes)
            {
                Console.WriteLine(childNode.Attributes["Include"].InnerText);
            }
            

            Console.Write(frameworkNode.InnerText);
        }
    }
}
