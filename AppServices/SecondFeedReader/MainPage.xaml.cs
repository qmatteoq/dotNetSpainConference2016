using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using FeedReader.Library;

namespace SecondFeedReader
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
            FeedHelper helper = new FeedHelper();
            var items = await helper.GetNewsAsync("https://blogs.windows.com/feed");
            News.ItemsSource = items;
            Loading.IsActive = false;
        }
    }
}
