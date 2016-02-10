using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using FeedReader.Parser.Models;
using Newtonsoft.Json;

namespace FeedReader
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Loading.IsActive = true;
            AppServiceConnection connection = new AppServiceConnection();
            connection.PackageFamilyName = Package.Current.Id.FamilyName;
            connection.AppServiceName = "FeedParser";

            var status = await connection.OpenAsync();

            if (status == AppServiceConnectionStatus.Success)
            {
                ValueSet data = new ValueSet();
                data.Add("FeedUrl", "http://blog.qmatteoq.com/feed/");

                var response = await connection.SendMessageAsync(data);
                if (response.Status == AppServiceResponseStatus.Success)
                {
                    string items = response.Message["FeedItems"].ToString();
                    var result = JsonConvert.DeserializeObject<List<FeedItem>>(items);
                    News.ItemsSource = result;
                }
            }

            Loading.IsActive = false;
        }
    }
}
