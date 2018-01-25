using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs {
    [Serializable]
    public class ElectionCityDialog : IDialog<string> {

        private int attempts = 2;

      
        public async Task StartAsync(IDialogContext context) {

            await context.PostAsync("Von welchem Ort möchtest du Ergebnisse wissen?");
            context.Wait(MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result) {
            var message = await result;

            if ((message.Text != null) && (message.Text.Trim().Length > 0)) {
                var cityName = message.Text;
                //todo check if cityName is a valid city! (gregors API)

                context.Done(message.Text);
            }
            else {
                context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
            }

        }

    }
}