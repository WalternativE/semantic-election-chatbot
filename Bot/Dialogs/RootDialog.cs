using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object> {

        private string cityName { get; set; }
        private string language { get; set; }


        public Task StartAsync(IDialogContext context) {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result) {
            await result;
            await SendWelcomeMessageAsync(context);


        }

        private async Task SendWelcomeMessageAsync(IDialogContext context) {
            await context.PostAsync($"Hallo ich bin dein FH Wahl Freund! Fangen wir an...");
            context.Call(new LanguageDialog(), LanguageDialogAfter);

        }

        private async Task LanguageDialogAfter(IDialogContext context, IAwaitable<string> result) {
            language = await result;
            context.Call(new ElectionCityDialog( ), this.ElectionCityDialogAfter);

        }

        private async Task ElectionCityDialogAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                cityName = await result;
                context.Call(new ElectionResultDialog(cityName, language),  ElectionResultDialogAfter);

            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Sorry, aber ich weißnicht was du genau möchtest. Fangen wir nochmals von vorne an.");

                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task ElectionResultDialogAfter(IDialogContext context, IAwaitable<string> result) {
            await context.PostAsync("So, ich hoffe das Ergebniss macht dich nicht depresiv.");
            await SendWelcomeMessageAsync(context);
        }

    }
}