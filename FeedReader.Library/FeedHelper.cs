using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using FeedReader.Library.Models;
using Newtonsoft.Json;

namespace FeedReader.Library
{
    public class FeedHelper
    {
        public async Task<IEnumerable<FeedItem>> GetNewsAsync(string url)
        {
            AppServiceConnection connection = new AppServiceConnection();
            connection.PackageFamilyName = "637bc306-acb1-4e73-bbe0-70ecc919ca1c_e8f4dqfvn1be6";
            connection.AppServiceName = "FeedParser";

            var status = await connection.OpenAsync();

            if (status == AppServiceConnectionStatus.Success)
            {
                ValueSet data = new ValueSet();
                data.Add("FeedUrl", url);

                var response = await connection.SendMessageAsync(data);
                if (response.Status == AppServiceResponseStatus.Success)
                {
                    string items = response.Message["FeedItems"].ToString();
                    var result = JsonConvert.DeserializeObject<List<FeedItem>>(items);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
