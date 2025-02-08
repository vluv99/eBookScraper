using AngleSharp.Dom;

namespace eBookScraper.websites;

public interface IWebsite
{
    string BaseUrl { get; }

    Task<Book> GetBook(string url);
}