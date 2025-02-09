using AngleSharp.Dom;

namespace eBookScraper.websites;

public interface IWebsite
{
    string WebsiteName { get; }

    Task<Book> GetBook(string url);
}