using System.Runtime.InteropServices.JavaScript;
using System.Web;
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
        string html = Fetcher.GetPage(book.url);

        //Use the default configuration for AngleSharp
        IConfiguration config = Configuration.Default;

        //Create a new context for evaluating webpages with the given config
        IBrowsingContext context = BrowsingContext.New(config);

        IHtmlParser parser = context.GetService<IHtmlParser>()!;
        IDocument document = parser.ParseDocument(html);

        // Get the title
        var title = document.QuerySelector(".title")?.TextContent.Trim();
        book.title = title ?? "";
        Console.WriteLine("title: " + book.title);

        // Get Book infos
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

        // Get description
        var desc = document.QuerySelector(".desc-text")?.TextContent.Trim();
        book.description = desc ?? "";
        Console.WriteLine("description: " + book.description);

        // Get rating
        var rating = document.QuerySelector("span[itemprop='ratingValue']")?.TextContent.Trim();
        book.rating = rating != null ? Convert.ToDecimal(rating) : 0;
        Console.WriteLine("rating: " + book.rating);

        // Get chapters list


        //TODO: fetch chapters
    }
}