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
        var url = "https://novelbin.me/novel-book/heaven-officials-blessing";

        Novelbin website = new Novelbin("HOB");
        Book book = await website.GetBook(url);
        //book.ConsoleContent();

        var path = await EPubClass.Generator(book);
        Console.WriteLine(path);
    }
}