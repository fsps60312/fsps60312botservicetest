using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using AdaptiveCards;

namespace Microsoft.Bot.Sample.SimpleEchoBot.Posts
{
    [Serializable]
    public class UrlReactor
    {
        private static UrlReactor Instance = new UrlReactor();
        P1995730270697235 p1995730270697235 = new P1995730270697235();
        public static async Task<bool> ReactAsync(IDialogContext context, IAwaitable<IMessageActivity> argument,IMessageActivity message)
        {
            return await Instance.Main(context, argument,message);
        }
        private async Task<bool> Main(IDialogContext context, IAwaitable<IMessageActivity> argument, IMessageActivity message)
        {
            switch (message.Text)
            {
                case "https://www.facebook.com/CodingSimplifyLife/posts/1995730270697235":
                    await p1995730270697235.MessageReceivedAsync(context, argument,message);
                    return true;
                default:
                    {
                        if (message.Text.StartsWith("http://") || message.Text.StartsWith("https://"))
                        {
                            if (message.Text.StartsWith("https://www.facebook.com/CodingSimplifyLife/posts/"))
                            {
                                await context.PostAsync("Oops......這篇文沒有彩蛋哦～試試看別篇吧XD");
                            }
                            else
                            {
                                await context.PostAsync("偷偷告訴你，以後傳某些連結給我（特別是Code風景區某些文章的連結）會有特殊反應哦！敬請期待！>///<");
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
            }
        }
    }
}