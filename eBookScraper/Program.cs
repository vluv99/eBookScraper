using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Web;
using System.IO;


internal class Program
{
    private static void Main(string[] args)
    {
        var htmlDocument = new HtmlDocument();
        var filePath =
            "/home/vluv/Documents/Projects/eBookScraper/eBookScraper/scraps/heaven-officials-blessing-0.html";
        if (!File.Exists(filePath))
        {
            // send GET request to url
            var url = "https://novelbin.me/novel-book/heaven-officials-blessing";
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            htmlDocument.LoadHtml(html);

            // Save to file
            File.WriteAllText(filePath, html);
        }
        else
        {
            // Read from file
            var htmlText = File.ReadAllText(filePath);
            htmlDocument.LoadHtml(htmlText);
        }

        // Get the title
        var titleElement = htmlDocument.DocumentNode.SelectSingleNode("//h3[@class='title']");
        var title = HttpUtility.HtmlDecode(titleElement.InnerText).Trim();
        Console.WriteLine("title: " + title);

        var infoBlockElement = htmlDocument.DocumentNode.SelectSingleNode("//ul[@class='info info-meta']");

        // Get author name
        foreach (var node in infoBlockElement.ChildNodes)
        {
            if (node.NodeType == HtmlNodeType.Element)
            {
                var h3Element = node.SelectSingleNode("h3");
                if (h3Element != null)
                {
                    Console.WriteLine(h3Element.InnerText);
                }
                else if (node.SelectSingleNode("*/h3") != null)
                {
                    Console.WriteLine(node.SelectSingleNode("*/h3")?.InnerText ?? "N/A");
                }

                Console.WriteLine("...............");

                var dataElement = node.ChildNodes[2];
                if (dataElement?.InnerText != null)
                {
                    Console.WriteLine(dataElement.InnerText);
                }

                //var dataElement = node.ChildNodes.First((n) => n.NodeType == HtmlNodeType.Element);
                // if (dataElement != null)
                // {
                //     Console.WriteLine(dataElement.InnerText);
                // }

                // else if (node.SelectSingleNode("*/h3") != null)
                // {
                //     Console.WriteLine(node.SelectSingleNode("*/h3")?.InnerText ?? "N/A");
                // }
                Console.WriteLine("##################################################");
            }
        }


        //String author = HttpUtility.HtmlDecode(titleElement.InnerText).Trim();
        //Console.WriteLine("title: " + author);

        // Get completion status

        // Get image
    }
}