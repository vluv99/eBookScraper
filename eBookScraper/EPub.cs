using net.vieapps.Components.Utility.Epub;

namespace eBookScraper;

public static class EPubClass
{
    public static async Task<string> Generator(Book book)
    {
        var epub = new Document();
        epub.AddBookIdentifier("bookID");
        epub.AddLanguage("English");
        epub.AddTitle(book.Title);
        epub.AddAuthor(book.Author);

        var imageBytes = await File.ReadAllBytesAsync(book.ImagePath);
        var coverImageId = epub.AddImageData("cover.jpg", imageBytes);
        epub.AddMetaItem("cover", coverImageId);

        var coverTemplate = $$"""
                              <!DOCTYPE html>
                              <html xmlns="http://www.w3.org/1999/xhtml">
                                  <head>
                                      <title>{{book.Title}}</title>
                                      <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
                                      <link type="text/css" rel="stylesheet" href="style.css"/>
                                      <style type="text/css">
                                          @page {
                                              padding: 0;
                                              margin: 0;
                                          }
                                      </style>
                                  </head>
                                  <body>
                                      {{book.Description}}
                                  </body>
                              </html>
                              """;

        // cover
        epub.AddXhtmlData("page-0.xhtml", coverTemplate);
        epub.AddNavPoint("Cover", "page-0.xhtml", 0);

        // chapter
        for (var index = 0; index < book.Chapters.Count; index++)
        {
            var name = $"page-{index + 1}.xhtml";
            var chapter = book.Chapters[index];
            var content = chapter.Content.Select(c => c.Replace("<br>", "<br/>"));

            var chapterTemplate = $$"""
                                    <!DOCTYPE html>
                                    <html xmlns="http://www.w3.org/1999/xhtml">
                                        <head>
                                            <title>{{chapter.Title}}</title>
                                            <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
                                            <link type="text/css" rel="stylesheet" href="style.css"/>
                                            <style type="text/css">
                                                @page {
                                                    padding: 0;
                                                    margin: 0;
                                                }
                                            </style>
                                        </head>
                                        <body>
                                            {{string.Join(" ", content)}}
                                        </body>
                                    </html>
                                    """;


            epub.AddXhtmlData(name, chapterTemplate);
            epub.AddNavPoint(chapter.Title, name, index + 1);
        }

        var path = Fetcher.SavePath + "/" + book.Title + ".epub";
        epub.Generate(path);
        return path;
    }
}