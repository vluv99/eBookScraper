using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Web;

using System.IO;

namespace eBookScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var htmlDocument = new HtmlDocument();
            var filePath = "heaven-officials-blessing-0.html";
            if (!File.Exists(filePath))
            {
                // send GET request to url
                String url = "https://novelbin.me/novel-book/heaven-officials-blessing";
                var httpClient = new HttpClient();
                var html = httpClient.GetStringAsync(url).Result;
                htmlDocument.LoadHtml(html);
            
                // Save to file
                File.WriteAllText(filePath, html);  
            }
            else
            {
                // Read from file
                string htmlText = File.ReadAllText(filePath);
                htmlDocument.LoadHtml(htmlText);
            }
            
            // Get the title
            var titleElement = htmlDocument.DocumentNode.SelectSingleNode("//h3[@class='title']");
            String title = HttpUtility.HtmlDecode(titleElement.InnerText).Trim();
            Console.WriteLine("title: " + title);
            
            var infoBlockElement = htmlDocument.DocumentNode.SelectSingleNode("//ul[@class='info info-meta']");
            
            // Get author name
            var authorElement = infoBlockElement.ChildNodes;
            Console.WriteLine(authorElement);
            
            //String author = HttpUtility.HtmlDecode(titleElement.InnerText).Trim();
            //Console.WriteLine("title: " + author);
            
            // Get completion status
            
            // Get image
        }
    }
}

