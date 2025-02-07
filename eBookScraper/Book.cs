namespace eBookScraper;

class Book
{
    public string title = "";
    public string author = "";
    public List<string> altNames = [];
    public List<string> genres = [];
    public string status = "";
    public string description = "";
    public string url = "";
    public List<Chapter> chapterList = [];
    public decimal rating = 0;
}