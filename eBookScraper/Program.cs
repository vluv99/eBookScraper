using eBookScraper;
using eBookScraper.websites;

internal class Program
{
    private static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    private static async Task MainAsync(string[] args)
    {
        const string url = "https://novelbin.me/novel-book/heaven-officials-blessing";

        var website = new Novelbin("HOB");
        var book = await website.GetBook(url);
        //book.ConsoleContent();

        var path = await EPubClass.Generator(book);
        Console.WriteLine(path);
    }
}