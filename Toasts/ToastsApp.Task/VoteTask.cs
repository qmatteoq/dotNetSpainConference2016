using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ToastsApp.Task
{
    public sealed class VoteTask: IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            string arguments = details.Argument;
            var result = details.UserInput;

            if (arguments == "vote")
            {
                string value = result["rating"].ToString();
                int vote = int.Parse(value);
                string message;
                if (vote <= 3)
                {
                    message = "Sorry, there was a problem sending your vote :-)";
                }
                else
                {
                    message = "Thanks for voting!";
                }

                ToastNotification toast = PrepareToast(message);
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }

        private ToastNotification PrepareToast(string message)
        {
            string xml = $@"<toast>
                              <visual>
                                <binding template=""ToastGeneric"">
                                  <image placement=""appLogoOverride"" src=""Assets/MicrosoftLogo.png"" />
                                  <text>{message}</text>
                                </binding>
                              </visual>
                            </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            ToastNotification notification = new ToastNotification(doc);
            return notification;
        }
    }
}
