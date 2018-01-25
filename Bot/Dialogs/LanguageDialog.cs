using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs {

    [Serializable]
    public class LanguageDialog : IDialog<string> {
        public static readonly string English = "English";
        public static readonly string German = "Deutsch";

        public Task StartAsync(IDialogContext context) {
            PromptDialog.Choice(context, MessageReceivedAsync, new List<string> {English, German},
                "Welche Sprache möchtest du verwenden?");
            
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<string> result) {
            string resultLAnguage = await result;
            context.Done(resultLAnguage);
        }
    }
}