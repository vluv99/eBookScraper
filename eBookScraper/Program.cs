using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using eBookScraper;
using eBookScraper.websites;

internal class Program
{
    private static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    static async Task MainAsync(string[] args)
    {
        Book book = new Book();
        book.url = "https://novelbin.me/novel-book/heaven-officials-blessing";
        string html = await Fetcher.GetPage(book.url);

        Novelbin website = new Novelbin();
        IDocument document = website.Parser.ParseDocument(html);

        // Get the title
        book.title = website.GetTitle(document);
        // Get description
        book.description = website.GetDescription(document);
        // Get book infos
        book.author = website.GetAuthor(document);
        book.altNames = website.GetAlternativeNames(document);
        book.status = website.GetStatus(document);
        book.genres = website.GetGenres(document);
        // Get rating
        book.rating = website.GetRating(document);

        // Get chapters list
        book.chapters = await website.GetChapters(document, book.url);


        book.ConsoleContent();
    }
}