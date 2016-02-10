using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using FeedReader.Services.Models;
using FeedReader.Services.Services;

namespace FeedReader.Cortana
{
    public sealed class SpeechTask: IBackgroundTask
    {
        private VoiceCommandServiceConnection _voiceServiceConnection;
        private BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            if (triggerDetails != null && triggerDetails.Name == "FeedReaderVoiceCommandService")
            {
                _voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                _voiceServiceConnection.VoiceCommandCompleted += VoiceServiceConnection_VoiceCommandCompleted;
                var voiceCommand = await _voiceServiceConnection.GetVoiceCommandAsync();
                if (voiceCommand?.CommandName == "ShowLatestNews")
                {
                    await ShowLatestNews();
                }
            }
        }

        private async Task ShowLatestNews()
        {
            string progress = "Getting the latest news...";
            await ShowProgressScreen(progress);
            RssService feedService = new RssService();
            var news = await feedService.GetNews("http://blog.qmatteoq.com/feed");

            List<VoiceCommandContentTile> contentTiles = new List<VoiceCommandContentTile>();

            VoiceCommandUserMessage message = new VoiceCommandUserMessage();
            string text = "Here are the latest news";
            message.DisplayMessage = text;
            message.SpokenMessage = text;

            foreach (FeedItem item in news.Take(5))
            {
                VoiceCommandContentTile tile = new VoiceCommandContentTile();
                tile.ContentTileType = VoiceCommandContentTileType.TitleOnly;
                tile.Title = item.Title;
                tile.TextLine1 = item.PublishDate.ToString("g");

                contentTiles.Add(tile);
            }

            VoiceCommandResponse response = VoiceCommandResponse.CreateResponse(message, contentTiles);
            await _voiceServiceConnection.ReportSuccessAsync(response);

        }

        private async Task ShowProgressScreen(string message)
        {
            var userProgressMessage = new VoiceCommandUserMessage();
            userProgressMessage.DisplayMessage = userProgressMessage.SpokenMessage = message;

            VoiceCommandResponse response = VoiceCommandResponse.CreateResponse(userProgressMessage);
            await _voiceServiceConnection.ReportProgressAsync(response);
        }

        private void VoiceServiceConnection_VoiceCommandCompleted(VoiceCommandServiceConnection sender, VoiceCommandCompletedEventArgs args)
        {
            _deferral?.Complete();
        }
    }
}
