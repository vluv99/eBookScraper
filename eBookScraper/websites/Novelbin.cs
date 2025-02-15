using AngleSharp;
using AngleSharp.Dom;
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
        var config = Configuration.Default;

        //Create a new context for evaluating webpages with the given config
        var context = BrowsingContext.New(config);

        Parser = context.GetService<IHtmlParser>()!;
        Prefix = prefix;
    }

    public async Task<Book> GetBook(string url)
    {
        var book = new Book { Url = url };
        var html = await Fetcher.GetPage(url, Prefix);
        IDocument document = Parser.ParseDocument(html);

        // Get the title
        book.Title = GetTitle(document);
        // Get the image url
        book.ImageUrl = GetImageUrl(document);
        // Get description
        book.Description = GetDescription(document);
        // Get book infos
        book.Author = GetAuthor(document);
        book.AltNames = GetAlternativeNames(document);
        book.Status = GetStatus(document);
        book.Genres = GetGenres(document);
        // Get rating
        book.Rating = GetRating(document);

        // Get chapters list
        book.Chapters = await GetChapters(document, book.Url);

        return book;
    }

    private string GetTitle(IDocument document)
    {
        return document.QuerySelector(".title")?.TextContent.Trim() ?? "";
    }

    private string GetImageUrl(IDocument document)
    {
        var e = document.QuerySelector(".book > img");
        return e?.Attributes.First(attr => attr.Name == "data-src").Value ?? "";
    }

    private string GetDescription(IDocument document)
    {
        return document.QuerySelector(".desc-text")?.TextContent.Trim() ?? "";
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
        var chapters = new List<Chapter>();
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
                    var chapter = new Chapter
                    {
                        Title = a.TextContent.Trim().Replace("\u00A0", "").Replace("\u200C", ""),
                        Url = a.Attributes["href"]?.Value ?? ""
                    };
                    return chapter;
                }).ToList();
                chapters.AddRange(res ?? []);
            }
        }


        // fetch chapters
        foreach (var chapter in chapters)
        {
            chapter.Content.AddRange(await GetChapterContent(chapter));
        }

        return chapters;
    }

    private async Task<List<string>> GetChapterContent(Chapter chapter)
    {
        var content = new List<string>();
        if (chapter.Url != "")
        {
            var html = await Fetcher.GetPage(chapter.Url, "HOB");
            IDocument chapterDocument = Parser.ParseDocument(html);
            content = chapterDocument
                .QuerySelectorAll("#chr-content > p")
                .Select((p) =>
                {
                    var containsImg = p.QuerySelector("img");
                    if (containsImg != null)
                    {
                        p.RemoveChild(containsImg);
                    }

                    return p;
                })
                .Select(p =>
                {
                    if (String.IsNullOrWhiteSpace(p.InnerHtml))
                    {
                        return "";
                    }

                    return p.OuterHtml.Trim();
                })
                .ToList();
        }

        return content;
    }
}