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
                            await context.PostAsync("\u5077\u5077\u544a\u8a34\u4f60\uff0c\u4ee5\u5f8c\u50b3\u67d0\u4e9b\u9023\u7d50\u7d66\u6211\uff08\u7279\u5225\u662fCode\u98a8\u666f\u5340\u67d0\u4e9b\u6587\u7ae0\u7684\u9023\u7d50\uff09\u6703\u6709\u7279\u6b8a\u53cd\u61c9\u54e6\uff01\u656c\u8acb\u671f\u5f85\uff01>///<"/*偷偷告訴你，以後傳某些連結給我（特別是Code風景區某些文章的連結）會有特殊反應哦！敬請期待！>///<*/);
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