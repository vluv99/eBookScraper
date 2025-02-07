using System.Web;

namespace eBookScraper;

public class Fetcher
{
    public static string GetPage(string url)
    {
        string html = "";
        var fileName = GetFileName(url);
        var filePath =
            "/home/vluv/Documents/Projects/eBookScraper/eBookScraper/scraps/" + fileName + ".html";

        if (!File.Exists(filePath))
        {
            // send GET request to url
            var httpClient = new HttpClient();
            html = httpClient.GetStringAsync(url).Result;

            // Save to file
            File.WriteAllText(filePath, html);
        }
        else
        {
            // Read from file
            html = File.ReadAllText(filePath);
        }

        return html;
    }

    static string GetFileName(string url)
    {
        var uri = new Uri(url);
        return HttpUtility.UrlDecode(uri.Segments[uri.Segments.Length - 1]);
    }
}