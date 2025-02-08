using System.Web;
using PuppeteerSharp;

namespace eBookScraper;

public class Fetcher
{
    public static async Task<string> GetPage(string url, bool usePuppetieer = false)
    {
        var html = "";
        var fileName = GetFileName(url);
        var filePath =
            "/home/vluv/Documents/Projects/eBookScraper/eBookScraper/scraps/" + fileName + ".html";

        if (!File.Exists(filePath))
        {
            // send GET request to url
            var httpClient = new HttpClient();

            if (usePuppetieer)
            {
                html = await UsePuppeteer(url);
            }
            else
            {
                html = httpClient.GetStringAsync(url).Result;
            }

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

    private static string GetFileName(string url)
    {
        var segments = url.Split('/');
        return segments[segments.Length - 1];
    }

    private static async Task<string> UsePuppeteer(string url)
    {
        var launchOptions = new LaunchOptions()
        {
            Headless = true
        };

        await new BrowserFetcher().DownloadAsync();
        await using (var browser = await Puppeteer.LaunchAsync(launchOptions))
        await using (var page = await browser.NewPageAsync())
        {
            await page.GoToAsync(url);
            await page.WaitForNetworkIdleAsync();
            return await page.GetContentAsync();
        }
    }
}