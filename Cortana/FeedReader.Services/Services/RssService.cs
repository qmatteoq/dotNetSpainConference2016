using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Web.Http;
using FeedReader.Services.Models;

namespace FeedReader.Services.Services
{
    public class RssService
    {
        public async Task<List<FeedItem>> GetNews(string url)
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(new Uri(url));
            var xdoc = XDocument.Parse(result);
            return (from item in xdoc.Descendants("item")
                    select new FeedItem
                    {
                        Title = (string)item.Element("title"),
                        Description = (string)item.Element("description"),
                        Link = (string)item.Element("link"),
                        PublishDate = DateTime.Parse((string)item.Element("pubDate"))
                    }).ToList();
        }
    }
}
