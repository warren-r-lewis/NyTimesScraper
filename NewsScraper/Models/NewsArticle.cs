namespace NewsScraper.Models
{
    public class NewsArticle
    {

        public string title { get; set; }   
        public string url { get; set; } 

        public NewsArticle(string Title, string Url)
        {
            title = Title;
            url = Url;
        }

    }
}
