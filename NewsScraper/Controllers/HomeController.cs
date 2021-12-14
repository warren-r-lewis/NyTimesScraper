using Microsoft.AspNetCore.Mvc;
using NewsScraper.Models;
using System.Diagnostics;

namespace NewsScraper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        { 
            _logger = logger;
        }

        public IActionResult Index()
        {
            var AllArticles = RetrieveNews();
            ViewData["Articles"] = AllArticles;
            return View(AllArticles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }
        public static List<NewsArticle> RetrieveNews()
        {
            var nytimes = "https://www.nytimes.com/";
            var ny_response = CallUrl(nytimes).Result;
            HtmlDocument ny_htmlDocument = new HtmlDocument();
            ny_htmlDocument.LoadHtml(ny_response);
            var Nodes = new List<HtmlNode>();
            Nodes = ny_htmlDocument.DocumentNode.SelectNodes("//h3").ToList(); //.Where(x=>x.InnerHtml.Contains("story-wrapper")).ToList();
            var news_articles=new List<NewsArticle>();
            foreach (var node in Nodes)
            {
                string title = node.InnerText;
                string url = "No Url";
                var urlParent = node.ParentNode.Ancestors();
                foreach(var urlNode in urlParent)
                {
                    if (urlNode.Name == "a")
                    {
                        url = urlNode.Attributes["href"].Value;
                    }
                }

                var article = new NewsArticle(title, url);
                news_articles.Add(article);
            }
            return news_articles;
        }
        
    }
}