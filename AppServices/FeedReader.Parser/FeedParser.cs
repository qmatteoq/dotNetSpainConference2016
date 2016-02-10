using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Data.Json;
using Windows.Foundation.Collections;
using Windows.Web.Http;
using FeedReader.Parser.Models;
using Newtonsoft.Json;

namespace FeedReader.Parser
{
    public sealed class FeedParser: IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            if (triggerDetails.Name == "FeedParser")
            {
                triggerDetails.AppServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
            }
        }

        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var appServiceDeferral = args.GetDeferral();
            var message = args.Request.Message;
            string uri = message["FeedUrl"].ToString();

            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(new Uri(uri));
            List<FeedItem> items = GetNews(result);
            string serializedResult = JsonConvert.SerializeObject(items);

            ValueSet response = new ValueSet();
            response.Add("FeedItems", serializedResult);

            await args.Request.SendResponseAsync(response);
            appServiceDeferral.Complete();
        }

        private List<FeedItem> GetNews(string data)
        {
            var xdoc = XDocument.Parse(data);
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
