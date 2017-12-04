using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;


namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }
        // Azure page: https://portal.azure.com/#blade/WebsitesExtension/BotsIFrameBlade/id/%2Fsubscriptions%2Fed3b27fa-21db-4e94-8061-2d654c6b87d5%2FresourceGroups%2Ffsps60312botservicetest%2Fproviders%2FMicrosoft.Web%2Fsites%2Ffsps60312botservicetest
        // Unicode convert: https://www.ifreesite.com/unicode-ascii-ansi.htm
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.PostAsync("\u4f60\u8aaa\u4e86\u300c"/*§A»¡¤F¡u*/ + message.Text + "\u300d"/*¡v*/);
            if (message.Text=="What you know about me?")
            {
                await context.PostAsync(context.UserData.GetType().ToString());
                await context.PostAsync(JsonConvert.SerializeObject(context.UserData));
                await context.PostAsync(context.UserData.GetValue<string>("Id"));
                await context.PostAsync(context.UserData.GetValue<string>("Name"));
                await context.PostAsync(context.UserData.GetValue<string>("Properties"));
                await context.PostAsync($"Id: {message.From.Id}\n" + $"Name: {message.From.Name}\n" + $"Properties: {message.From.Properties.ToString()}");
            }
            //StringBuilder sb = new StringBuilder();
            //foreach (var v in context.UserData as System.Collections.IEnumerable) sb.Append(v);
            context.Wait(MessageReceivedAsync);
        }
    }
}