using System.Net;
using net.vieapps.Components.Utility.Epub;

namespace eBookScraper;

public class EPubClass
{
    public async static Task<string> Generator(Book book)
    {
        var epub = new Document();
        epub.AddBookIdentifier("bookID");
        epub.AddLanguage("English");
        epub.AddTitle(book.Title);
        epub.AddAuthor(book.Author);

        var client = new HttpClient();
        var imageBytes = await client.GetByteArrayAsync(book.ImageUrl);
        var coverImageId = epub.AddImageData("cover.jpg", imageBytes);
        epub.AddMetaItem("cover", coverImageId);

        var pageTemplate = @"<!DOCTYPE html>
			<html xmlns=""http://www.w3.org/1999/xhtml"">
				<head>
					<title>{0}</title>
					<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>
					<link type=""text/css"" rel=""stylesheet"" href=""style.css""/>
					<style type=""text/css"">
						@page {
							padding: 0;
							margin: 0;
						}
					</style>
				</head>
				<body>
					{1}
				</body>
			</html>".Trim().Replace("\t", "");

        // cover
        epub.AddXhtmlData("page-0.xhtml",
            pageTemplate.Replace("{0}", book.Title).Replace("{1}", book.Description));

        // chapter
        for (var index = 0; index < book.Chapters.Count; index++)
        {
            var name = $"page-{index + 1}.xhtml";
            var chapter = book.Chapters[index];

            epub.AddXhtmlData(name,
                pageTemplate.Replace("{0}", chapter.Title).Replace("{1}", string.Join(" ", chapter.Content)));
            epub.AddNavPoint(chapter.Title, name,
                index + 1);
        }

        var path = Fetcher.SavePath + "/" + book.Title + ".epub";
        epub.Generate(path);
        return path;
    }
}