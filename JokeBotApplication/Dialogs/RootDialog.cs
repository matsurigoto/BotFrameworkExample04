using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using RestSharp;

namespace JokeBotApplication.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text.IndexOf("笑話") >= 0 || activity.Text.IndexOf("joke") >= 0)
            {
                var client = new RestClient("https://icanhazdadjoke.com");
                var request = new RestRequest("", Method.GET);

                var response = await client.ExecuteTaskAsync<RootObject>(request);      
                await context.PostAsync($"{response.Data.Joke}");
            }

            context.Wait(MessageReceivedAsync);
        }
    }
    public class RootObject
    {
        public string Id { get; set; }
        public string Joke { get; set; }
        public int Status { get; set; }

    }
}