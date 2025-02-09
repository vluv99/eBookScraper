using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace eBookScraper.websites;

public class Novelbin : IWebsite
{
    public string WebsiteName { get; } = "novelbin";
    private IHtmlParser Parser { get; }
    private string Prefix { get; }

    public Novelbin(string prefix)
    {
        //Use the default configuration for AngleSharp
        IConfiguration config = Configuration.Default;

        //Create a new context for evaluating webpages with the given config
        IBrowsingContext context = BrowsingContext.New(config);

        Parser = context.GetService<IHtmlParser>()!;
        Prefix = prefix;
    }

    public async Task<Book> GetBook(string url)
    {
        Book book = new Book();
        book.url = url;
        string html = await Fetcher.GetPage(url, Prefix);
        IDocument document = Parser.ParseDocument(html);

        // Get the title
        book.title = GetTitle(document);
        // Get description
        book.description = GetDescription(document);
        // Get book infos
        book.author = GetAuthor(document);
        book.altNames = GetAlternativeNames(document);
        book.status = GetStatus(document);
        book.genres = GetGenres(document);
        // Get rating
        book.rating = GetRating(document);

        // Get chapters list
        book.chapters = await GetChapters(document, book.url);

        return book;
    }

    private string GetTitle(IDocument document)
    {
        return document.QuerySelector(".title")?.TextContent.Trim() ?? "";
    }

    private string GetDescription(IDocument document)
    {
        return document.QuerySelector(".desc-text")?.TextContent.Trim() ?? "";
    }

    private string GetAuthor(IDocument document)
    {
        var element = GetInfoBlockData(document, "Author");
        return element?.Children[1].TextContent.Trim() ?? "";
    }

    private List<string> GetAlternativeNames(IDocument document)
    {
        var element = GetInfoBlockData(document, "Alternative names");
        var content = element?.ChildNodes[2].TextContent.Trim();
        return content?.Split(",").Select((name) => name.Trim()).ToList() ?? [];
    }

    private string GetStatus(IDocument document)
    {
        var element = GetInfoBlockData(document, "Status");
        return element?.Children[1].TextContent.Trim() ?? "";
    }

    private List<string> GetGenres(IDocument document)
    {
        var element = GetInfoBlockData(document, "Genre");
        return element?.QuerySelectorAll("a").Select(a => a.TextContent.Trim()).ToList() ?? [];
    }

    private decimal GetRating(IDocument document)
    {
        var rating = document.QuerySelector("span[itemprop='ratingValue']")?.TextContent.Trim();
        return rating != null ? Convert.ToDecimal(rating) : 0;
    }

    private async Task<List<Chapter>> GetChapters(IDocument document, string bookUrl)
    {
        var chaptersUrl = document.QuerySelector("#tab-chapters-title")?.Attributes["id"]?.Value;
        List<Chapter> chapters = new List<Chapter>();
        if (chaptersUrl != null)
        {
            // for some reason the href isn't correct, but the id is, so using that
            string chaptersHtml = await Fetcher.GetPage(bookUrl + "#" + chaptersUrl, Prefix, true);
            var chaptersDocument = Parser.ParseDocument(chaptersHtml);

            var chaptersListElement = chaptersDocument.QuerySelectorAll(".list-chapter");
            foreach (var element in chaptersListElement)
            {
                var res = element.QuerySelectorAll("a")?.Select((a) =>
                {
                    Chapter chapter = new Chapter();
                    chapter.title = a.TextContent.Trim();
                    chapter.url = a.Attributes["href"]?.Value ?? "";
                    return chapter;
                }).ToList();
                chapters.AddRange(res ?? new List<Chapter>());
            }
        }
        //TODO: fetch the actual chapter content

        return chapters;
    }

    private IElement? GetInfoBlockData(IDocument document, string searchLabel)
    {
        var infoBlock = document.QuerySelector(".info.info-meta");
        if (infoBlock != null)
        {
            foreach (var element in infoBlock.Children)
            {
                var label = element.QuerySelector("h3")?.TextContent.Trim();
                if (label != null && label.Contains(searchLabel))
                {
                    return element;
                }
            }
        }

        return null;
    }
}