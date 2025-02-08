namespace eBookScraper;

public class Book
{
    public string title = "";
    public string author = "";
    public List<string> altNames = [];
    public List<string> genres = [];
    public string status = "";
    public string description = "";
    public string url = "";
    public List<Chapter> chapters = [];
    public decimal rating = 0;

    public void ConsoleContent()
    {
        Console.WriteLine("title: " + title);
        Console.WriteLine("author: " + author);
        Console.WriteLine("alt names: " + string.Join(", ", altNames));
        Console.WriteLine("status: " + status);
        Console.WriteLine("genre: " + string.Join(", ", genres));
        Console.WriteLine("description: " + description);
        Console.WriteLine("rating: " + rating);

        Console.WriteLine("chapters: " + string.Join(", ", chapters.Select(ch => ch.title)));
    }
}