using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace eBookScraper.websites;

public class Novelbin : IWebsite
{
    public string BaseUrl { get; } = "https://novelbin.me";
    public IHtmlParser Parser { get; }

    public Novelbin()
    {
        //Use the default configuration for AngleSharp
        IConfiguration config = Configuration.Default;

        //Create a new context for evaluating webpages with the given config
        IBrowsingContext context = BrowsingContext.New(config);

        Parser = context.GetService<IHtmlParser>()!;
    }

    public string GetTitle(IDocument document)
    {
        return document.QuerySelector(".title")?.TextContent.Trim() ?? "";
    }

    public string GetDescription(IDocument document)
    {
        return document.QuerySelector(".desc-text")?.TextContent.Trim() ?? "";
    }

    public string GetAuthor(IDocument document)
    {
        var element = GetInfoBlockData(document, "Author");
        return element?.Children[1].TextContent.Trim() ?? "";
    }

    public List<string> GetAlternativeNames(IDocument document)
    {
        var element = GetInfoBlockData(document, "Alternative names");
        var content = element?.ChildNodes[2].TextContent.Trim();
        return content?.Split(",").Select((name) => name.Trim()).ToList() ?? [];
    }

    public string GetStatus(IDocument document)
    {
        var element = GetInfoBlockData(document, "Status");
        return element?.Children[1].TextContent.Trim() ?? "";
    }

    public List<string> GetGenres(IDocument document)
    {
        var element = GetInfoBlockData(document, "Genre");
        return element?.QuerySelectorAll("a").Select(a => a.TextContent.Trim()).ToList() ?? [];
    }

    public decimal GetRating(IDocument document)
    {
        var rating = document.QuerySelector("span[itemprop='ratingValue']")?.TextContent.Trim();
        return rating != null ? Convert.ToDecimal(rating) : 0;
    }

    public async Task<List<Chapter>> GetChapters(IDocument document, string bookUrl)
    {
        var chaptersUrl = document.QuerySelector("#tab-chapters-title")?.Attributes["id"]?.Value;
        List<Chapter> chapters = new List<Chapter>();
        if (chaptersUrl != null)
        {
            // for some reason the href isn't correct, but the id is, so using that
            string chaptersHtml = await Fetcher.GetPage(bookUrl + "#" + chaptersUrl);
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