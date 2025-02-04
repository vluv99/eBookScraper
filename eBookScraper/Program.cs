using System.Runtime.InteropServices.JavaScript;
using Fizzler;
using HtmlAgilityPack;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using eBookScraper;

internal class Program
{
    private static void Main(string[] args)
    {
        //Use the default configuration for AngleSharp
        IConfiguration config = Configuration.Default;

        //Create a new context for evaluating webpages with the given config
        IBrowsingContext context = BrowsingContext.New(config);

        var htmlDocument = new HtmlDocument();
        var stringSource = "";
        Novel novel = new Novel();
        novel.url = "https://novelbin.me/novel-book/heaven-officials-blessing";

        var filePath =
            "/home/vluv/Documents/Projects/eBookScraper/eBookScraper/scraps/heaven-officials-blessing-0.html";
        if (!File.Exists(filePath))
        {
            // send GET request to url
            var url = novel.url;
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url).Result;
            htmlDocument.LoadHtml(html);

            // Save to file
            File.WriteAllText(filePath, html);
            stringSource = htmlDocument.DocumentNode.OuterHtml;
        }
        else
        {
            // Read from file
            var htmlText = File.ReadAllText(filePath);
            htmlDocument.LoadHtml(htmlText);
            stringSource = htmlDocument.DocumentNode.OuterHtml;
        }


        IHtmlParser parser = context.GetService<IHtmlParser>()!;
        //var source = "<h1>Some example source</h1><p>This is a paragraph element";
        //IDocument document = parser!.ParseDocument(source);
        IDocument document = parser.ParseDocument(stringSource);


        // Get the title
        var title = document.QuerySelector(".title")?.TextContent.Trim();
        novel.title = title ?? "";
        Console.WriteLine("title: " + title);

        var infoBlock = document.QuerySelector(".info.info-meta");
        if (infoBlock != null)
        {
            foreach (var element in infoBlock.Children)
            {
                var label = element.QuerySelector("h3")?.TextContent.Trim();
                var content = "";
                if (label != null && label.Contains("Author"))
                {
                    //TODO: check if element.children has more than 1 content, if it doesn't we need to use ChildNodes otherwise Children can be used
                    content = element.Children[1].TextContent.Trim();
                    novel.author = content;
                    Console.WriteLine("author: " + novel.author);
                }
                else if (label != null && label.Contains("Alternative names"))
                {
                    content = element.ChildNodes[2].TextContent.Trim();
                    novel.altNames = content.Split(",");
                    Console.WriteLine("alt names: " + string.Join(",", novel.altNames));
                }
                else if (label != null && label.Contains("Status"))
                {
                    content = element.Children[1].TextContent.Trim();
                    novel.status = content;
                    Console.WriteLine("status: " + novel.status);
                }
                else if (label != null && label.Contains("Genre"))
                {
                    //TODO
                }
            }
        }
    }
}