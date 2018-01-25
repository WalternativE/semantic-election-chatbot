using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chronic;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
	[Serializable]
	public class ElectionResultDialog : IDialog<string>
	{
		private string CityName { get; }
	    private string language { get; set; }

        public ElectionResultDialog(string cityName, string language) {
            CityName = cityName;
            this.language = language;
        }

		public async Task StartAsync(IDialogContext context)
		{
			await context.PostAsync($"Wir suchen die ergebnisse für {CityName}");

			await SearchResults(context);

			context.Done("");
		}

		private async Task SearchResults(IDialogContext context)
		{
			//todo search results from Gregors API
			var res = BotDataProvider.Provider.Get2017Results(language.Equals(LanguageDialog.English) ? "en" : "de");

			await context.PostAsync("Die Ergebnisse wurden gefunden.");
            res.OrderBy(x => x.BallotCount).ForEach(async party => {
                await context.PostAsync($"{party.PartyLabel} => {party.BallotCount} Stimmen");
            });

		}

	}
}