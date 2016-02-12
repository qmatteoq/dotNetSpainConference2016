using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Networking.Connectivity;
using Windows.UI.Notifications;

namespace ConnectionStatus.Task
{
    public sealed class ConnectionTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            NetworkConnectivityLevel level = connectionProfile.GetNetworkConnectivityLevel();
            var message = level == NetworkConnectivityLevel.InternetAccess ? "Connected to Internet" : "Not connected to Internet";

            string xml = $@"
            <toast activationType='foreground' launch='args'>
                <visual>
                    <binding template='ToastGeneric'>
                        <text>{message}</text>
                    </binding>
                </visual>
            </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            ToastNotification notification = new ToastNotification(doc);
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(notification);
        }
    }
}
