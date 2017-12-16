//https://www.facebook.com/CodingSimplifyLife/posts/2002954469974815
using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using AdaptiveCards;
using System.Linq;

namespace Microsoft.Bot.Sample.SimpleEchoBot.Posts
{
    [Serializable]
    public class P2002954469974815 : MyDialog<IMessageActivity>
    {
        protected override async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.PostAsync("哼，證明題是吧？您傳錯篇網址囉！ :p <br/>請再仔細看那篇文～");
            context.Done(message = null);
        }
    }
}