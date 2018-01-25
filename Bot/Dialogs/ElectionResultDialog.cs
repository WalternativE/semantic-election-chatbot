using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot.Dialogs
{
	[Serializable]
	public class ElectionResultDialog : IDialog<string>
	{
		private string CityName { get; }

		public ElectionResultDialog(string cityName)
		{
			CityName = cityName;
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
			Thread.Sleep(TimeSpan.FromSeconds(4)); //fake api call
			var res = BotDataProvider.Provider.Get2017Results();

			await context.PostAsync("Die Ergebnisse wurden gefunden.");
			await context.PostAsync("Österreich ist generell sehr rassistisch!!!");
		}

	}
}