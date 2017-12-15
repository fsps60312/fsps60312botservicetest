using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class BasicJudge : MyDialog<IMessageActivity>
    {
        protected override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            try
            {
                if (message.Text == null)
                {
                    await context.PostAsync("吼～都這樣，不說點話嗎？><");
                    //Main.MarkContextCompleted(message);
                    message=null;
                }
                else if (message.Text.Length > 250)
                {
                    await context.PostAsync("???");
                    //Main.MarkContextCompleted(message);
                    message=null;
                }
            }
            finally { context.Done(message); }
        }
    }
}
