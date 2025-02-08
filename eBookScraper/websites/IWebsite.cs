using AngleSharp.Dom;

namespace eBookScraper.websites;

public interface IWebsite
{
    string BaseUrl { get; }

    string GetTitle(IDocument document);
    string GetDescription(IDocument document);
    string GetAuthor(IDocument document);
    List<string> GetAlternativeNames(IDocument document);
    string GetStatus(IDocument document);
    List<string> GetGenres(IDocument document);
    Decimal GetRating(IDocument document);
    Task<List<Chapter>> GetChapters(IDocument document, string bookUrl);
}