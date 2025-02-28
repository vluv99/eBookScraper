namespace eBookScraper;

public class Book
{
    public string Title = "";
    public string ImageUrl = "";
    public string ImagePath = "";
    public string Author = "";
    public List<string> AltNames = [];
    public List<string> Genres = [];
    public string Status = "";
    public string Description = "";
    public string Url = "";
    public List<Chapter> Chapters = [];
    public decimal Rating = 0;

    public void ConsoleContent()
    {
        Console.WriteLine("title: " + Title);
        Console.WriteLine("imageUrl: " + ImageUrl);
        Console.WriteLine("imagePath: " + ImagePath);
        Console.WriteLine("author: " + Author);
        Console.WriteLine("alt names: " + string.Join(", ", AltNames));
        Console.WriteLine("status: " + Status);
        Console.WriteLine("genre: " + string.Join(", ", Genres));
        Console.WriteLine("description: " + Description);
        Console.WriteLine("rating: " + Rating);

        Console.WriteLine("chapters: " + string.Join(", ", Chapters.Select(ch => ch.Title)));
    }
}