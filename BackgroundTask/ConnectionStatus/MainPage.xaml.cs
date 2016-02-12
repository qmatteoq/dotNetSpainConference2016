using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ConnectionStatus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (BackgroundTaskRegistration.AllTasks.All(x => x.Value.Name != "ConnectionTask"))
            {
                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = "ConnectionTask";
                taskBuilder.TaskEntryPoint = "ConnectionStatus.Task.ConnectionTask";
                taskBuilder.SetTrigger(new SystemTrigger(SystemTriggerType.NetworkStateChange, false));
                var status = await BackgroundExecutionManager.RequestAccessAsync();
                if (status != BackgroundAccessStatus.Denied)
                {
                    taskBuilder.Register();
                }
            }
        }
    }
}
