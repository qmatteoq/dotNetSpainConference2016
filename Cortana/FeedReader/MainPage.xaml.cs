using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using FeedReader.Services.Services;

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

            StorageFile file = await Package.Current.InstalledLocation.GetFileAsync("VoiceCommands.xml");
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(file);

            RssService service = new RssService();
            var items = await service.GetNews("http://blog.qmatteoq.com/feed");
            News.ItemsSource = items;
            Loading.IsActive = false;
        }
    }
}
