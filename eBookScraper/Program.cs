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
        Book book = new Book();
        book.url = "https://novelbin.me/novel-book/heaven-officials-blessing";
        string html = "";

        var filePath =
            "/home/vluv/Documents/Projects/eBookScraper/eBookScraper/scraps/heaven-officials-blessing-0.html";
        if (!File.Exists(filePath))
        {
            // send GET request to url
            var url = book.url;
            var httpClient = new HttpClient();
            html = httpClient.GetStringAsync(url).Result;

            // Save to file
            File.WriteAllText(filePath, html);
        }
        else
        {
            // Read from file
            html = File.ReadAllText(filePath);
        }

        //Use the default configuration for AngleSharp
        IConfiguration config = Configuration.Default;

        //Create a new context for evaluating webpages with the given config
        IBrowsingContext context = BrowsingContext.New(config);

        IHtmlParser parser = context.GetService<IHtmlParser>()!;
        IDocument document = parser.ParseDocument(html);

        // Get the title
        var title = document.QuerySelector(".title")?.TextContent.Trim();
        book.title = title ?? "";
        Console.WriteLine("title: " + title);

        var infoBlock = document.QuerySelector(".info.info-meta");
        if (infoBlock != null)
        {
            foreach (var element in infoBlock.Children)
            {
                var label = element.QuerySelector("h3")?.TextContent.Trim();
                if (label != null && label.Contains("Author"))
                {
                    //TODO: check if element.children has more than 1 content, if it doesn't we need to use ChildNodes otherwise Children can be used
                    book.author = element.Children[1].TextContent.Trim();
                    Console.WriteLine("author: " + book.author);
                }
                else if (label != null && label.Contains("Alternative names"))
                {
                    var content = element.ChildNodes[2].TextContent.Trim();
                    book.altNames = content.Split(",").Select((name) => name.Trim()).ToList();
                    Console.WriteLine("alt names: " + string.Join(", ", book.altNames));
                }
                else if (label != null && label.Contains("Status"))
                {
                    book.status = element.Children[1].TextContent.Trim();
                    Console.WriteLine("status: " + book.status);
                }
                else if (label != null && label.Contains("Genre"))
                {
                    book.genres = element.QuerySelectorAll("a").Select(a => a.TextContent.Trim()).ToList();
                    Console.WriteLine("genre: " + string.Join(", ", book.genres));
                }
            }
        }
    }
}